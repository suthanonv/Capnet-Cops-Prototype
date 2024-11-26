using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialDirector : MonoBehaviour
{
    [SerializeField] GameObject Tutorial;
    [SerializeField] TMP_Text TutorialBody;
    [SerializeField] MaterialChange[] MaterialChanges;
    [SerializeField] GameObject Cameraholder;
    [SerializeField] GameObject arrow;
    [SerializeField] TMP_Text next;
    [SerializeField] GameObject NextButton;
    [SerializeField] PodStroingScript podStorage;
    Pod pod;
    MaterialChange podMat;

    int nextCount;
    float delay;
    int sizeCount;

    void Start()
    {
        ShowPlayer();
        TutorialBody.text = "These are your Units use them wisely";
    }

    void Update()
    {
        if ( nextCount == 0)
        {
            Cameraholder.transform.position = new Vector3(11.7f, 3.54f, 2.139658f);
        }
        if (nextCount == 1)
        {
            StopShowingPlayer();
            ShowShip();
            TutorialBody.text = "This is your ship it's your only ticket out of here, protect it at all cost";
            Cameraholder.transform.position = new Vector3(4.855145f, 3.54f, -14.92151f);
        }
        if (nextCount == 2)
        {
            StopShowingShip();
            TutorialBody.text = "These are the lootpods, Contains the Materials you needs to fix your ship";
            pod = FindAnyObjectByType<Pod>();
            Cameraholder.transform.position = pod.transform.position + new Vector3(-0.3f,3.07f,1.15f);
            podMat = pod.GetComponentInChildren<MaterialChange>();
            podMat.AddingOutLine();
        }
        if (nextCount == 3)
        {
            podMat.RemovingOutLine();
            ShowPlayer();
            TutorialBody.text = "Select a unit and move them to a loot pod to collect them. Action buttons are on the bottom of the screen.";
            Cameraholder.transform.position = new Vector3(11.7f, 3.54f, 2.139658f);
            NextButton.SetActive(false);
            if(podStorage.CollecedPod == 1)
            {
                nextCount++;
            }
        }
        if (nextCount == 4)
        {
            TutorialBody.text = "Press End Phase to Start enemies wave";
            arrow.SetActive(true);
            delay += Time.deltaTime;
            if (delay >= 0.2)
            {
                sizeCount++;
                delay = 0;
            }
            if(sizeCount % 2 ==0)
            {
                arrow.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            }
            else
            {
                arrow.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        if (nextCount == 5)
        {
            arrow.SetActive(false);
            TutorialBody.text = "These are the monster don't let them destroy your ship. End of tutorial. Goodluck";
            next.text = "Start";
            delay += Time.deltaTime;
            if(delay >= 4)
            {
                NextButton.SetActive(true);
            }
        }
        if (nextCount == 6)
        {
            Tutorial.SetActive(false);
        }

    }


    void ShowPlayer()
    {
        MaterialChanges[0].AddingOutLine();
        MaterialChanges[1].AddingOutLine();

    }

    void StopShowingPlayer()
    {
        MaterialChanges[0].RemovingOutLine();
        MaterialChanges[1].RemovingOutLine();

    }
    void ShowShip()
    {
        MaterialChanges[2].AddingOutLine();
    }
    void StopShowingShip()
    {
        MaterialChanges[2].RemovingOutLine();
    }

    public void Next()
    {
        nextCount++;
    }
}
