using System.Collections;
using System.Linq;
using UnityEngine;
public class TurnBaseSystem : MonoSingleton<TurnBaseSystem>
{

    enum Turn
    {
        Enemies,
        Player
    };

    public DelegateList<EntityTurnBehaviour> enemiesTurnSystems;
    public DelegateList<EntityTurnBehaviour> playerTurnSystems;

    public Interact PlayerInteractScript;
    public OldPathFinding EnemyPathFindingScript;


    [SerializeField] GameObject EndPharseButton;

    private Turn currentTurn;

    bool CanEndPahse = false;

    bool onBattlePhase = false;

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

    public Character GetHumenNearestChar(Character Enemy)
    {
        Character Nearest = null;
        float PreviosDistance = 99999999;

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
                PlayerInteractScript.selectedCharacter = null;
                PlayerInteractScript.enabled = true;
                CameraBehaviouerControll.instance.LookAtTarget(playerTurnSystems.List[0].transform);
                CameraBehaviouerControll.instance.LookAtTarget(null);

                currentTurn = Turn.Player;
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
                    PlayerInteractScript.enabled = true;
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