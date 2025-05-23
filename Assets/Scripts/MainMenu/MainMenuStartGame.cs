using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuStartGame : MonoBehaviour
{
    MainMenuSceneController mainMenuSceneController;
    Animator animator;
    public bool start = false;
    private bool textFadeOutFinished = false;
    public bool startFinished = false;

    void Start()
    {
        mainMenuSceneController = GetComponent<MainMenuSceneController>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        if (mainMenuSceneController.fadeInFinished && Input.GetKeyDown(KeyCode.Space))
        {
            //only allow start game after fade in has finished
            start = true;
            Debug.Log("Game is starting.");
            animator.SetTrigger("FadeOutText");
            SceneManager.LoadScene(1);
        }
        if (textFadeOutFinished)
        {
            Debug.Log("TextFadeOut finished");
            //cutting it short for now to test that it works. Later this will be the actual asteroid cutscene.
            startFinished = true;
        }
    }

    private void OnFadeOutComplete()
    {
        textFadeOutFinished = true;
    }
}
