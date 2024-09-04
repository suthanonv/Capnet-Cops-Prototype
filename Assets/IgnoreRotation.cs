using UnityEngine;

public class IgnoreRotation : MonoBehaviour
{
    [SerializeField] Vector3 Offset = new Vector3();

    Vector3 fixedRotation = new Vector3(0, 0, 0); // Example: No rotation

    void Update()
    {
        this.transform.position = this.transform.parent.position + Offset;
        transform.rotation = Quaternion.Euler(fixedRotation);
    }
}
