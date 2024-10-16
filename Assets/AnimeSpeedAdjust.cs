using System.Collections.Generic;
using UnityEngine;

public class AnimeSpeedAdjust : MonoBehaviour
{

    public static AnimeSpeedAdjust Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] List<Case> SpeedCase;

    public float GetAnimSpeed(EntityTeam TeamSide)
    {
        if (TeamSide.EntityTeamSide == Team.Enemy)
        {
            int Index = TurnBaseSystem.instance.enemiesTurnSystems.List.Count - 1;

            if (TurnBaseSystem.instance.enemiesTurnSystems.List.Count >= SpeedCase.Count) Index = SpeedCase.Count - 1;
            else if (Index < 0) return 0.5f;

            return SpeedCase[Index].Speed;

        }
        else
        {
            return 0.5f;
        }
    }
}

[System.Serializable]
public class Case
{
    public string Name;
    public float Speed;
}