using System.Collections.Generic;
using UnityEngine;

public class AssignOutLine : MonoBehaviour
{
    public GameObject OutLine;
    [SerializeField] Transform Map;
    List<GameObject> tiles = new List<GameObject>();
    private void OnValidate()
    {
        if (Application.isPlaying == false && OutLine != null)
        {
            foreach (Transform i in Map)
            {
                if (i.TryGetComponent<Tile>(out Tile tile))
                {
                    tiles.Add(tile.gameObject);
                }
            }

            AssigningOutLine(tiles);
        }
    }

    void AssigningOutLine(List<GameObject> Tile)
    {
        foreach (GameObject i in Tile)
        {
            Instantiate(OutLine, i.transform);
        }

        Destroy(this.gameObject);
    }
}
