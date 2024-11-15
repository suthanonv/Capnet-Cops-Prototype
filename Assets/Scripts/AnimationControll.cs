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
            Debug.Log(this.transform.parent.GetComponent<EntityTurnBehaviour>().Status.AvalibleActionPoint);
            this.transform.parent.GetComponent<Character>().FinalizePosition(this.transform.parent.GetComponent<Character>().characterTile);
            Target.GetComponent<Character>().characterTile.InteractAble = true;
            Target.TakeDamage(EntityTurn.Status.BaseDamage);
        }
    }

    public void EndAction()
    {
        EntityTurn.OnActionEnd();

    }
}
