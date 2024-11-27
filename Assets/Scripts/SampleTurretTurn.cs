using UnityEngine;

public class SampleTurretTurn : EntityTurnBehaviour
{
    private Pathfinder pathfinder;
    private Character Char;
    private EntityTurnBehaviour turnBehaviour;
    private AnimationControll animC;

    private Animator anim;
    public AudioClip shot1;
    AudioSource audioSource;



    public AttackingRadius attackingRadius;



    protected override void Start()
    {
        base.Start();

        audioSource = GetComponent<AudioSource>();
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
        if (this == null)
        {
            return;
        }
        Debug.Log("Turn Play");
        if (Target == null)
        {
            OnActionEnd();
            return;
        }
        animC.Target = Target.GetComponent<EnemyHealth>();
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(shot1, 0.7f);
        anim.SetTrigger("Attacking");
       

    }


    public override void OnActionEnd()
    {
        Debug.Log("ENDDDDDDDDDDDDD");
        TurretQuque.instance.ActionEnd = true;
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
