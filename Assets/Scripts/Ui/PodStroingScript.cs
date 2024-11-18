using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PodStroingScript : MonoBehaviour
{
    public static PodStroingScript instance;

    void Awake()
    {
        instance = this;
    }

    [SerializeField] TextMeshProUGUI UiTExt;
    int collecedPod;
    public int CollecedPod { 
        get 
        { 
            return collecedPod; 
        } 
        set 
        { 
            collecedPod = value; 
            UiTExt.text = collecedPod.ToString();
            if (collecedPod == 12) 
            {
                SceneManager.LoadScene("Win");
            }
        } 
    }
}
