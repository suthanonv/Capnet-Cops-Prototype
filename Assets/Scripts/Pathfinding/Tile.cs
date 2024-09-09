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
                        SetColor(Color.red);
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
                    SetColor(costMap[terrainCost]);
                }
                else
                {
                    Color newColor = new Color(0, 0.9f, 0.9f); // Default to Cyan Color

                    if (occupyingCharacter != null)
                    {
                        if (occupyingCharacter.TryGetComponent<EntityTeam>(out EntityTeam team))
                        {
                            if (team.EntityTeamSide == Team.Enemy)
                            {
                                newColor = Color.red;
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
        SetColor(costMap[terrainCost]);
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

        SetColor(Color.white);


    }

    public void ClearHighlight()
    {
        OnHighlight = false;
        SetColor(costMap[terrainCost]);


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
        if (terrainCost > costMap.Count - 1)
            terrainCost = 0;

        if (costMap.TryGetValue(terrainCost, out Color col))
        {
            SetColor(col);
        }
        else
        {
            Debug.Log("Invalid terraincost or mapping" + terrainCost);
        }
    }

    private void SetColor(Color color)
    {
        GetComponent<MeshRenderer>().material.color = color;
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
