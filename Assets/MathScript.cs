using UnityEngine;

public class MathScript : MonoBehaviour
{
    public static MathScript instance;

    private void Awake()
    {
        instance = this;
    }
    public int Ceiling(int CurrentHealth)
    {
        if (CurrentHealth < TurretDamage.Instance.Damage)
        {
            return 1;
        }
        else
        {
            return Mathf.CeilToInt(CurrentHealth / TurretDamage.Instance.Damage);
        }
    }
}
