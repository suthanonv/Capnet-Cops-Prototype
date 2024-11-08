using UnityEngine;

public class CollectingPod : MonoBehaviour
{
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Exploring.Instance.OnExploringStart();
            Destroy(this.gameObject);
        }
    }
}
