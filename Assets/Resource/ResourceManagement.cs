using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using TMPro;

public class ResourceManagement : MonoBehaviour
{
    [SerializeField] public int bioMass;
    [SerializeField] public int scrap;
    [SerializeField] public int humanResource;

    [SerializeField] public TextMeshProUGUI text1;
    [SerializeField] public TextMeshProUGUI text2;
    [SerializeField] public TextMeshProUGUI text3;

    private void Start()
    {
        OnResourceChange();
    }
    
    public void IncreaseResource(int num, int type)
    {
        switch (type)
        {
            case 0 :
                bioMass += num; break;
            case 1 :
                scrap += num; break;
            case 2 : 
                humanResource += num; break;
        }
        OnResourceChange();
    }
    
    public void DecreaseResource(int num, int type)
    {
        switch (type)
        {
            case 0 :
                bioMass -= num; break;
            case 1 :
                scrap -= num; break;
            case 2 : 
                humanResource -= num; break;
        }
        OnResourceChange();
    }

    public void OnResourceChange()
    {
        text1.SetText(bioMass.ToString());
        text2.SetText(scrap.ToString());
        text3.SetText(humanResource.ToString());
    }

    public void SaveResource()
    {
        SaveSystem.SaveResource(this);
    }

    public void LoadResource()
    {
        ResourcesData data = SaveSystem.LoadResource();

        scrap = data.scraps;
        bioMass = data.biomass;
        humanResource = data.humanResource;
        
        OnResourceChange();
    }

    public void ClearSave()
    {
        scrap = 0;
        bioMass = 0;
        humanResource = 0;
        
        SaveResource();
    }

}
