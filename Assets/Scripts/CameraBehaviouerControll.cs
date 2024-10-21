using UnityEngine;

public class CameraBehaviouerControll : MonoBehaviour
{
    public static CameraBehaviouerControll instance;
    public Transform origin;

    private void Start()
    {
        origin = GameObject.Find("CameraOrigin").GetComponent<Transform>();
    }

    private void Awake()
    {
        instance = this;
    }
    public void LookAtTarget(Transform target)
    {
        // Check if the target is not null
        if (target != null)
        {
            // Make the camera look at the target's position
            transform.LookAt(target.position);
            this.GetComponent<CameraControl>().enabled = false;

        }
        else
        {
            this.GetComponent<CameraControl>().enabled = true;
        }
    }

    public void ResetTransform()
    {
        Debug.Log("reset transform");
        this.transform.localRotation = Quaternion.Euler(55, 0, 0);
        
        // transform.parent.position = new Vector3(origin.transform.position.x, origin.transform.position.y,
        //     origin.transform.position.z);
        
        transform.parent.localRotation = Quaternion.Euler(origin.transform.rotation.x, origin.transform.rotation.y,
            origin.transform.rotation.z);
    }
}
