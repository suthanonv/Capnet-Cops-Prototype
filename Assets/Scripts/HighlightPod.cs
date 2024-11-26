using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightPod : MonoBehaviour
{
    [SerializeField] MaterialChange MaterialChange;

    public void HighlightingPod()
    {
        MaterialChange.AddingOutLine();
    }
    public void StopHighlightPod()
    {
        MaterialChange.RemovingOutLine();
    }
}
