using UnityEngine;

public class AttackingRadius : MonoBehaviour
{
    public GameObject EnemyToAttack;

    private void OnTriggerEnter(Collider other)
    {
        if (EnemyToAttack != null)
        {
            if (EnemyToAttack.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth health))
            {
                health.RemoveBulletQuque();
            }
        }


        if (other.gameObject.TryGetComponent<EntityTeam>(out EntityTeam team) &&
            team.EntityTeamSide == Team.Enemy &&
            other.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth healths) &&
            healths.CanbeTarget())
        {
            // Set the enemy to attack and initiate the attack
            EnemyToAttack = other.gameObject;
            healths.SetBulletQuque();
            this.transform.parent.GetComponent<EntityTurnBehaviour>().onTurn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (EnemyToAttack != null)
        {
            if (EnemyToAttack.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth health))
            {
                health.RemoveBulletQuque();
            }
        }

        if (other.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth healths))
        {
            if (healths.CanbeTarget())
            {
                healths.SetBulletQuque();
                EnemyToAttack = other.gameObject;
                this.transform.parent.GetComponent<EntityTurnBehaviour>().onTurn();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (EnemyToAttack == null &&
            other.gameObject.TryGetComponent<EntityTeam>(out EntityTeam team) &&
            team.EntityTeamSide == Team.Enemy &&
            other.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth healths) &&
            healths.CanbeTarget())
        {
            // Set the enemy to attack if within range and no current target
            EnemyToAttack = other.gameObject;
            healths.SetBulletQuque();
        }
    }
}
