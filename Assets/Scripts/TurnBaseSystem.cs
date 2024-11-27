using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public enum Target
{
    Player, Turret, Base, Pod, None
}
public enum Turn
{
    Enemies,
    Player
};


public class TurnBaseSystem : MonoSingleton<TurnBaseSystem>
{

    [SerializeField] Transform Base;
    public AudioSource audioSource;
    public AudioClip endturn;
    public DelegateList<EntityTurnBehaviour> enemiesTurnSystems;
    public DelegateList<EntityTurnBehaviour> playerTurnSystems;

    public DelegateList<EntityTurnBehaviour> TurretTurn;

    public Interact PlayerInteractScript;
    public OldPathFinding EnemyPathFindingScript;


    public GameObject EndPharseButton;
    [SerializeField] GameObject BaseHitBox;

    Turn turn;
    public Turn currentTurn
    {
        get { return turn; }
        set
        {
            turn = value;
            if (turn == Turn.Player)
            {
                PlayerInteractScript.selectedCharacter = null;
                PlayerInteractScript.enabled = true;
                CameraBehaviouerControll.instance.LookAtTarget(Base);
                CameraBehaviouerControll.instance.LookAtTarget(null);
                PlayerInteractScript.enabled = true;
            }
        }
    }

    bool CanEndPahse = false;

    bool onBattlePhase = false;

    public UnityEvent<bool> OnPhaseChange = new UnityEvent<bool>();

    bool PreviosBattlePhase = true;
    public bool OnBattlePhase
    {
        get { return onBattlePhase; }

        set
        {

            onBattlePhase = value;
            if (onBattlePhase != PreviosBattlePhase)
            {
                OnPhaseChange.Invoke(onBattlePhase);
            }
            PreviosBattlePhase = value;
            if (onBattlePhase == false && PreparationPharse.instance.CurrentClockTime.SecondSum() >= PreparationPharse.instance.PhaseTransitionTime.SecondSum())
            {
                PreparationPharse.instance.SetToStartTime();
                PlayerActionUI.instance.EnableUI = false;

                if (PlayerInteractScript.selectedCharacter != null)
                {
                    PlayerInteractScript.selectedCharacter.GetComponent<EntityTurnBehaviour>().OffACtion();
                }

                PlayerInteractScript.selectedCharacter = null;
                PlayerInteractScript.enabled = true;// make player can choosing a tile to moving
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
        float PreviosDistance = float.MaxValue;

        // Iterate through target priorities
        foreach (Target PriorityTarget in TargetPriority)
        {
            if (PriorityTarget == Target.Player)
            {
                foreach (EntityTurnBehaviour entity in playerTurnSystems.List)
                {
                    if (entity != null)
                    {
                        float currentDistance = Vector3.Distance(Enemy.transform.position, entity.transform.position);
                        if (currentDistance < PreviosDistance)
                        {
                            Nearest = entity.gameObject.GetComponent<Character>();
                            PreviosDistance = currentDistance;

                        }
                    }
                }


            }
            else if (PriorityTarget == Target.Turret)
            {
                foreach (EntityTurnBehaviour entity in TurretTurn.List)
                {
                    if (entity != null)
                    {
                        float currentDistance = Vector3.Distance(Enemy.transform.position, entity.transform.position);
                        if (currentDistance < PreviosDistance)
                        {
                            Nearest = entity.gameObject.GetComponent<Character>();
                            PreviosDistance = currentDistance;
                        }
                    }
                }
            }


            // Break if we already found a target
            if (Nearest != null) break;
        }

        // Fallback to base if needed
        if (TargetPriority[0] == Target.Base || Nearest == null)
        {
            foreach (Transform baseChild in BaseHitBox.transform)
            {
                if (baseChild != null)
                {
                    float currentDistance = Vector3.Distance(Enemy.transform.position, baseChild.position);
                    if (currentDistance < PreviosDistance)
                    {
                        Nearest = baseChild.GetComponent<Character>();
                        PreviosDistance = currentDistance;
                    }
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
                foreach (EntityTurnBehaviour i in TurretTurn.List)
                {
                    if (i.TryGetComponent<SampleTurretTurn>(out SampleTurretTurn Turret))
                    {
                        Turret.attackingRadius.GetEnemy(null);
                    }

                }

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
            i.GetComponent<SampleTurretTurn>().attackingRadius.AttackAnyEnemy();
        }
        PlayerInteractScript.enabled = true;
        currentTurn = Turn.Player;

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
                    i.ResetState();
                    Debug.Log(i.Status.AvalibleMoveStep);
                }
                TurretTurncall();



                if (onBattlePhase)
                {
                    if (currentTurn == Turn.Enemies)
                    {
                        PauseBattle = true;
                        PhaseTelling.instance.ENemyturn();
                    }
                    else
                    {
                        PauseBattle = true;
                        PhaseTelling.instance.Playerturn();

                    }
                }
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


    public bool PauseBattle = false;

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


    public EntityTurnBehaviour CurrentEnemyTurn;

    private void Update()
    {
        if (OnBattlePhase && PauseBattle == false)
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
                    if (!OnBattlePhase)
                    {
                        return;
                    }


                    if (TurnNum >= enemiesTurnSystems.List.Count) TurnNum = 0;
                    {

                        CurrentEnemyTurn = enemiesTurnSystems.List[TurnNum];
                        enemiesTurnSystems.List[TurnNum].onTurn();

                        foreach (EntityTurnBehaviour i in TurretTurn.List)
                        {
                            if (i.TryGetComponent<SampleTurretTurn>(out SampleTurretTurn Turret))
                            {
                                Turret.attackingRadius.GetEnemy(enemiesTurnSystems.List[TurnNum].gameObject);
                            }

                        }
                    }
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
        //PlayOneShot(endturn);
        ShowMoveingRange.instance.CloseMovingRangeVisual();

        ActionEnd = true;
        PlayerInteractScript.selectedCharacter = null;

        if (!onBattlePhase)
        {
            PreparationPharse.instance.SetToAttackTime();
        }

        EndPharseButton.SetActive(false);
        PlayerInteractScript.enabled = false;
        PlayerActionUI.instance.EnableUI = false;
        TurnNum = 0;
        currentTurn = Turn.Enemies;


        foreach (EntityTurnBehaviour i in playerTurnSystems.List)
        {
            i.ResetState();
        }

        if (onBattlePhase)
        {
            if (currentTurn == Turn.Enemies)
            {
                PauseBattle = true;
                PhaseTelling.instance.ENemyturn();
            }
            else
            {
                PauseBattle = true;
                PhaseTelling.instance.Playerturn();

            }
        }
    }





}