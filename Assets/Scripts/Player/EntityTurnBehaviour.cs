using UnityEngine;



public enum State
{
    Idle,
    Wait
};

public class EntityTurnBehaviour : MonoBehaviour
{

    public EntityStat Status;
    public State currentState;

    Animator animator;

    protected virtual void Start()
    {
        animator = this.gameObject.transform.GetChild(1).GetComponent<Animator>();
    }

    public virtual void DeSelect()
    {
        animator.SetTrigger("deselect");
    }

    public virtual void Select()
    {
        animator.SetTrigger("Select");

    }
    public virtual bool InterActacle()
    {
        return false;
    }

    public virtual void onTurn()
    {
        Status.ResetStatus();
    }


    public virtual void DoingAction(int TypeOfAction)
    {

    }

    public virtual void OnActionEnd()
    {

    }

    public virtual void WalkingButton()
    {

    }

    public virtual void AttackingButton()
    {

    }

    public virtual void Onwalking()
    {

    }

    public virtual void OffACtion()
    {

    }

}
