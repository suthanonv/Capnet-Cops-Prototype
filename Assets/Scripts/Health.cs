using UnityEngine;

public class Health : MonoBehaviour
{
    public int Maxhealth;
    EnemyTurnBehaviour turnBehaviour;
    Team EntityTeam;
    [SerializeField] Animator anim;

    public virtual void Start()
    {
        turnBehaviour = this.GetComponent<EnemyTurnBehaviour>();
        EntityTeam = this.GetComponent<EntityTeam>().EntityTeamSide;
    }

    public virtual void TakeDamage(int Damage)
    {
        Maxhealth -= Damage;


        if (Maxhealth <= 0)
        {
            Died();
        }
        else
        {

            anim.SetTrigger("Hurt");
            this.transform.GetChild(1).GetComponent<MaterialChange>().OnHitMeterial();
        }
    }


    public virtual void Died()
    {

        anim.SetTrigger("Died");


    }


    public virtual void OnDied()
    {
        this.GetComponent<Character>().characterTile.occupyingCharacter = null;
        this.GetComponent<Character>().characterTile.Occupied = false;

        TurnBaseTurnVisual.Instance.RemoveImageFromTurnVisual(this.gameObject.GetComponent<EntityTurnBehaviour>());

        Destroy(this.gameObject);


        if (EntityTeam == Team.Enemy) TurnBaseSystem.instance.enemiesTurnSystems.Remove(turnBehaviour);
        else TurnBaseSystem.instance.playerTurnSystems.Remove(turnBehaviour);

        TurnBaseSystem.instance.UpdateCombatPhase();
    }

}
