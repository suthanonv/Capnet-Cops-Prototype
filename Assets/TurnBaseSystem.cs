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




    bool onBattlePhase = true;

    public bool OnBattlePhase
    {
        get { return onBattlePhase; }

        set
        {
            onBattlePhase = value;
        }
    }



    bool actionEnd = true;

    
    
    bool IsContinueCombatPhase()
    {
        foreach(EntityTurnBehaviour i in turnSystems.List)
        {
            if(i.gameObject.GetComponent<EntityTeam>().EntityTeamSide == Team.Enemy)
            {
                return true;
            }
        }
        return false;
    }


    
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


    public void OrderingTurn()
    {
        turnSystems.List.OrderByDescending(i => i.Status.Speed);

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

                OnBattlePhase = IsContinueCombatPhase();
            }
        }
    }

    int TurnNum = 0;


    private void Update()
    {
        if(OnBattlePhase)
        {
           
            if (ActionEnd)
            {
                ActionEnd = false;

                OnBattlePhase = IsContinueCombatPhase();

                if (!OnBattlePhase)
                {
                    return;
                }
                turnSystems.List[TurnNum].onTurn();
            }
        }
    }

  

    
}
