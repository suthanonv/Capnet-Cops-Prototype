using UnityEngine;

public class CameraBehaviouerControll : MonoBehaviour
{
    public static CameraBehaviouerControll instance;
    public Transform origin;

    public float speed = 2f;
    private float elapsedTime = 0f;
    public bool isMoving = false;
    private GameObject targetTransform;

    private void Start()
    {
        origin = GameObject.Find("CameraOrigin").GetComponent<Transform>();
    }

    private void Awake()
    {
        instance = this;
    }
    public bool GetTarGet()
    {
        return (TargetCheck != null);
    }


    Transform TargetCheck;
    public void LookAtTarget(Transform target)
    {
        TargetCheck = target;
        // Check if the target is not null
        if (target != null)
        {
            targetTransform = target.GetChild(0).gameObject;
            isMoving = true;
            elapsedTime = 0f;
            this.GetComponent<CameraControl>().enabled = false;

        }
        else
        {
            this.GetComponent<CameraControl>().enabled = true;
        }
    }

    private void Moving()
    {
        if (targetTransform == null) return;
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / speed;

        transform.parent.transform.position =
            Vector3.Lerp(this.transform.parent.transform.position, targetTransform.transform.position, t);

        float startYRotation = transform.parent.transform.eulerAngles.y;
        float endYRotation = targetTransform.transform.eulerAngles.y;
        float yRotation = Mathf.LerpAngle(startYRotation, endYRotation, t);

        transform.parent.transform.rotation = Quaternion.Euler(transform.parent.transform.rotation.eulerAngles.x,
            yRotation, transform.parent.transform.rotation.eulerAngles.z);

        if (t >= 1f)
        {
            isMoving = false;
            elapsedTime = 0f;
        }
    }

    private void Update()
    {
        if (isMoving) Moving();
    }

    public void ResetTransform()
    {
        // Debug.Log("reset transform");
        // this.transform.localRotation = Quaternion.Euler(55, 0, 0);
        //
        // // transform.parent.position = new Vector3(origin.transform.position.x, origin.transform.position.y,
        // //     origin.transform.position.z);
        //
        // transform.parent.localRotation = Quaternion.Euler(origin.transform.rotation.x, origin.transform.rotation.y,
        //     origin.transform.rotation.z);
    }
}