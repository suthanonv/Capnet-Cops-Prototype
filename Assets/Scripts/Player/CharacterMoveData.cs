using UnityEngine;

[CreateAssetMenu(fileName = "MoveData", menuName = "ScriptableObjects/Character Move Data", order = 1)]
public class CharacterMoveData : ScriptableObject
{
    public int MaxMove = 8;
    int baseAttackRange = 0;
    [SerializeField] bool IsEnemy = false;
    public int BaseAttackRange { get { if (!IsEnemy) return baseAttackRange; else { return AttackRange; } } set { baseAttackRange = value; } }

    public int AttackRange = 1;

    public float MoveSpeed = 0.5f;
}
