using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    Tile OccupiledTile;
    [SerializeField]
    LayerMask GroundLayerMask;

    private void Start()
    {
        FindTileAtStart();
    }

    void FindTileAtStart()
    {

        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 50f, GroundLayerMask))
        {
            FinalizePosition(hit.transform.GetComponent<Tile>());
            return;
        }

    }

    void FinalizePosition(Tile tile)
    {
        transform.position = tile.transform.position;
        tile.Occupied = true;
        tile.occupyingCharacter = this.gameObject;
    }

}
