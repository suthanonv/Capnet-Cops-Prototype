using System;
using System.Collections;
using UnityEngine;

public class MaterialChange : MonoBehaviour
{


    public MeshRenderer[] ModelPart;



    float HitChageperiod = 0.125f;



    [Header("Material")]
    [SerializeField] Material OnHit;
    [SerializeField] Material OffHit;
    [SerializeField] Material OutLine;

    private void Start()
    {
        NormleMaterial();
    }


    public void OnHitMeterial()
    {

        StopAllCoroutines();
        foreach (MeshRenderer i in ModelPart)
        {

            i.GetComponent<Renderer>().material = OffHit;
        }

        StartCoroutine(ChangeHitingColor());
    }

    void DisableMeterial()
    {

        foreach (MeshRenderer i in ModelPart)
        {

            i.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    void EnableMeterial()
    {
        foreach (MeshRenderer i in ModelPart)
        {

            i.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    public void NormleMaterial()
    {
        foreach (MeshRenderer i in ModelPart)
        {
            i.GetComponent<Renderer>().material = OffHit;
        }
    }


    IEnumerator ChangeHitingColor()
    {


        foreach (MeshRenderer i in ModelPart)
        {


            i.GetComponent<Renderer>().material = OnHit;


        }

        yield return new WaitForSeconds(HitChageperiod);


        NormleMaterial();
    }


    public void AddingOutLine()
    {
        foreach (MeshRenderer renderer in ModelPart)
        {
            if (renderer != null)
            {
                // Get the current materials array
                Material[] materials = renderer.materials;

                // Ensure the array has at least 2 elements to assign the outline material
                if (materials.Length > 1)
                {
                    materials[1] = OutLine; // Assign the outline material
                }
                else
                {
                    // If not, resize the array to add the outline material
                    Array.Resize(ref materials, 2);
                    materials[1] = OutLine;
                }

                // Reassign the materials array back to the renderer
                renderer.materials = materials;
            }
        }
    }

    public void RemovingOutLine()
    {
        foreach (MeshRenderer renderer in ModelPart)
        {
            if (renderer != null)
            {
                // Get the current materials array
                Material[] materials = renderer.materials;

                // Ensure the array has more than 1 element
                if (materials.Length > 1)
                {
                    // Create a new array without the outline material (index 1)
                    Material[] newMaterials = new Material[materials.Length - 1];

                    // Copy elements, skipping the outline material at index 1
                    for (int i = 0, j = 0; i < materials.Length; i++)
                    {
                        if (i != 1) // Skip the outline material
                        {
                            newMaterials[j] = materials[i];  
                            j++;
                        }
                    }

                    // Reassign the updated materials array back to the renderer
                    renderer.materials = newMaterials;
                }
            }
        }
    }

}


