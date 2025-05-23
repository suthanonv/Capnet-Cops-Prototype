using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    GameObject HighLightTile;

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


    bool interactable = true;
    public bool InteractAble { get { return interactable; } set { interactable = value; } }
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
                        occupyingCharacter.transform.GetChild(1).gameObject.GetComponent<MaterialChange>().AddingOutLine();
                    }
                }
            }
            else if (occupyingCharacter != null && !isinrange)
            {
                if (occupyingCharacter.TryGetComponent<EntityTeam>(out EntityTeam team))
                {
                    if (team.EntityTeamSide == Team.Enemy)
                    {
                        occupyingCharacter.transform.GetChild(1).gameObject.GetComponent<MaterialChange>().RemovingOutLine();
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
            if (EnemyToSpawn != null) showVisual = true;

            if (HighLightTile != null)
            {
                HighLightTile.SetActive(showVisual);

            }
            if (!OnHighlight)
            {
                if (!showVisual)
                {
                    SetColor(baseMaterial);
                }
                else
                {
                    Material newColor = MaterialStorage.Instance.CyanNeon;

                    if (occupyingCharacter != null)
                    {
                        if (occupyingCharacter.TryGetComponent<EntityTeam>(out EntityTeam team))
                        {
                            if (team.EntityTeamSide == Team.Enemy)
                            {
                                newColor = MaterialStorage.Instance.RedNeon;
                                occupyingCharacter.transform.GetChild(1).gameObject.GetComponent<MaterialChange>().AddingOutLine();

                            }
                        }
                    }

                    ChangeHighLightColor(newColor);

                }
            }
        }
    }
    /*  [SerializeField]
      TMP_Text costText; */



    #endregion


    private void Start()
    {
        try
        {
            SetColor(baseMaterial);
            if (this.transform.childCount < 2)
            {
                Destroy(this.gameObject);
            }
            else
            {
                HighLightTile = this.transform.GetChild(1).gameObject;

            }
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex);
            Destroy(this.gameObject);
        }

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
        if (occupyingCharacter == null)
            SetColor(MaterialStorage.Instance.White);
        else
        {
            if (occupyingCharacter.GetComponent<EntityTeam>().EntityTeamSide == Team.Human)
            {
                SetColor(MaterialStorage.Instance.White);
            }
            else
            {
                SetColor(MaterialStorage.Instance.Red);
            }
        }

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

        //HighLightTile.GetComponent<MeshRenderer>().material = color;
    }


    void ChangeHighLightColor(Material Newmaterials)
    {
        if (HighLightTile == null) return;

        MeshRenderer renderer = HighLightTile.GetComponent<MeshRenderer>();
        Material[] materials = renderer.materials;
        materials[1] = Newmaterials;
        renderer.materials = materials;

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



    public void SpawnEnemy()
    {
        GameObject SpawningEnemy = EnemyToSpawn;

        EnemyToSpawn = null;
        ShowRangeVisual = false;

        this.GetComponent<Collider>().enabled = true;

        ChangeHighLightColor(MaterialStorage.Instance.CyanNeon);

        EnemyWaveSpawn.instance.SpawningEnemy.RemoveListener(SpawnEnemy);

        Instantiate(SpawningEnemy, transform.position + new Vector3(0, 0.17f, 0), Quaternion.identity);


    }

    GameObject EnemyToSpawn;
    public void SetSpawnEnemy(GameObject Enemey)
    {


        EnemyToSpawn = Enemey;
        EnemyWaveSpawn.instance.SpawningEnemy.AddListener(SpawnEnemy);
        this.GetComponent<Collider>().enabled = false;
        Occupied = true;

        if (PodCutScene.instance.OnCutScenen == true)
        {
            PodCutScene.instance.OnEnd.AddListener(SetEnemyTileHighlight);
        }
        else
        {
            SetEnemyTileHighlight();
        }
    }


    public void SetEnemyTileHighlight()
    {

        ShowRangeVisual = true;

        ChangeHighLightColor(MaterialStorage.Instance.RedNeon);
    }
}
