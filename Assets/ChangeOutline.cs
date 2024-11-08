using UnityEngine;
[ExecuteInEditMode]
public class ChangeOutline : MonoBehaviour
{
    MeshFilter filter;
    private void Awake()
    {
        if (this.transform.parent.TryGetComponent<Tile>(out Tile tiel) == false)
        {
            Destroy(this.gameObject);
        }

        filter = this.GetComponent<MeshFilter>();
    }

    private void Start()
    {
        foreach (Transform i in this.transform.parent)
        {
            if (i != this.transform)
            {
                filter.mesh = i.transform.GetComponent<MeshFilter>().mesh;
                return;
            }
        }
    }


}
