using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoseScreenDirector : MonoBehaviour
{
    [SerializeField] int _delay = 5;
    [SerializeField] GameObject _lose;
    [SerializeField] GameObject _tryAgain;
    [SerializeField] GameObject _mainMenu;
    float _time = 0;
    void Update()
    {
        _time += Time.deltaTime;

        if (_time >= _delay)
        {
            _lose.SetActive(true);
            if (_time >= _delay + 1)
            {
                _tryAgain.SetActive(true);
                _mainMenu.SetActive(true);
            }
        }
    }
}


