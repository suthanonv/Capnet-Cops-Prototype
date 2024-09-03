using UnityEngine;

public class PathIllustrator : MonoBehaviour
{
    private const float LineHeightOffset = 0.33f;
    private LineRenderer line;
    private Path previousPath;



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




        for (int i = 0; i < loopCount; i++)
        {
            path.tiles[i].Highlight();

            // Set the position for the LineRenderer
            //  Transform tileTransform = path.tiles[i].transform;
            // line.SetPosition(i, tileTransform.position.With(y: tileTransform.position.y + LineHeightOffset));
        }
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


    }


}
