using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourcesData
{
    public int scraps;
    public int biomass;
    public int humanResource;
    
    public ResourcesData(ResourceManagement resource)
    {
        scraps = resource.scrap;
        biomass = resource.bioMass;
        humanResource = resource.humanResource;
    }
}

