using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Team
{
    Human , Enemy 
}

public class EntityTeam : MonoBehaviour
{
    public Team EntityTeamSide = Team.Human;
}
