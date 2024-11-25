using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockUI : MonoBehaviour
{
    [SerializeField] List<GameObject> UIToClose = new List<GameObject>();
    [SerializeField] GameObject InCombatPhase;


    public void OHGOWEGHIWG(bool BattlePhase)
    {
        if (BattlePhase == false)
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
