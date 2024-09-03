using System.Linq;
using UnityEngine;
public class TurnBaseSystem : MonoSingleton<TurnBaseSystem>
{



    public DelegateList<EntityTurnBehaviour> turnSystems;

    public Interact PlayerInteractScript;
    public OldPathFinding EnemyPathFindingScript;


    bool CanEndPahse = false;

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
        if (!CanEndPahse) return true;

        foreach (EntityTurnBehaviour i in turnSystems.List)
        {
            if (i.gameObject.GetComponent<EntityTeam>().EntityTeamSide == Team.Enemy)
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



        foreach (EntityTurnBehaviour i in turnSystems.List)
        {
            if (i.GetComponent<EntityTeam>().EntityTeamSide == Team.Human)
            {
                if (Vector3.Distance(Enemy.transform.position, i.transform.position) < PreviosDistance)
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
        turnSystems.List = turnSystems.List.OrderByDescending(i => i.Status.Speed).ToList();
    }




    public bool ActionEnd
    {
        get { return actionEnd; }
        set
        {
            actionEnd = value;

            if (actionEnd)
            {
                TurnNum++;



                if (TurnNum > turnSystems.List.Count - 1)
                {
                    TurnNum = 0;
                    OrderingTurn();
                }



            }
        }
    }

    int TurnNum = 0;


    private void Update()
    {
        if (OnBattlePhase)
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
