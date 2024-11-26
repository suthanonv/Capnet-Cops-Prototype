using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void LoadingScene(int i)
    {
        SceneManager.LoadScene(i);
    }
}
