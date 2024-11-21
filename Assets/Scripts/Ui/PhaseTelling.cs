using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Animations;

public class PhaseTelling : MonoBehaviour
{
    [SerializeField] List<Color> color = new List<Color>();
    string Player = "- Player Turn -";
    string Enemy = "- Enemy Turn -";
    public TextMeshProUGUI text;
    public Animator animate;
    public GameObject PhaseTellingGameObject;

    private void Update()
    {
        Playerturn();
        ENemyturn();
    }

    void Playerturn()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Phase Player && Change this to connet to another script
        {
            PhaseTellingGameObject.gameObject.SetActive(true);
            animate.enabled = false;
            Debug.Log("Player Phase");
            text.text = Player;
            text.color = color[0];
            StartCoroutine(RunAnimation());
        }
    }

    void ENemyturn()
    {
        if (Input.GetKeyDown(KeyCode.W)) // Phase Enemy && Change this to connet to another script
        {
            PhaseTellingGameObject.gameObject.SetActive(true);
            animate.enabled = false;
            Debug.Log("Enemy Phase");
            text.text = Enemy;
            text.color = color[1];
            StartCoroutine(RunAnimation());
            
        }
    }

    IEnumerator bruh()
    {
        yield return new WaitForSeconds(1f);
        PhaseTellingGameObject.gameObject.SetActive(false);
    }
    IEnumerator RunAnimation()
    {
        yield return new WaitForSeconds(2);
        animate.enabled = true;
        animate.Play("PhaseTelling");
        StartCoroutine(bruh());
    }
}
