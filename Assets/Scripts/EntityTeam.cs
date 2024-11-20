using UnityEngine;


public enum Team
{
    Human, Enemy, Pod
}

public class EntityTeam : MonoBehaviour
{
    public Team EntityTeamSide = Team.Human;
    public Target TypeOfTarget;
}
