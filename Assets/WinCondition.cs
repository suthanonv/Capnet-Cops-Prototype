using UnityEngine;
using UnityEngine.SceneManagement;
public class WinCondition : MonoBehaviour
{

    [SerializeField] int RequiredBiosMass;
    [SerializeField] int RequiredScrap;
    private void Update()
    {
        if (ResourceManagement.Instance.bioMass >= RequiredBiosMass && ResourceManagement.Instance.scrap >= RequiredScrap)
        {
            SceneManager.LoadScene("Win");
        }
    }
}
