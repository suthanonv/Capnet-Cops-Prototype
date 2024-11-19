using TMPro;
using UnityEngine;
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
    public int CollecedPod
    {
        get
        {
            return collecedPod;
        }
        set
        {
            collecedPod = value;
            UiTExt.text = $"{CollecedPod.ToString()}/12";
            if (collecedPod == 12)
            {
                SceneManager.LoadScene("Win");
            }
        }
    }

    private void Start()
    {
        UiTExt.text = $"{CollecedPod.ToString()}/12";
    }
}
