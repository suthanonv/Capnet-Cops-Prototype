using UnityEngine;


[ExecuteInEditMode]

public class MaterialAssign : MonoBehaviour
{

    MaterialChange SetMeterial;

    void Awake()
    {
        SetMeterial = GetComponent<MaterialChange>();
    }

    private void Start()
    {
        MeshRenderer[] AllChild = GetComponentsInChildren<MeshRenderer>();

        SetMeterial.ModelPart = AllChild;

        SetMeterial.NormleMaterial();
    }

}
