using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhaseTelling : MonoBehaviour
{

    public static PhaseTelling instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] List<Color> color = new List<Color>();
    string Player = "- Player Turn -";
    string Enemy = "- Enemy Turn -";
    public TextMeshProUGUI text;
    public Animator animate;
    public GameObject PhaseTellingGameObject;

    private void Update()
    {

    }

    public void Playerturn()
    {
        animate.SetTrigger("Reset");
        PhaseTellingGameObject.gameObject.SetActive(true);
        Debug.Log("Player Phase");
        text.text = Player;
        text.color = color[0];
        StartCoroutine(RunAnimation());

    }

    public void ENemyturn()
    {
        animate.SetTrigger("Reset");
        PhaseTellingGameObject.gameObject.SetActive(true);
        Debug.Log("Enemy Phase");
        text.text = Enemy;
        text.color = color[1];
        StartCoroutine(RunAnimation());


    }


    IEnumerator RunAnimation()
    {
        yield return new WaitForSeconds(1);
        animate.SetTrigger("Transition");
    }
}
