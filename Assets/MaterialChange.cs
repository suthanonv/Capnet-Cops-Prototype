using System.Collections;
using UnityEngine;

public class MaterialChange : MonoBehaviour
{


    public MeshRenderer[] ModelPart;



    [Header("Hiting Effect")]
    [SerializeField] float HitChageperiod;



    [Header("Material")]
    [SerializeField] Material OnHit;
    [SerializeField] Material OffHit;


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

    void NormleMaterial()
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
}


