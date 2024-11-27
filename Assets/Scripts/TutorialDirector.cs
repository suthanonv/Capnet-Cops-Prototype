using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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


    [SerializeField] List<Text_Anim_Detail> Tutorial_Dialouge = new List<Text_Anim_Detail>();
    public int nextCount { get; set; }
    float delay;
    int sizeCount;

    int DisPlayCount = -1;

    void Update()
    {
        if (nextCount == 0 && DisPlayCount != nextCount)
        {
            NextButton.SetActive(false);

            DisPlayCount = nextCount;
            Cameraholder.transform.position = new Vector3(11.7f, 3.54f, 2.139658f);
            ShowPlayer();
            StartCoroutine(TextAnimation(Tutorial_Dialouge[nextCount]));

        }
        if (nextCount == 1 && DisPlayCount != nextCount)
        {
            NextButton.SetActive(false);

            DisPlayCount = nextCount;
            StopShowingPlayer();
            ShowShip();
            StartCoroutine(TextAnimation(Tutorial_Dialouge[nextCount]));

            Cameraholder.transform.position = new Vector3(4.855145f, 3.54f, -14.92151f);
        }
        if (nextCount == 2 && DisPlayCount != nextCount)
        {
            NextButton.SetActive(false);

            DisPlayCount = nextCount;
            StopShowingShip();
            StartCoroutine(TextAnimation(Tutorial_Dialouge[nextCount]));

            pod = FindAnyObjectByType<Pod>();
            Cameraholder.transform.position = pod.transform.position + new Vector3(-0.3f, 3.07f, 1.15f);
            podMat = pod.GetComponentInChildren<MaterialChange>();
            podMat.AddingOutLine();
        }
        if (nextCount == 3 && DisPlayCount != nextCount)
        {
            NextButton.SetActive(false);

            DisPlayCount = nextCount;
            podMat.RemovingOutLine();
            ShowPlayer();
            StartCoroutine(TextAnimation(Tutorial_Dialouge[nextCount]));

            Cameraholder.transform.position = new Vector3(11.7f, 3.54f, 2.139658f);

        }
        if (nextCount == 4 && DisPlayCount != nextCount)
        {
            DisPlayCount = nextCount;
            NextButton.SetActive(false);
            StartCoroutine(TextAnimation(Tutorial_Dialouge[nextCount]));

        }
        if (nextCount == 5 && DisPlayCount != nextCount)
        {
            StopAllCoroutines();
            DisPlayCount = nextCount;
            arrow.SetActive(false);
            NextButton.SetActive(false);


            StartCoroutine(TextAnimation(Tutorial_Dialouge[nextCount]));




        }
        if (nextCount == 6 && DisPlayCount != nextCount)
        {
            DisPlayCount = nextCount;
            Tutorial.SetActive(false);
        }

        ArrowAnimation();
        UpdatePod();
    }

    void UpdatePod()
    {
        if (nextCount == 3)
        {
            if (podStorage.CollecedPod == 1)
            {
                nextCount++;
            }
        }
    }

    void ArrowAnimation()
    {
        if (nextCount != 4) return;
        arrow.SetActive(true);
        delay += Time.deltaTime;
        if (delay >= 0.2)
        {
            sizeCount++;
            delay = 0;
        }
        if (sizeCount % 2 == 0)
        {
            arrow.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }
        else
        {
            arrow.transform.localScale = new Vector3(1, 1, 1);
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
        TutorialBody.text = "";
    }



    IEnumerator TextAnimation(Text_Anim_Detail textDetail)
    {
        int count = 0;
        TutorialBody.text = "";

        while (TutorialBody.text.Length < textDetail.Text.Length)
        {
            TutorialBody.text += textDetail.Text[count];
            yield return new WaitForSeconds(textDetail.TextPlaySpeed);
            count++;
        }
        if (nextCount == 5)
            StartCoroutine(CloseUI());
        else if (nextCount != 3 && nextCount != 4)
        {
            NextButton.SetActive(true);
        }

    }

    [SerializeField] float CloseTime = 4;
    IEnumerator CloseUI()
    {
        yield return new WaitForSeconds(CloseTime);
        nextCount++;
    }

}


[System.Serializable]
public class Text_Anim_Detail
{
    public string Text;
    public float TextPlaySpeed;
    public bool IsPLayed { get; set; }
}

