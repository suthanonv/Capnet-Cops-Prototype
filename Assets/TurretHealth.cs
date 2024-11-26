using UnityEngine;
public class TurretHealth : Health
{
    [SerializeField] GameObject Effect;
    public override void OnDied()
    {
        this.GetComponent<Character>().characterTile.occupyingCharacter = null;
        this.GetComponent<Character>().characterTile.Occupied = false;

        Effect.transform.parent = null;
        Effect.gameObject.SetActive(true);

        Destroy(this.gameObject);

        EntityTurnBehaviour turnBehaviour = this.GetComponent<EntityTurnBehaviour>();

        TurnBaseSystem.instance.TurretTurn.Remove(turnBehaviour);

    }
}
