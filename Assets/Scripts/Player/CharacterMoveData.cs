using UnityEngine;

[CreateAssetMenu(fileName = "MoveData", menuName = "ScriptableObjects/Character Move Data", order = 1)]
public class CharacterMoveData : ScriptableObject
{
    public int MaxMove = 8;
    [SerializeField] int baseAttackRange = 1;
    [SerializeField] bool IsEnemy = false;
    public int BaseAttackRange
    {

        get
        {
            if (!IsEnemy)
            {
                if (TurnBaseSystem.instance.OnBattlePhase == false)
                    return 1;
                else
                {
                    if (TargetObj == Target.None)
                    {
                        Debug.Log($"None {AttackRange}");

                        return AttackRange;
                    }
                    else
                    {
                        if (TargetObj == Target.Pod)
                        {
                            Debug.Log($"Pod {1}");
                            return 1;
                        }
                        else
                        {
                            Debug.Log($"Player {AttackRange}");

                            return AttackRange;
                        }
                    }
                }

            }
            else
            {
                return AttackRange;
            }

        }



        set { baseAttackRange = value; }
    }


    Target tarobj;
    public Target TargetObj { get { return tarobj; } set { tarobj = value; Debug.Log(tarobj); } }

    public int AttackRange = 1;

    public float MoveSpeed = 0.5f;
}
