using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OldPathFinding : MonoBehaviour
{
    #region member fields
    [SerializeField]
    LayerMask tileMask;
    #endregion

    public Path FindPath(Tile origin, Tile destination, CharacterMoveData moveData)
    {
        List<Tile> openSet = new List<Tile>();
        HashSet<Tile> closedSet = new HashSet<Tile>();

        openSet.Add(origin);
        origin.costFromOrigin = 0;

        float tileDistance = origin.GetComponent<MeshFilter>().sharedMesh.bounds.extents.z * 2;

        while (openSet.Count > 0)
        {
            openSet.Sort((x, y) => x.TotalCost.CompareTo(y.TotalCost));
            Tile currentTile = openSet[0];

            openSet.Remove(currentTile);
            closedSet.Add(currentTile);

            // Destination reached
            if (currentTile == destination)
            {
                return PathBetween(destination, origin, moveData);
            }

            foreach (Tile neighbor in NeighborTiles(currentTile, destination))
            {
                if (closedSet.Contains(neighbor))
                    continue;

                float costToNeighbor = currentTile.costFromOrigin + neighbor.terrainCost + tileDistance;
                if (costToNeighbor < neighbor.costFromOrigin || !openSet.Contains(neighbor))
                {
                    neighbor.costFromOrigin = costToNeighbor;
                    neighbor.costToDestination = Vector3.Distance(destination.transform.position, neighbor.transform.position);
                    neighbor.parent = currentTile;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return null;
    }

    private List<Tile> NeighborTiles(Tile origin, Tile destination)
    {
        const float HEXAGONAL_OFFSET = 1.75f;
        List<Tile> tiles = new List<Tile>();
        Vector3 direction = Vector3.forward * (origin.GetComponent<MeshFilter>().sharedMesh.bounds.extents.x * HEXAGONAL_OFFSET);
        float rayLength = 4f;
        float rayHeightOffset = 1f;

        for (int i = 0; i < 6; i++)
        {
            direction = Quaternion.Euler(0f, 60f, 0f) * direction;

            Vector3 aboveTilePos = (origin.transform.position + direction).With(y: origin.transform.position.y + rayHeightOffset);

            if (Physics.Raycast(aboveTilePos, Vector3.down, out RaycastHit hit, rayLength, tileMask))
            {
                Tile hitTile = hit.transform.GetComponent<Tile>();
                if (hitTile != null && !tiles.Contains(hitTile) && (hitTile.Occupied == false || hitTile == destination))
                {
                    tiles.Add(hitTile);
                }
            }

            Debug.DrawRay(aboveTilePos, Vector3.down * rayLength, Color.blue);
        }

        if (origin.connectedTile != null)
            tiles.Add(origin.connectedTile);

        return tiles;
    }

    public Path PathBetween(Tile dest, Tile source, CharacterMoveData moveData)
    {
        Path path = MakePath(dest, source, moveData);
        return path;
    }

    private Path MakePath(Tile destination, Tile origin, CharacterMoveData moveData)
    {
        List<Tile> tiles = new List<Tile>();
        Tile current = destination;

        while (current != origin)
        {
            tiles.Add(current);
            current = current.parent;

            if (current == null)
                break; // Safety check in case of missing parents
        }

        tiles.Add(origin);
        tiles.Reverse();

        // Ensure the path doesn't exceed the movement range
        List<Tile> moveRange = tiles.Take(moveData.MaxMove).ToList();
        List<Tile> attackRange = tiles.Skip(moveData.MaxMove).Take(moveData.AttackRange + 1).ToList();

        tiles = moveRange.Concat(attackRange).ToList();

        if (tiles.Count > 0)
            Debug.Log(tiles[tiles.Count - 1].occupyingCharacter == null);

        Path path = new Path();
        path.tiles = tiles.ToArray();



        return path;
    }
}
