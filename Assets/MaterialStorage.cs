using UnityEngine;

public class MaterialStorage : MonoBehaviour
{
    public static MaterialStorage Instance;
    private void Awake() { Instance = this; }

    public Material White;
    public Material Red;
    public Material Cyan;
    public Material RedNeon;
    public Material CyanNeon;

}
