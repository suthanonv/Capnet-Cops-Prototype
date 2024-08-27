using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OldPathFinding : MonoBehaviour
{
    #region member fields
    [SerializeField]
    LayerMask tileMask;
    #endregion

    /// <summary>
    /// Main pathfinding function, marks tiles as being in frontier, while keeping a copy of the frontier
    /// in "currentFrontier" for later clearing
    /// </summary>
    /// <param name="character"></param>
    public Path FindPath(Tile origin, Tile destination, CharacterMoveData moveData)
    {
        List<Tile> openSet = new List<Tile>();
        List<Tile> closedSet = new List<Tile>();

        openSet.Add(origin);
        origin.costFromOrigin = 0;

        float tileDistance = origin.GetComponent<MeshFilter>().sharedMesh.bounds.extents.z * 2;

        while (openSet.Count > 0)
        {
            openSet.Sort((x, y) => x.TotalCost.CompareTo(y.TotalCost));
            Tile currentTile = openSet[0];

            openSet.Remove(currentTile);
            if (currentTile != destination) // Only add to closedSet if not the destination
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

    /// <summary>
    /// Returns a list of all neighboring hexagonal tiles and ladders,
    /// including the destination tile even if it is occupied.
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    private List<Tile> NeighborTiles(Tile origin, Tile destination)
    {
        const float HEXAGONAL_OFFSET = 1.75f;
        List<Tile> tiles = new List<Tile>();
        Vector3 direction = Vector3.forward * (origin.GetComponent<MeshFilter>().sharedMesh.bounds.extents.x * HEXAGONAL_OFFSET);
        float rayLength = 4f;
        float rayHeightOffset = 1f;

        // Rotate a raycast in 60 degree steps and find all adjacent tiles
        for (int i = 0; i < 6; i++)
        {
            direction = Quaternion.Euler(0f, 60f, 0f) * direction;

            Vector3 aboveTilePos = (origin.transform.position + direction).With(y: origin.transform.position.y + rayHeightOffset);

            if (Physics.Raycast(aboveTilePos, Vector3.down, out RaycastHit hit, rayLength, tileMask))
            {
                Tile hitTile = hit.transform.GetComponent<Tile>();
                if (!tiles.Contains(hitTile) && (hitTile.Occupied == false || hitTile == destination))
                    tiles.Add(hitTile);
            }

            Debug.DrawRay(aboveTilePos, Vector3.down * rayLength, Color.blue);
        }

        // Additionally add connected tiles such as ladders
        if (origin.connectedTile != null)
            tiles.Add(origin.connectedTile);

        return tiles;
    }

    /// <summary>
    /// Called by Interact.cs to create a path between two tiles on the grid 
    /// </summary>
    /// <param name="dest"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public Path PathBetween(Tile dest, Tile source, CharacterMoveData moveData)
    {
        Path path = MakePath(dest, source, moveData);
        return path;
    }

    /// <summary>
    /// Creates a path between two tiles
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="origin"></param>
    /// <returns></returns>
    private Path MakePath(Tile destination, Tile origin, CharacterMoveData moveData)
    {
        List<Tile> tiles = new List<Tile>();
        Tile current = destination;

        while (current != origin)
        {
            tiles.Add(current);
            if (current.parent != null)
                current = current.parent;
            else
                break;
        }

        tiles.Add(origin);
        tiles.Reverse();

        tiles = tiles.Take(moveData.MaxMove).ToList();

        Path path = new Path();
        path.tiles = tiles.ToArray();

        return path;
    }
}
