using UnityEngine;

public class TurretDamage : MonoBehaviour
{
    public static TurretDamage Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public int Damage;


}

