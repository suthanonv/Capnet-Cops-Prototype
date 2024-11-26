using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PodStroingScript : MonoBehaviour
{
    public static PodStroingScript instance;
    [SerializeField] private EnemySpawnPoint Pod;
    [SerializeField] private TextMeshProUGUI UiTExt;

    private int collecedPod;

    public int CollecedPod
    {
        get => collecedPod;
        set
        {
            collecedPod = value;
            UiTExt.text = $"{CollecedPod}/{Pod.podSpawningRanges.Count}";
            if (collecedPod == Pod.podSpawningRanges.Count)
            {
                SceneManager.LoadScene("Win Scene");
            }
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (Pod == null)
        {
            Pod = GameObject.FindObjectOfType<EnemySpawnPoint>();
            if (Pod == null)
            {
                Debug.LogError("EnemySpawnPoint not found in the scene!");
                return;
            }
        }

        if (UiTExt == null)
        {
            Debug.LogError("UiTExt is not assigned! Assign a TextMeshProUGUI in the Inspector.");
            return;
        }

        UiTExt.text = $"{CollecedPod}/{Pod.podSpawningRanges.Count}";
    }
}
