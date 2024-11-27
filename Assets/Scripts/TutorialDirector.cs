using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class TutorialDirector : MonoBehaviour
{
    [SerializeField] GameObject Tutorial;
    [SerializeField] TMP_Text TutorialBody;
    [SerializeField] MaterialChange[] MaterialChanges;
    [SerializeField] GameObject Cameraholder;
    [SerializeField] GameObject EndPhaseArrow;
    [SerializeField] GameObject LeftArrow;
    [SerializeField] GameObject LeftMiddleArrow;
    [SerializeField] GameObject MiddleArrow;
    [SerializeField] TMP_Text next;
    [SerializeField] GameObject NextButton;
    [SerializeField] PodStroingScript podStorage;
    [SerializeField] GameObject Engineer;
    [SerializeField] ResourceManagement ResourceManagement;
    [SerializeField] Button EndPhaseButton;
    [SerializeField] Interact interact;
    [SerializeField] Character EngineerChar;
    Pod pod;
    MaterialChange podMat;


    TurretHealth turret;

    [SerializeField] List<Text_Anim_Detail> Tutorial_Dialouge = new List<Text_Anim_Detail>();
    int count;
    public int nextCount { get { return count; } set { count = value; StopAllCoroutines(); } }
    float delay;
    int sizeCount;

    int DisPlayCount = -1;

    void Update()
    {
        if (nextCount == 0 && DisPlayCount != nextCount)
        {
            EndPhaseButton.interactable = false;

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
            StartCoroutine(TextAnimation(Tutorial_Dialouge[nextCount]));
            LeftArrow.SetActive(false);
            LeftMiddleArrow.SetActive(false);
            MiddleArrow.SetActive(false);
        }
        if (nextCount == 5 && DisPlayCount != nextCount)
        {
            DisPlayCount = nextCount;
            Cameraholder.transform.position = Engineer.transform.position + new Vector3(-0.3f, 3.07f, 1.15f);
            MaterialChanges[1].AddingOutLine();
            StartCoroutine(TextAnimation(Tutorial_Dialouge[nextCount]));
            NextButton.SetActive(false);

        }
        if (nextCount == 6 && DisPlayCount != nextCount)
        {
            DisPlayCount = nextCount;
            Cameraholder.transform.position = Engineer.transform.position + new Vector3(-0.3f, 3.07f, 1.15f);
            StartCoroutine(TextAnimation(Tutorial_Dialouge[nextCount]));
            NextButton.SetActive(false);

        }
        if (nextCount == 7 && DisPlayCount != nextCount)
        {
            EndPhaseButton.interactable = true;
            DisPlayCount = nextCount;
            MaterialChanges[1].RemovingOutLine();
            StartCoroutine(TextAnimation(Tutorial_Dialouge[nextCount]));
            LeftArrow.SetActive(false);
            LeftMiddleArrow.SetActive(false);
            MiddleArrow.SetActive(false);
        }
        if (nextCount == 8 && DisPlayCount != nextCount)
        {
            StopAllCoroutines();
            DisPlayCount = nextCount;
            EndPhaseArrow.SetActive(false);

            StartCoroutine(TextAnimation(Tutorial_Dialouge[nextCount]));

        }
        if (nextCount == 9 && DisPlayCount != nextCount)
        {
            DisPlayCount = nextCount;
            StartCoroutine(TextAnimation(Tutorial_Dialouge[nextCount]));
            NextButton.SetActive(false);

        }
        if (nextCount == 10 && DisPlayCount != nextCount)
        {
            DisPlayCount = nextCount;
            StartCoroutine(TextAnimation(Tutorial_Dialouge[nextCount]));
        }
        if (nextCount == 11 && DisPlayCount != nextCount)
        {
            DisPlayCount = nextCount;
            StartCoroutine(TextAnimation(Tutorial_Dialouge[nextCount]));
            NextButton.SetActive(false);

        }
        if (nextCount == 12 && DisPlayCount != nextCount)
        {
            DisPlayCount = nextCount;
            StartCoroutine(TextAnimation(Tutorial_Dialouge[nextCount]));
            LeftArrow.SetActive(false);
            LeftMiddleArrow.SetActive(false);
            MiddleArrow.SetActive(false);
        }
        if (nextCount == 13 && DisPlayCount != nextCount)
        {
            DisPlayCount = nextCount;
            Tutorial.SetActive(false);
        }

        EndPhaseArrowAnimation();
        UpdatePod();
        updateTurret();
        CheckSelectCharacter();
        CheckSelectEngineer();
        ArrowAnimation();
        Count3ArrowTrigger();
        Count6ArrowTrigger();
        Count11ArrowTrigger();


        if (EndPhaseButton.interactable == false)
        {
            EndPhaseButton.GetComponent<Image>().enabled = false;
            foreach (Transform i in EndPhaseButton.transform)
            {
                i.gameObject.SetActive(false);
            }
        }
        else
        {
            EndPhaseButton.GetComponent<Image>().enabled = true;
            foreach (Transform i in EndPhaseButton.transform)
            {
                i.gameObject.SetActive(true);
            }

        }

    }

    public void SkipTutorial()
    {
        if (nextCount < 13)
        {
            nextCount = 13;
            EndPhaseButton.interactable = true;
        }
    }
    void CheckSelectCharacter()
    {
        if (nextCount == 9)
        {
            if (interact.selectedCharacter != null)
            {
                nextCount++;
            }
        }
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

    void updateTurret()
    {
        if (nextCount == 6)
        {
            turret = FindAnyObjectByType<TurretHealth>();
            if (turret != null)
            {
                nextCount++;
            }
        }
    }

    void CheckSelectEngineer()
    {
        if (nextCount == 5)
        {
            if (interact.selectedCharacter == EngineerChar)
            {
                nextCount++;
            }
        }
    }

    void EndPhaseArrowAnimation()
    {
        if (nextCount != 7) return;
        EndPhaseArrow.SetActive(true);
        delay += Time.deltaTime;
        if (delay >= 0.4)
        {
            sizeCount++;
            delay = 0;
        }
        if (sizeCount % 2 == 0)
        {
            EndPhaseArrow.transform.position += new Vector3(0.05f, 0, 0);
        }
        else
        {
            EndPhaseArrow.transform.position -= new Vector3(0.05f, 0, 0);
        }
    }


    void ArrowAnimation()
    {
        if (nextCount != 3 && nextCount != 6 && nextCount != 11) return;
        delay += Time.deltaTime;
        if (delay >= 0.4)
        {
            sizeCount++;
            delay = 0;
        }
        if (sizeCount % 2 == 0)
        {
            LeftArrow.transform.position += new Vector3(0, 0.05f, 0);
            LeftMiddleArrow.transform.position += new Vector3(0, 0.05f, 0);
            MiddleArrow.transform.position += new Vector3(0, 0.05f, 0);
        }
        else
        {
            LeftArrow.transform.position -= new Vector3(0, 0.05f, 0);
            LeftMiddleArrow.transform.position -= new Vector3(0, 0.05f, 0);
            MiddleArrow.transform.position -= new Vector3(0, 0.05f, 0);
        }
    }

    void Count3ArrowTrigger()
    {
        if (nextCount != 3) return;
        if (interact.selectedCharacter == EngineerChar)
        {
            LeftArrow.SetActive(true);
            LeftMiddleArrow.SetActive(false);
            MiddleArrow.SetActive(false);
        }
        else if (interact.selectedCharacter == null)
        {
            LeftArrow.SetActive(false);
            LeftMiddleArrow.SetActive(false);
            MiddleArrow.SetActive(false);
        }
        else 
        {
            LeftMiddleArrow.SetActive(true);
            LeftArrow.SetActive(false);
            MiddleArrow.SetActive(false);  
        }
    }

    void Count6ArrowTrigger()
    {
        if(nextCount != 6) return;
        if (interact.selectedCharacter == EngineerChar)
        {
            MiddleArrow.SetActive(true);
            LeftArrow.SetActive(false);
            LeftMiddleArrow.SetActive(false);
        }
        else
        {
            LeftArrow.SetActive(false);
            LeftMiddleArrow.SetActive(false);
            MiddleArrow.SetActive(false);
        }
    }

    void Count11ArrowTrigger()
    {
        if (nextCount != 11) return;
        if (interact.selectedCharacter != null)
        {
            LeftArrow.SetActive(false);
            LeftMiddleArrow.SetActive(false);
            MiddleArrow.SetActive(true);
        }
        else
        {
            LeftArrow.SetActive(false);
            LeftMiddleArrow.SetActive(false);
            MiddleArrow.SetActive(false);
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
        if (nextCount == 13)
            StartCoroutine(CloseUI());
        else if (nextCount == 8)
        {
            yield return new WaitForSeconds(15);
            NextButton.SetActive(true);
        }
        else if (nextCount != 3 && nextCount != 5 && nextCount != 6 && nextCount != 7 && nextCount != 9)
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

