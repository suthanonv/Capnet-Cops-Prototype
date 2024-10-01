using UnityEngine;

public class CameraBehaviouerControll : MonoBehaviour
{
    public static CameraBehaviouerControll instance;

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
}
