using UnityEngine;

public class TurretAnimation : AnimationControll
{
    [SerializeField] Bullet BulletPrefab;
    [SerializeField] Transform ShootingPoint;
    [SerializeField] float Speed = 20;
    [SerializeField] bool NeedReduceActionPoint;
    public override void onStartAttacking()
    {
        CameraBehaviouerControll.instance.LookAtTarget(this.transform.parent);
        base.onStartAttacking();
    }
    public override void Attacking()
    {
        if (Target != null)
        {
            if (Target.gameObject.GetComponent<EntityTeam>().TypeOfTarget != global::Target.Pod)
            {
                Bullet bullet = Instantiate(BulletPrefab, ShootingPoint.transform.position, Quaternion.identity);

                bullet.SetTarget = Target.gameObject;
                bullet.TurretAnimation = this;

                if (NeedReduceActionPoint)
                {
                    this.transform.parent.GetComponent<EntityTurnBehaviour>().Status.AvalibleActionPoint -= 1;
                }

                bullet.GetComponent<Rigidbody>().AddForce(ShootingPoint.transform.forward * Speed, ForceMode.Impulse);
                bullet.SetDamage = this.transform.parent.gameObject.GetComponent<EntityTurnBehaviour>().Status.BaseDamage;
                CameraBehaviouerControll.instance.LookAtTarget(bullet.transform);
            }
            else
            {
                this.transform.parent.GetComponent<Character>().FinalizePosition(this.transform.parent.GetComponent<Character>().characterTile);
                Target.GetComponent<Character>().characterTile.InteractAble = true;
                Target.TakeDamage(EntityTurn.Status.BaseDamage);

                EndAction();


            }

        }
    }

    public void CollectingPod()
    {

    }
}
