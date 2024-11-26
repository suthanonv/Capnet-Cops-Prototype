using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightMonster : MonoBehaviour
{
    [SerializeField] MaterialChange MaterialChange;
    public void HighlightingMonster()
    {
        MaterialChange.AddingOutLine();
    }
    public void StopHighlightMonster()
    {
        MaterialChange.RemovingOutLine();
    }
}
