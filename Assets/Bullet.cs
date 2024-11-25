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

    public int SetDamage = 25;

    public void Damage(EnemyHealth Target)
    {
        CameraBehaviouerControll.instance.LookAtTarget(Target.transform);
        EFfect.transform.parent = null;
        EFfect.gameObject.SetActive(true);
        EFfect.GetComponent<DestroySelf>().turret = TurretAnimation;
        Target.TakeDamage(SetDamage);


        if (TurnBaseSystem.instance.currentTurn == Turn.Player)
        {
            TurnBaseSystem.instance.currentTurn = Turn.Player;
        }

        Destroy(this.gameObject);
    }
}
