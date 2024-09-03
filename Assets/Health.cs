using UnityEngine;

public class Health : MonoBehaviour
{
    public int Maxhealth;


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

        TurnBaseSystem.instance.turnSystems.Remove(this.GetComponent<EnemyTurnBehaviour>());
        Destroy(this.gameObject);

    }
}
