using UnityEngine;

public class SampleTurretTurn : EntityTurnBehaviour
{
    private Pathfinder pathfinder;
    private Character Char;
    private EntityTurnBehaviour turnBehaviour;
    private AnimationControll animC;

    private Animator anim;




    [SerializeField] AttackingRadius attackingRadius;



    protected override void Start()
    {
        base.Start();


        if (pathfinder == null)
            pathfinder = GameObject.Find("Pathfinder").GetComponent<Pathfinder>();

        Char = this.GetComponent<Character>();

        turnBehaviour = this.GetComponent<EntityTurnBehaviour>();
        TurnBaseSystem.instance.TurretTurn.Add(this);
        anim = this.transform.GetChild(1).GetComponent<Animator>();
        animC = this.transform.GetChild(1).GetComponent<AnimationControll>();


        Status.BaseDamage = TurretDamage.Instance.Damage;
    }




    public override void onTurn()
    {

        if (attackingRadius.EnemyToAttack == null) return;

        animC.Target = attackingRadius.EnemyToAttack.GetComponent<EnemyHealth>();
        if (animC.Target == null) return;
        anim.SetTrigger("Attacking");

    }


    private void Update()
    {
        if (attackingRadius.EnemyToAttack != null)
        {
            transform.rotation = Quaternion.LookRotation(Char.transform.position.DirectionTo(attackingRadius.EnemyToAttack.transform.position).Flat(), Vector3.up);

        }
        else
        {
            this.transform.rotation = Quaternion.identity;
        }
    }



}
