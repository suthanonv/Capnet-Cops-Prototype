using UnityEngine;

public class Health : MonoBehaviour
{
    public int Maxhealth;
    EnemyTurnBehaviour turnBehaviour;

    private void Start()
    {
        turnBehaviour = this.GetComponent<EnemyTurnBehaviour>();
    }

    public void TakeDamage(int Damage)
    {
        Maxhealth -= Damage;
        if (Maxhealth <= 0)
        {
            Died();
        }
    }


    void Died()
    {


        this.GetComponent<Character>().characterTile.occupyingCharacter = null;
        this.GetComponent<Character>().characterTile.Occupied = false;

        TurnBaseTurnVisual.Instance.RemoveImageFromTurnVisual(this.gameObject.GetComponent<EntityTurnBehaviour>());

        Destroy(this.gameObject);


        TurnBaseSystem.instance.turnSystems.Remove(turnBehaviour);


    }


}
