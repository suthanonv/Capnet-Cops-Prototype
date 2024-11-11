using UnityEngine;
[ExecuteInEditMode]

public class ChangeOutlineMeshFilter : MonoBehaviour
{

    public GameObject outline;
    private Mesh meh;
    private int count = 0;
    private bool isTerrain = false;

    private void Awake()
    {
        if (this.TryGetComponent<Tile>(out Tile tile))
        {
            if (this.transform.GetChild(0) != null)
            {
                meh = this.transform.GetChild(0).GetComponent<Mesh>();
            }
            else
            {
                meh = this.transform.GetComponent<Mesh>();
            }
            isTerrain = true;
        }
    }

    private void Update()
    {
        if (outline != null && count == 0 && isTerrain == true)
        {
            Instantiate(outline, transform);
            count++;
        }

        if (count > 0)
        {
            if (this.transform.GetChild(1) != null)
            {
                Mesh mesh = this.transform.GetChild(1).GetComponent<Mesh>();
                mesh = meh;
            }
            else
            {
                Mesh mesh = this.transform.GetComponent<Mesh>();
                mesh = meh;
            }
        }
    }

}
