using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float speed = 8f;
    public float mouseRotateSpeed = 1f;
    public float mouseMoveSpeed = 0.5f; // Speed for moving with the mouse
    Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        UpdateCamera();
    }

    private void UpdateCamera()
    {
        Vector3 input = InputValues(out float yRotation).normalized;
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView + input.y * 2, 30, 110);
        transform.parent.Translate(input * speed * Time.deltaTime); // Translate camera based on mouse movement
        transform.parent.Rotate(Vector3.up * yRotation * Time.deltaTime * speed * 4); // Rotate camera
    }

    private Vector3 InputValues(out float yRotation)
    {
        // Mouse movement and zoom
        Vector3 values = new Vector3();
        values.y = -Input.GetAxis("Mouse ScrollWheel");

        // Moving camera when holding right mouse button
        if (Input.GetMouseButton(2)) // Right mouse button held down
        {
            values.x = -Input.GetAxis("Mouse X") * mouseMoveSpeed; // Use horizontal mouse movement for strafing
            values.z = -Input.GetAxis("Mouse Y") * mouseMoveSpeed; // Use vertical mouse movement for forward/backward movement
        }

        // Rotation with right mouse button
        yRotation = 0;
        if (Input.GetMouseButton(1)) // Rotate only when right mouse button is held down
        {
            yRotation = Input.GetAxis("Mouse X") * mouseRotateSpeed;
        }

        return values;
    }
}
