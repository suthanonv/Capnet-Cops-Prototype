using UnityEngine;
public class AnimationControll : MonoBehaviour
{

    public Health Target { get; set; }

    protected EntityTurnBehaviour EntityTurn;

    private void Start()
    {
        EntityTurn = transform.parent.GetComponent<EntityTurnBehaviour>();
    }


    public void onStartAttacking()
    {
        TurnBaseSystem.instance.PlayerInteractScript.enabled = false;
    }
    public virtual void Attacking()
    {
        if (Target != null)
        {
            ShowMoveingRange.instance.CloseMovingRangeVisual();
            this.transform.parent.GetComponent<EntityTurnBehaviour>().Status.AvalibleActionPoint -= 1;
            this.transform.parent.GetComponent<Character>().FinalizePosition(this.transform.parent.GetComponent<Character>().characterTile);
            Target.GetComponent<Character>().characterTile.InteractAble = true;
            Target.TakeDamage(EntityTurn.Status.BaseDamage);


            if (PausedChar != null)
            {
                PausedChar.Paused = false;
            }
        }
    }

    public void EndAction()
    {
        EntityTurn.OnActionEnd();

    }

    Character PausedChar;
    public void PauseTarGet()
    {
        if (Target != null)
        {
            PausedChar = Target.GetComponent<Character>();
            PausedChar.Paused = true;
        }
    }
}
