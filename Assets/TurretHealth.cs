public class TurretHealth : Health
{
    public override void OnDied()
    {
        this.GetComponent<Character>().characterTile.occupyingCharacter = null;
        this.GetComponent<Character>().characterTile.Occupied = false;


        Destroy(this.gameObject);

        EntityTurnBehaviour turnBehaviour = this.GetComponent<EntityTurnBehaviour>();

        TurnBaseSystem.instance.TurretTurn.Remove(turnBehaviour);

    }
}
