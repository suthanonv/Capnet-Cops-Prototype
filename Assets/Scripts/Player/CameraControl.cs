using System.Collections;
using UnityEngine;
public class CameraControl : MonoBehaviour
{
    public static CameraControl instance;


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
    [SerializeField] private GameObject piviotPoint;
    [SerializeField] private bool isRotateAround = false;

    [SerializeField] float MaxZoom = 12.5f;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    public void SetPiviotPoint(GameObject character)
    {
        /*isRotateAround = true;
        piviotPoint = character;*/
    }

    public void UndoPiviotPoint()
    {
        /*isRotateAround = false;
        piviotPoint = null;*/
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

    public void SetCamSize(float targetSize, float duration)
    {
        StartCoroutine(LerpToNewSize(targetSize, duration));
    }

    private IEnumerator LerpToNewSize(float targetSize, float duration)
    {
        float startSize = cam.orthographicSize;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            cam.orthographicSize = Mathf.Lerp(startSize, targetSize, elapsedTime / duration);
            yield return null;
        }

        // Ensure the final size is exactly the target size
        cam.orthographicSize = targetSize;
    }
    public void SetCamPosition(Vector3 newCamPos, float duration)
    {
        StartCoroutine(LerpToNewPosition(newCamPos, duration));
    }

    private IEnumerator LerpToNewPosition(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = transform.parent.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            transform.parent.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            yield return null;
        }

        // Ensure the final position is exactly the target position
        transform.parent.position = targetPosition;
    }

    private void UpdateCamera()
    {
        if (PlayerActionUI.instance.IsMouseOnUI == true || PodCutScene.instance.OnCutScenen)
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
        cam.orthographicSize = Mathf.Clamp(Zoom, 1, MaxZoom);

        transform.parent.Translate(input * translateSpeed * Time.deltaTime); // Translate camera based on mouse movement

        if (isRotateAround && piviotPoint != null)
        {
            transform.parent.RotateAround(piviotPoint.transform.position, Vector3.up, yRotation * Time.deltaTime * speed * 4);
        }
        else if (!isRotateAround && piviotPoint == null)
        {
            transform.parent.Rotate(Vector3.up * yRotation * Time.deltaTime * speed * 4); // Rotate camera
        }

        if (Input.mousePosition.x <= 0 + screenEdgeDetectionArea)
        {
            transform.parent.Translate(Vector3.left * speed / 2 * Time.deltaTime);
        }

        if (Input.mousePosition.x >= Screen.width - screenEdgeDetectionArea)
        {
            transform.parent.Translate(Vector3.right * speed / 2 * Time.deltaTime);
        }

        if (Input.mousePosition.y >= Screen.height - screenEdgeDetectionArea)
        {
            transform.parent.Translate(Vector3.forward * speed / 2 * Time.deltaTime);
        }

        if (Input.mousePosition.y <= 0 + screenEdgeDetectionArea)
        {
            transform.parent.Translate(-Vector3.forward * speed / 2 * Time.deltaTime);
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