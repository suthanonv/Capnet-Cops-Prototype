using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterProfile : MonoBehaviour
{
    [SerializeField] GameObject Character;
    [SerializeField] Slider Slide;

    private void Start()
    {
        Slide.maxValue = Character.GetComponent<Health>().Maxhealth;
    }

    void Update()
    {
        Slide.value = Character.GetComponent<Health>().Maxhealth;
    }
}
