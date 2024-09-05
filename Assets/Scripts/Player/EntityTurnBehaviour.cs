using UnityEngine;

public class EntityTurnBehaviour : MonoBehaviour
{

    public EntityStat Status;
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



}
