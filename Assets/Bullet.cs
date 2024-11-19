using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Transform EFfect;
    public GameObject SetTarget;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth health))
        {
            if (other.gameObject != SetTarget)
            {
                SetTarget.GetComponent<EnemyHealth>().RemoveBulletQuque();
            }

            Damage(health);
        }
    }

    public void Damage(EnemyHealth Target)
    {
        Target.TakeDamage(TurretDamage.Instance.Damage);
        EFfect.transform.parent = null;
        Destroy(this.gameObject);
        Destroy(EFfect, 1);
    }
}
