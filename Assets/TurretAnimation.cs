using UnityEngine;

public class TurretAnimation : AnimationControll
{
    [SerializeField] Bullet BulletPrefab;
    [SerializeField] Transform ShootingPoint;
    [SerializeField] float Speed = 20;

    public override void onStartAttacking()
    {
        CameraBehaviouerControll.instance.LookAtTarget(this.transform.parent);
        base.onStartAttacking();
    }
    public override void Attacking()
    {
        if (Target != null)
        {
            Bullet bullet = Instantiate(BulletPrefab, ShootingPoint.transform.position, Quaternion.identity);

            bullet.SetTarget = Target.gameObject;
            bullet.TurretAnimation = this;

            bullet.GetComponent<Rigidbody>().AddForce(ShootingPoint.transform.forward * Speed, ForceMode.Impulse);

            CameraBehaviouerControll.instance.LookAtTarget(bullet.transform);
        }
    }
}
