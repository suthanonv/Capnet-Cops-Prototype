using UnityEngine;

public class AttackingRadius : MonoBehaviour
{
    public GameObject EnemyToAttack;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<EntityTeam>(out EntityTeam team))
        {
            if (team.EntityTeamSide == Team.Enemy)
            {
                EnemyToAttack = other.gameObject;
                // Trigger the attack when an enemy enters the collider
                this.transform.parent.GetComponent<EntityTurnBehaviour>().onTurn();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<EntityTeam>(out EntityTeam team))
        {
            if (team.EntityTeamSide == Team.Enemy && EnemyToAttack == other.gameObject)
            {
                EnemyToAttack = null;
                // Trigger the attack again when the enemy leaves the collider
                this.transform.parent.GetComponent<EntityTurnBehaviour>().onTurn();
            }
        }
    }
}
