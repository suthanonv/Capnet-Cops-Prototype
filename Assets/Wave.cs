using System.Collections.Generic;

[System.Serializable]
public class Wave
{
    public string WaveName;
    public List<EnemyInWave> AllEnemyInWave = new List<EnemyInWave>();
}
