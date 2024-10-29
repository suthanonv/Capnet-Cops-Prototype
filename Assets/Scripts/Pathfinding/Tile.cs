using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{



    #region member fields
    public Tile parent;
    public Tile connectedTile;
    public GameObject occupyingCharacter;

    public float costFromOrigin = 0;
    public float costToDestination = 0;
    public int terrainCost = 0;
    public float TotalCost { get { return costFromOrigin + costToDestination + terrainCost; } }
    public bool Occupied { get; set; } = false;


    int tileHight;
    public int TileHight { get { return tileHight; } set { tileHight = value; } }

    bool isinrange = false;


    Material baseMaterial;



    private void Awake()
    {
        foreach (Transform i in this.transform)
        {
            if (i.TryGetComponent<MeshRenderer>(out MeshRenderer mesh))
            {
                baseMaterial = mesh.material;
            }
        }

        if (baseMaterial == null)
        {
            baseMaterial = this.GetComponent<MeshRenderer>().material;
        }
    }

    public bool IsInAttackRange
    {
        get
        {
            return isinrange;
        }
        set
        {
            isinrange = value;

            if (isinrange && occupyingCharacter != null)
            {
                if (occupyingCharacter.TryGetComponent<EntityTeam>(out EntityTeam team))
                {
                    if (team.EntityTeamSide == Team.Enemy)
                    {
                        SetColor(MaterialStorage.Instance.Red);
                    }
                }
            }
        }

    }

    bool showVisual = false;

    public bool ShowRangeVisual
    {
        get
        {
            return showVisual;
        }
        set
        {
            showVisual = value;

            if (!OnHighlight)
            {
                if (!showVisual)
                {
                    SetColor(baseMaterial);
                }
                else
                {
                    Material newColor = MaterialStorage.Instance.Cyan;

                    if (occupyingCharacter != null)
                    {
                        if (occupyingCharacter.TryGetComponent<EntityTeam>(out EntityTeam team))
                        {
                            if (team.EntityTeamSide == Team.Enemy)
                            {
                                newColor = MaterialStorage.Instance.Red;
                            }
                        }
                    }
                    SetColor(newColor);
                }
            }
        }
    }
    /*  [SerializeField]
      TMP_Text costText; */



    #endregion


    private void Start()
    {
        SetColor(baseMaterial);
    }

    Dictionary<int, Color> costMap = new Dictionary<int, Color>()
    {
        {0, new Color(0.1f, 0.6f, 0.1f) }, //Gray
        {1, new Color(0.1f, 0.6f, 0.1f) },//Red
        {2, new Color(0.1f, 0.6f, 0.1f) } //Dark red
    };

    /// <summary>
    /// Changes color of the tile by activating child-objects of different colors
    /// </summary>
    /// <param name="col"></param>
    /// 
    bool OnHighlight = false;
    public void Highlight()
    {

        SetColor(MaterialStorage.Instance.White);


    }

    public void ClearHighlight()
    {
        OnHighlight = false;
        SetColor(baseMaterial);


        IsInAttackRange = isinrange;

        ShowRangeVisual = showVisual;
    }

    /// <summary>
    /// This is called when right clicking a tile to increase its cost
    /// </summary>
    /// <param name="value"></param>
    public void ModifyCost()
    {
        terrainCost++;

        return;
        if (terrainCost > costMap.Count - 1)
            terrainCost = 0;

        if (costMap.TryGetValue(terrainCost, out Color col))
        {
        }
        else
        {
            Debug.Log("Invalid terraincost or mapping" + terrainCost);
        }
    }

    private void SetColor(Material color)
    {
        GetComponent<MeshRenderer>().material = color;
        if (this.transform.childCount > 1)
            foreach (Transform i in this.transform)
            {
                if (i.TryGetComponent<MeshRenderer>(out MeshRenderer mesh))
                {
                    mesh.material = color;
                }
            }

    }

    /*
    public void DebugCostText()
    {
        costText.text = TotalCost.ToString("F1");
    }

    public void ClearText()
    {
        costText.text = "";
    } */
}
