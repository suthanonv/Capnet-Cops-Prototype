[System.Serializable]
public class EntityStat
{
    public int BaseDamage = 50;

    public int Speed = 1;

    public CharacterMoveData moveData;


    int avaliblemovestep;
    public int AvalibleMoveStep
    {
        get { return avaliblemovestep; }
        set
        {
            avaliblemovestep = value;
            UnityEngine.Debug.Log($"Avalible Move {avaliblemovestep}");
            if (avaliblemovestep <= 0)
            {
                avaliblemovestep = 0;
            }
        }
    }

    public int ActionPoint;

    int avalibleactionpoint;
    public int AvalibleActionPoint
    {
        get { return avalibleactionpoint; }
        set
        {
            avalibleactionpoint = value;
            if (avalibleactionpoint < 0)
            {
                avalibleactionpoint = 0;
            }
        }
    }


    public void ResetStatus()
    {
        if (TurnBaseSystem.instance.OnBattlePhase)
        {

            AvalibleActionPoint = ActionPoint;
            AvalibleMoveStep = moveData.MaxMove;
        }
        else
        {
            AvalibleActionPoint = 99;
        }
    }


    public bool IsWalking { get; set; } = false;
}
