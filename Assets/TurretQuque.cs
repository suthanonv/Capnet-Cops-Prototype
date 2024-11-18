using System.Collections.Generic;
using UnityEngine;

public class TurretQuque : MonoBehaviour
{
    public static TurretQuque instance;

    private void Awake()
    {
        instance = this;
    }

    List<EntityTurnBehaviour> entityTurnBehaviours = new List<EntityTurnBehaviour>();


    EntityTurnBehaviour CurrentnTurret = null;

    bool actionEnd = true;
    public bool ActionEnd
    {
        get
        {
            return actionEnd;
        }
        set
        {
            if (value == true)
            {
                if (CurrentnTurret != null)
                    entityTurnBehaviours.Remove(CurrentnTurret);
            }
            actionEnd = value;
        }
    }

    private void Update()
    {
        if (entityTurnBehaviours.Count > 0 && ActionEnd)
        {
            ActionEnd = false;
            CurrentnTurret = entityTurnBehaviours[0];
            CurrentnTurret.onTurn();
        }
    }




    public void AddingQuque(EntityTurnBehaviour Turret)
    {
        entityTurnBehaviours.Add(Turret);
    }

}
