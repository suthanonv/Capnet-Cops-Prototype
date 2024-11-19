using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinSceneDirector : MonoBehaviour
{
    [SerializeField] int _delay = 5;
    [SerializeField] GameObject _win;
    [SerializeField] GameObject _endOfBeta;
    [SerializeField] GameObject _playAgain;
    [SerializeField] GameObject _mainMenu;
    float _time = 0;
    void Update()
    {
        _time += Time.deltaTime;

        if (_time >= _delay)
        {
            _win.SetActive(true);
            _endOfBeta.SetActive(true);
            if (_time >= _delay + 1)
            {
                _playAgain.SetActive(true);
                _mainMenu.SetActive(true);
            }
        }
    }
}
