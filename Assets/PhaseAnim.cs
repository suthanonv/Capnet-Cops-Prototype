using UnityEngine;

public class PhaseAnim : MonoBehaviour
{
    public void OnFinish()
    {
        TurnBaseSystem.instance.PauseBattle = false;
    }
}
