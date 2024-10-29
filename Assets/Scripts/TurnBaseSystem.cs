using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Target
{
    Player, Turret, Base
}
public enum Turn
{
    Enemies,
    Player
};


public class TurnBaseSystem : MonoSingleton<TurnBaseSystem>
{


    public DelegateList<EntityTurnBehaviour> enemiesTurnSystems;
    public DelegateList<EntityTurnBehaviour> playerTurnSystems;

    public DelegateList<EntityTurnBehaviour> TurretTurn;

    public Interact PlayerInteractScript;
    public OldPathFinding EnemyPathFindingScript;


    public GameObject EndPharseButton;
    [SerializeField] GameObject BaseHitBox;

    public Turn currentTurn { get; set; }

    bool CanEndPahse = false;

    bool onBattlePhase = false;

    public bool OnBattlePhase
    {
        get { return onBattlePhase; }

        set
        {
            onBattlePhase = value;
            Debug.Log(onBattlePhase);
            if (onBattlePhase == false)
            {
                PreparationPharse.instance.SetToStartTime();
                PlayerActionUI.instance.EnableUI = false;
                TurnBaseSystem.instance.PlayerInteractScript.selectedCharacter = null;
                TurnBaseSystem.instance.PlayerInteractScript.enabled = true;// make player can choosing a tile to moving
            }
        }
    }

    bool actionEnd = true;






    public void UpdateCombatPhase()
    {
        OnBattlePhase = isAttackPhaseEnded();
    }

    bool IsContinueCombatPhase()
    {
        if (!CanEndPahse) return true;

        if (currentTurn == Turn.Enemies)
        {
            foreach (EntityTurnBehaviour i in enemiesTurnSystems.List)
            {
                if (i != null && i.currentState == State.Idle)
                {
                    return true;
                }
            }
        }
        else
        {
            foreach (EntityTurnBehaviour i in playerTurnSystems.List)
            {
                if (i != null && i.currentState == State.Idle)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public Character GetHumenNearestChar(Character Enemy, List<Target> TargetPriority)
    {
        Character Nearest = null;
        float PreviosDistance = 99999999;

        foreach (Target PriorityTarget in TargetPriority)
        {
            if (PriorityTarget == Target.Player)
            {
                foreach (EntityTurnBehaviour i in playerTurnSystems.List)
                {
                    if (i != null)
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
            }
            else if (PriorityTarget == Target.Turret)
            {

                foreach (EntityTurnBehaviour i in playerTurnSystems.List)
                {
                    if (i != null)
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
            }

            if (Nearest != null) break;
        }
        if (TargetPriority[0] == Target.Base || Nearest == null)
        {
            foreach (Transform i in BaseHitBox.transform)
            {
                if (i != null)
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


        }




        return Nearest;
    }

    public void OrderingTurn()
    {
        enemiesTurnSystems.List = enemiesTurnSystems.List.OrderByDescending(i => i.Status.Speed).ToList();
        playerTurnSystems.List = playerTurnSystems.List.OrderByDescending(i => i.Status.Speed).ToList();
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

                OnBattlePhase = isAttackPhaseEnded();

                SwitchingSide();
            }
        }
    }

    public void TurretTurncall()
    {
        TurretTurn.List.RemoveAll(item => item == null);

        foreach (EntityTurnBehaviour i in TurretTurn.List)
        {
            i.onTurn();
        }
    }


    void SwitchingSide()
    {


        if (currentTurn == Turn.Enemies)
        {

            if (TurnNum > enemiesTurnSystems.List.Count - 1)
            {
                EndPharseButton.SetActive(false);
                TurnNum = 0;
                foreach (EntityTurnBehaviour i in playerTurnSystems.List)
                {
                    i.Status.ResetStatus();
                }
                TurretTurncall();
                PlayerInteractScript.selectedCharacter = null;
                PlayerInteractScript.enabled = true;
                CameraBehaviouerControll.instance.LookAtTarget(playerTurnSystems.List[0].transform);
                CameraBehaviouerControll.instance.LookAtTarget(null);

                currentTurn = Turn.Player;
                PlayerInteractScript.enabled = true;

            }
        }
        else
        {
            bool IsRanOutOfPoint = isallPlayerRanOutOfPoint();

            if (IsRanOutOfPoint == true)
            {
                EndPlayerPhase();
            }

        }

    }

    bool isAttackPhaseEnded()
    {
        enemiesTurnSystems.List.RemoveAll(item => item == null);
        playerTurnSystems.List.RemoveAll(item => item == null);
        TurretTurn.List.RemoveAll(item => item == null);

        if (enemiesTurnSystems.List.Count == 0 || enemiesTurnSystems.List.Count == 0) return false;

        return true;
    }

    bool isallPlayerRanOutOfPoint()
    {
        foreach (EntityTurnBehaviour i in playerTurnSystems.List)
        {
            if (i.Status.AvalibleActionPoint > 0 || i.Status.AvalibleMoveStep > 0) return false;
        }

        return true;
    }




    public int TurnNum { get; set; } = 0;

    IEnumerator NextActionDelay(bool value)
    {
        yield return new WaitForSeconds(0.1f);
        actionEnd = value;
    }

    private void ChangeTurn()
    {
        if (currentTurn == Turn.Enemies)
        {
            currentTurn = Turn.Player;
            foreach (SampleTroopTurn i in playerTurnSystems.List)
            {
                i.currentState = State.Idle;
            }
        }
        else
        {
            currentTurn = Turn.Enemies;
            foreach (EnemyTurnBehaviour i in enemiesTurnSystems.List)
            {
                i.currentState = State.Idle;
            }
        }

    }




    private void Update()
    {
        if (OnBattlePhase)
        {
            if (IsContinueCombatPhase() == false)
            {
                ChangeTurn();
            }
            else
            {
                if (ActionEnd && currentTurn == Turn.Enemies)
                {
                    PlayerInteractScript.enabled = false;
                    ActionEnd = false;

                    OnBattlePhase = isAttackPhaseEnded();

                    if (!OnBattlePhase)
                    {
                        return;
                    }

                    enemiesTurnSystems.List[TurnNum].onTurn();
                }
                else if (ActionEnd && currentTurn == Turn.Player)
                {
                    EndPharseButton.SetActive(true);
                }
            }
        }
    }





    public void EndPlayerPhase()
    {
        if (!onBattlePhase)
        {
            PreparationPharse.instance.SetToAttackTime();
        }

        EndPharseButton.SetActive(false);
        PlayerInteractScript.enabled = false;
        TurnNum = 0;
        currentTurn = Turn.Enemies;
    }





}