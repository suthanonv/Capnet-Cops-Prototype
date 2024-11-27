using UnityEngine;

public class TurretVisualRange : MonoBehaviour
{
    [SerializeField] GameObject visualRange;
    public void OnMouseOver()
    {
        if (TurnBaseSystem.instance.currentTurn == Turn.Player || TurnBaseSystem.instance.OnBattlePhase == false)
        {
            visualRange.SetActive(true);
        }
    }

    public void OnMouseExit()
    {
        visualRange.SetActive(false);
    }
}
