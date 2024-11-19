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

        CurrentFreme = 0;

    }
    public virtual void Attacking()
    {
        Debug.Log($"Target {Target == null}");
        if (Target != null)
            ShowMoveingRange.instance.CloseMovingRangeVisual();
        this.transform.parent.GetComponent<EntityTurnBehaviour>().Status.AvalibleActionPoint -= 1;
        this.transform.parent.GetComponent<Character>().FinalizePosition(this.transform.parent.GetComponent<Character>().characterTile);
        Target.GetComponent<Character>().characterTile.InteractAble = true;
        Target.TakeDamage(EntityTurn.Status.BaseDamage);




    }

    Character PausingChar;
    public void PauseCharacter()
    {
        PausingChar = Target.GetComponent<Character>();
        PausingChar.Paused = true;

    }

    public void UnPauseCharacter()
    {
        if (PausingChar != null)
        {
            PausingChar.Paused = false;
        }
    }


    public void EndAction()
    {
        EntityTurn.OnActionEnd();

    }




    public void ActiveObject()
    {
        this.gameObject.SetActive(true);
    }

    int CurrentFreme;
    public void DebugFreme()
    {
        Debug.Log($"Freme {CurrentFreme} Work!");
        CurrentFreme++;
    }
}

