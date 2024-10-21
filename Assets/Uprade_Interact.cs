using UnityEngine;

public class Uprade_Interact : MonoBehaviour
{

    [SerializeField] GameObject UpgradeUI;
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && TurnBaseSystem.instance.PlayerInteractScript.selectedCharacter == null)
        {
            UpgradeUI.SetActive(true);
            this.enabled = false;
        }
    }
}
