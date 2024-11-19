[System.Serializable]
public class Path
{
    public Tile[] tiles;

    public int PathLength = 0;

    public bool IsSamePath(Path PathCheck)
    {
        // Check if the PathCheck object is null
        if (PathCheck == null) return false;

        // Check if tiles in either path are null
        if (tiles == null || PathCheck.tiles == null) return false;

        // Check if the number of tiles is different
        if (tiles.Length != PathCheck.tiles.Length)
        {
            return false;
        }

        // Check each tile in the path
        for (int i = 0; i < tiles.Length; i++)
        {
            // If any tile is different, the paths are not the same
            if (tiles[i] != PathCheck.tiles[i])
            {
                return false;
            }
        }

        // All tiles are the same, so the paths are identical
        return true;
    }
}
