using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuSceneController : MonoBehaviour
{
    public Animator animator;
    public bool fadeInFinished = false;

    //used for exiting game
    private float timer;
    [SerializeField] private float holdDuration = 3f;

    //for executing the "cutscene"
    MainMenuStartGame mainMenuStart;

    //when "cutscene" finishes
    private float switchSceneTimer;
    [SerializeField] private float switchSceneDuration = 1.5f;

    //setting scene where the game takes place (this is where you'll update if more stages are added)

    void Start()
    {
        mainMenuStart = GetComponent<MainMenuStartGame>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        //if player presses 'Escape' for 'holdDuration' it will ExitGame()
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            timer = Time.time;
        }
        else if (Input.GetKey(KeyCode.Escape))
        {
            if (Time.time - timer > holdDuration)
            {
                timer = float.PositiveInfinity;
                //I copied this code from: https://discussions.unity.com/t/solved-hold-button-for-3-seconds/652471/3 since it seems to work pretty well
                Debug.Log("Exit game FadeOut");
                SceneFadeOut();
            }
        }
        else
        {
            timer = float.PositiveInfinity;
        }

        //if player presses any key it will initiate the start of the game
        if(mainMenuStart.startFinished == true)//only starts when all animations in MainMenuStartGame have finished running
        {
            Debug.Log("startFinished == true, now cutting to black");
            SceneCutToBlack();
            Debug.Log("switchSceneTimer has commenced.");
            switchSceneTimer = Time.deltaTime;
            //waits for switchSceneDuration to get to zero before switching scenes
            //if (Time.deltaTime - switchSceneTimer >= switchSceneDuration)
            //{
            //    Debug.Log("switchSceneTimer has ended: beginning load scene");
                //adapted code from exit game.
                timer = float.PositiveInfinity;
                SceneManager.GetSceneByName("BetaPresent");
                SceneManager.LoadScene("BetaPresent");
                Debug.Log("Loaded scene");
            //}
            //else
            //{
            //    timer = float.PositiveInfinity;
            //}
        }

    }

    public void SceneCutToBlack()
    {
        animator.SetTrigger("CutToBlack");
    }

    public void SceneFadeOut()
    {
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeInComplete()
    {
        fadeInFinished = true;
        Debug.Log("Fade in finished is TRUE");
    }

    public void OnFadeComplete()
    {
        if(mainMenuStart.start == false)
        {
            Application.Quit();
            Debug.Log("Exited Game");
        }
        else
        {

        }
    }
}
