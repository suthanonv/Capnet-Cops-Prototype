using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCameraMovement : MonoBehaviour
{
    private MainMenuCameraPositions _mainMenuCameraPositions;
    private List<GameObject> cams;

    private void Start()
    {
        _mainMenuCameraPositions = GetComponent<MainMenuCameraPositions>();
    }

    public void Cam0()
    {

    }
}
