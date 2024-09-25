using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PathIllustrator))]
public class Pathfinder : MonoBehaviour
{
    #region member fields
    PathIllustrator illustrator;
    [SerializeField]
    LayerMask tileMask;
    #endregion

    private void Start()
    {
        if (illustrator == null)
            illustrator = GetComponent<PathIllustrator>();
    }

    public Path FindPath(Tile origin, Tile destination, EntityStat MoveStat, EntityTeam teamOfEntity, bool IsAttacking)
    {
        List<Tile> openSet = new List<Tile>();
        List<Tile> closedSet = new List<Tile>();
        Dictionary<Tile, float> costFromOrigin = new Dictionary<Tile, float>();

        openSet.Add(origin);
        costFromOrigin[origin] = 0;

        float tileDistance = origin.GetComponent<MeshFilter>().sharedMesh.bounds.extents.z * 2;

        while (openSet.Count > 0)
        {
            openSet.Sort((x, y) => (costFromOrigin[x] + x.costToDestination).CompareTo(costFromOrigin[y] + y.costToDestination));
            Tile currentTile = openSet[0];

            openSet.Remove(currentTile);
            closedSet.Add(currentTile);

            // Check if destination is reached
            if (currentTile == destination)
            {
                List<Tile> path = RetracePath(destination, origin);
                if (path.Count <= MoveStat.AvalibleMoveStep + MoveStat.moveData.AttackRange + 1)
                {
                    return PathBetween(destination, origin, MoveStat);
                }
                return null;
            }

            foreach (Tile neighbor in NeighborTiles(currentTile, teamOfEntity, destination, false, IsAttacking))
            {
                if (closedSet.Contains(neighbor)) continue;

                float costToNeighbor = costFromOrigin[currentTile] + neighbor.terrainCost + tileDistance;
                if (!costFromOrigin.ContainsKey(neighbor) || costToNeighbor < costFromOrigin[neighbor])
                {
                    costFromOrigin[neighbor] = costToNeighbor;
                    neighbor.costToDestination = Vector3.Distance(destination.transform.position, neighbor.transform.position);
                    neighbor.parent = currentTile;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return null;
    }

    private List<Tile> RetracePath(Tile destination, Tile origin)
    {
        List<Tile> path = new List<Tile>();
        Tile current = destination;

        while (current != null)
        {
            path.Add(current);
            if (current == origin) break;
            current = current.parent;
        }

        path.Reverse();
        return path;
    }

    public List<Tile> NeighborTiles(Tile origin, EntityTeam typeOfEntity, Tile destination, bool JustShowRange, bool isAttacking)
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
                if (hitTile != null)
                {
                    if (isAttacking)
                    {
                        if (!hitTile.Occupied || hitTile == destination || JustShowRange)
                        {
                            tiles.Add(hitTile);
                        }
                    }
                    else
                    {
                        if (!hitTile.Occupied)
                        {
                            tiles.Add(hitTile);
                        }
                    }
                }
            }

            Debug.DrawRay(aboveTilePos, Vector3.down * rayLength, Color.blue);
        }

        // Additionally add connected tiles such as ladders
        if (origin.connectedTile != null)
            tiles.Add(origin.connectedTile);

        return tiles;
    }

    public Path PathBetween(Tile dest, Tile source, EntityStat MoveStat)
    {
        Path path = MakePath(dest, source, MoveStat);
        illustrator.IllustratePath(path, MoveStat);
        return path;
    }

    private Path MakePath(Tile destination, Tile origin, EntityStat MoveStat)
    {
        List<Tile> tiles = new List<Tile>();
        Tile current = destination;

        while (current != null)
        {
            tiles.Add(current);
            if (current == origin)
                break;

            current = current.parent;
        }

        tiles.Reverse();

        // Ensure the path length is limited by MaxMove
        tiles = tiles.Take(MoveStat.AvalibleMoveStep + MoveStat.moveData.AttackRange + 1).ToList();

        Path path = new Path();
        path.tiles = tiles.ToArray();

        return path;
    }
}