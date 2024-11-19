using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float speed = 8f;
    public float translateSpeed;
    public float capturedTranslateSpeed;
    public float mouseRotateSpeed = 1f;
    public float mouseMoveSpeed = 0.5f; // Speed for moving with the mouse
    public float mouseZoomSpeed = 2.0f;
    private float mouseCursorSpeedX;
    private float mouseCursorSpeedY;
    private float mouseCursorSpeed;
    public float screenEdgeDetectionArea = 100f;
    private bool isCaptured = false;
    Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        //get mouse speed of X axis
        mouseCursorSpeedX = Input.GetAxis("Mouse X") / Time.deltaTime;
        //get mouse speed of Y axis
        mouseCursorSpeedY = Input.GetAxis("Mouse Y") / Time.deltaTime;
        //find normalize speed of mouse speed of X and Y axis
        mouseCursorSpeed = Mathf.Sqrt(Mathf.Pow(mouseCursorSpeedX, 2) + Mathf.Pow(mouseCursorSpeedY, 2));

        UpdateCamera();
    }

    public void SetCamSize(float Size)
    {
        cam.orthographicSize = Size;
    }

    private void UpdateCamera()
    {
        if (PlayerActionUI.instance.IsMouseOnUI == true)
        {
            return;
        }
        //assign mouse cursor speed to translate speed
        translateSpeed = mouseCursorSpeed;
        //use absolute so the speed always positive
        translateSpeed = Mathf.Abs(translateSpeed);
        Vector3 input = InputValues(out float yRotation).normalized;
        float Zoom = cam.orthographicSize - input.y * mouseZoomSpeed;
        if (Zoom < 0)
        {
            Zoom = 1;
        }
        cam.orthographicSize = Zoom;

        transform.parent.Translate(input * translateSpeed * Time.deltaTime); // Translate camera based on mouse movement
        transform.parent.Rotate(Vector3.up * yRotation * Time.deltaTime * speed * 4); // Rotate camera

        if (Input.mousePosition.x <= 0 + screenEdgeDetectionArea)
        {
            transform.parent.Translate(Vector3.left * speed * Time.deltaTime);
        }

        if (Input.mousePosition.x >= Screen.width - screenEdgeDetectionArea)
        {
            transform.parent.Translate(Vector3.right * speed * Time.deltaTime);
        }

        if (Input.mousePosition.y >= Screen.height - screenEdgeDetectionArea)
        {
            transform.parent.Translate(Vector3.up * speed * Time.deltaTime);
        }

        if (Input.mousePosition.y <= 0 + screenEdgeDetectionArea)
        {
            transform.parent.Translate(Vector3.down * speed * Time.deltaTime);
        }
    }

    private Vector3 InputValues(out float yRotation)
    {
        // Mouse movement and zoom
        Vector3 values = new Vector3();
        values.y = Input.GetAxis("Mouse ScrollWheel");

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