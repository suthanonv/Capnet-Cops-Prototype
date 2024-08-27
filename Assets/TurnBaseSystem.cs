using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using JetBrains.Annotations;
public class TurnBaseSystem : MonoSingleton<TurnBaseSystem>
{



    public DelegateList<EntityTurnBehaviour> turnSystems;

    public Interact PlayerMovingScript;
    public OldPathFinding EnemyPathFindingScript;

    




    [SerializeField] bool StopBattlePhase = false;


    bool actionEnd = true;

    
    
    
    public Character GetHumenNearestChar(Character Enemy)
    {
        Character Nearest = null;
        float PreviosDistance = 99999999;



        foreach(EntityTurnBehaviour i in turnSystems.List)
        {
            if(i.GetComponent<EntityTeam>().EntityTeamSide == Team.Human)
            {
                if(Vector3.Distance(Enemy.transform.position , i.transform.position) < PreviosDistance)
                {
                    Nearest = i.gameObject.GetComponent<Character>();
                    PreviosDistance = Vector3.Distance(Enemy.transform.position, i.transform.position);
                }
            }
            else
            {
                continue;
            }
        }
        return Nearest;
    }
    
    
    
    public bool ActionEnd
    {
        get { return actionEnd; }
        set 
        {
            actionEnd = value;    

            if(actionEnd)
            {
                TurnNum++;
                TurnNum %= turnSystems.List.Count;
            }
        }
    }

    int TurnNum = 0;






    public void OrderingTurn()
    {
        turnSystems.List.OrderByDescending(i => i.Status.Speed);
      
    }


   

    private void Update()
    {
        if(!StopBattlePhase)
        {
            if (ActionEnd)
            {
                ActionEnd = false;
                turnSystems.List[TurnNum].onTurn();
            }
        }
    }

  

    
}
