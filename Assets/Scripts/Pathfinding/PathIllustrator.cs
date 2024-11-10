using System.Collections.Generic;
using UnityEngine;

public class PathIllustrator : MonoBehaviour
{
    [SerializeField] private float LineHeightOffset = 0.33f;
    private LineRenderer line;
    private Path previousPath;

    public static PathIllustrator instance;

    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        line = GetComponent<LineRenderer>();


    }

    public void IllustratePath(Path path, EntityStat moveData)
    {
        line.positionCount = path.tiles.Length;

        // Clear highlights if the path is different from the previous path
        if (previousPath != null && !previousPath.IsSamePath(path))
        {
            foreach (Tile tile in previousPath.tiles)
            {
                tile.ClearHighlight();
            }
        }

        // Update the previous path to the current one
        previousPath = path;

        // Highlight the tiles and set the line positions
        int loopCount = path.tiles.Length;

        if (path.tiles.Length > moveData.AvalibleMoveStep)
        {
            if (path.tiles[path.tiles.Length - 1].occupyingCharacter == null)
            {
                loopCount = moveData.AvalibleMoveStep + 1;
            }
            else if (path.tiles[path.tiles.Length - 1].occupyingCharacter.GetComponent<EntityTeam>().EntityTeamSide == Team.Human)
            {
                loopCount = moveData.AvalibleMoveStep + 1;
            }
        }

        // Highlight starting and ending tiles
        path.tiles[0].Highlight();
        path.tiles[loopCount - 1].Highlight();

        // Temporary list to store positions excluding (0, 0, 0)
        List<Vector3> validPositions = new List<Vector3>();

        for (int i = 0; i < loopCount; i++)
        {
            Transform tileTransform = path.tiles[i].transform;
            Vector3 position = tileTransform.position + new Vector3(0, LineHeightOffset, 0);

            // Add to list if position is not (0, 0, 0)
            if (position != Vector3.zero)
            {
                validPositions.Add(position);
            }
        }

        // Update the LineRenderer with filtered positions
        line.positionCount = validPositions.Count;
        line.SetPositions(validPositions.ToArray());
    }


    public void ClearPaht()
    {
        if (previousPath != null)
        {
            foreach (Tile tile in previousPath.tiles)
            {
                tile.ClearHighlight();
            }

        }
        line.positionCount = 0;


    }


}
