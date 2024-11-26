using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ClockUI : MonoBehaviour
{
    [SerializeField] List<GameObject> UIToClose = new List<GameObject>();
    [SerializeField] GameObject InCombatPhase;

    private void FixedUpdate()
    {
        OHGOWEGHIWG(TurnBaseSystem.instance.OnBattlePhase);
        Debug.Log($"{(TurnBaseSystem.instance.OnBattlePhase)}");
    }

    public void OHGOWEGHIWG(bool BattlePhase)
    {
        if (BattlePhase)
        {
            foreach (GameObject i in UIToClose)
            {
                i.SetActive(false);
            }
            InCombatPhase.SetActive(true);
            Debug.Log("äÍ â§è");
        }
        else
        {
            foreach (GameObject i in UIToClose)
            {
                i.SetActive(true);
            }
            InCombatPhase.SetActive(false);
            Debug.Log("äÍ ¤ÇÒÂ");
        }
    }
}
