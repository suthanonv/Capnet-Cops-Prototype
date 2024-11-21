using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Animations;

public class PhaseTelling : MonoBehaviour
{
    [SerializeField] List<Color> color = new List<Color>();
    string Player = "Player Turn";
    string Enemy = "Enemy Turn";
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
        if (Input.GetKeyDown(KeyCode.Space)) // Phase Player
        {
            PhaseTellingGameObject.gameObject.SetActive(true);
            text.text = Player;
            text.color = color[0];
            animate.Play("PhaseTelling");
        }

        if(animate.enabled == false)
        {
            PhaseTellingGameObject.gameObject.SetActive(false);
        }
    }

    void ENemyturn()
    {
        if (Input.GetKeyDown(KeyCode.W)) // Phase Enemy
        {
            PhaseTellingGameObject.gameObject.SetActive(true);
            text.text = Enemy;
            text.color = color[1];
            animate.Play("PhaseTelling");
        }

        if (animate.enabled == false)
        {
            PhaseTellingGameObject.gameObject.SetActive(false);
        }
    }
}
