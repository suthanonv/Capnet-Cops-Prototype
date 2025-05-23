using UnityEngine;
public class AnimationControll : MonoBehaviour
{

    public Health Target { get; set; }

    protected EntityTurnBehaviour EntityTurn;


    private void Start()
    {
        EntityTurn = transform.parent.GetComponent<EntityTurnBehaviour>();
    }


    public virtual void onStartAttacking()
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

        if (Target != null)
        {

            Target.GetComponent<Character>().characterTile.InteractAble = true;
            Target.TakeDamage(EntityTurn.Status.BaseDamage);
        }
        else
        {
            EndAction();
        }



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

    public void OnDied()
    {
        Debug.Log("Died");
        this.transform.parent.GetComponent<Health>().OnDied();
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


    public bool IsPuasingSelf;

    public void PauseSelf()
    {
        IsPuasingSelf = true;
        this.transform.parent.GetComponent<Character>().Paused = true;
    }

    public void UnPauseSelf()
    {
        IsPuasingSelf = false;
        this.transform.parent.GetComponent<Character>().Paused = false;
        if (this.transform.parent.TryGetComponent<EnemyTurnBehaviour>(out EnemyTurnBehaviour enemy))
        {
            enemy.Spawned = true;
        }
    }
}

