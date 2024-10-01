[System.Serializable]
public class EntityStat
{
    public int Speed = 1;

    public CharacterMoveData moveData;


    int avaliblemovestep;
    public int AvalibleMoveStep
    {
        get { return avaliblemovestep; }
        set
        {
            avaliblemovestep = value;

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
        AvalibleActionPoint = ActionPoint;
        if (TurnBaseSystem.instance.OnBattlePhase)
        {


            AvalibleMoveStep = moveData.MaxMove;
        }
        else
        {
            AvalibleMoveStep = 99;

        }
    }
}
