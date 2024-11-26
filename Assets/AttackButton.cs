using UnityEngine;
using UnityEngine.UI;
public class AttackButton : MonoBehaviour
{
    [SerializeField] Sprite _Building;
    [SerializeField] Sprite _Attack;
    [SerializeField] Image MainIcon;
    [SerializeField] GameObject UI_Text;

    private void Update()
    {
        if (TurnBaseSystem.instance.OnBattlePhase)
        {
            UI_Text.SetActive(false);
            MainIcon.sprite = _Attack;
        }
        else
        {
            UI_Text.SetActive(true);
            MainIcon.sprite = _Building;
        }
    }



}
