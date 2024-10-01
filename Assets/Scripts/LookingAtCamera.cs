using UnityEngine;

public class LookingAtCamera : MonoBehaviour
{

    private Camera mainCamera;

    void Start()
    {
        // Get reference to the main camera
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Maintain the offset position relative to the parent

        // Make the object look at the camera
        if (mainCamera != null)
        {
            transform.LookAt(mainCamera.transform);
            transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
        }
    }
}
