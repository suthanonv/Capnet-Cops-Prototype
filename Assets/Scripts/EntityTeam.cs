using UnityEngine;


public enum Team
{
    Human, Enemy
}

public class EntityTeam : MonoBehaviour
{
    public Team EntityTeamSide = Team.Human;
    public Target TypeOfTarget;
}
