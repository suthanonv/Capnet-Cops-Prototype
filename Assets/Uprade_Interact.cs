using UnityEngine;

public class Uprade_Interact : MonoBehaviour
{

    [SerializeField] GameObject UpgradeUI;
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UpgradeUI.SetActive(true);
            this.enabled = false;
        }
    }
}
