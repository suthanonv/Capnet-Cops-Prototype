using UnityEngine;
using UnityEngine.SceneManagement;
public class KeyManagement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            SceneManager.LoadScene("BetaPresent");
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {

            Application.Quit();
        }
    }
}
