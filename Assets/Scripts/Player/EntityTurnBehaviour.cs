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

}
