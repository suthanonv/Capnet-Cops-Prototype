using UnityEngine;

public class SampleTurretTurn : EntityTurnBehaviour
{
    private Pathfinder pathfinder;
    private Character Char;
    private EntityTurnBehaviour turnBehaviour;
    private AnimationControll animC;

    private Animator anim;




    public AttackingRadius attackingRadius;



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


    GameObject Target;
    public void SetTarget(GameObject Target)
    {
        this.Target = Target;
    }

    public override void onTurn()
    {
        Debug.Log(Target == null);
        if (Target == null) return;
        animC.Target = Target.GetComponent<EnemyHealth>();
        anim.SetTrigger("Attacking");

    }



    private void Update()
    {
        if (Target != null)
        {
            transform.rotation = Quaternion.LookRotation(Char.transform.position.DirectionTo(Target.transform.position).Flat(), Vector3.up);

        }
        else
        {
            this.transform.rotation = Quaternion.identity;
        }
    }



}
