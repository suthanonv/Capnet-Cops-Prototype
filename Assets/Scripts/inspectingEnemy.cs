using UnityEngine;

public class inspectingEnemy : MonoBehaviour
{


    private void OnMouseOver()
    {
        if (TurnBaseSystem.instance.PlayerInteractScript.selectedCharacter != null)
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
        }

    }

    private void OnMouseExit()
    {
        if (TurnBaseSystem.instance.PlayerInteractScript.selectedCharacter != null)
        {
            this.transform.GetChild(0).gameObject.SetActive(false);

        }

    }
}
