using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Transform EFfect;
    public GameObject SetTarget;
    public TurretAnimation TurretAnimation;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth health))
        {
            Debug.Log("HIt");

            if (other.gameObject != SetTarget)
            {
                health.RemoveBulletQuque();
            }

            Damage(health);
        }
    }

    public void Damage(EnemyHealth Target)
    {
        CameraBehaviouerControll.instance.LookAtTarget(Target.transform);
        EFfect.transform.parent = null;
        EFfect.gameObject.SetActive(true);
        EFfect.GetComponent<DestroySelf>().turret = TurretAnimation;
        Target.TakeDamage(TurretDamage.Instance.Damage);


        Destroy(this.gameObject);
    }
}
