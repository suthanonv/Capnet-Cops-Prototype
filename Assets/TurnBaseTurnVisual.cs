using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnBaseTurnVisual : MonoBehaviour
{

    public static TurnBaseTurnVisual Instance;

    [SerializeField] Transform CombatTurnImageHolder;

    [SerializeField] int ImageLimitInVisual = 6;

    [SerializeField] GameObject UIPrefab;

    private void Awake()
    {
        Instance = this;
    }



    public void SetCombatTurnVisual()
    {

        StopAllCoroutines();

        StartCoroutine(WaitFinishAddComponentInList(TurnBaseSystem.instance.turnSystems.List));

    }


    IEnumerator WaitFinishAddComponentInList(List<EntityTurnBehaviour> AllEntity)
    {
        yield return new WaitForSeconds(0.01f);


        for (int LoopCount = 0; LoopCount < ImageLimitInVisual; LoopCount++)
        {
            for (int Pattern = 0; Pattern < AllEntity.Count; Pattern++)
            {
                GameObject UI = Instantiate(UIPrefab, CombatTurnImageHolder);

                if (AllEntity[Pattern].GetComponent<EntityTeam>().EntityTeamSide == Team.Human)
                {
                    UI.GetComponent<Image>().color = Color.blue;
                }
                else
                {
                    UI.GetComponent<Image>().color = Color.red;
                }

                UI.GetComponent<TurnVisualComponent>().ObjectParent = AllEntity[Pattern];
            }
        }
    }

    public void UpdateTurnVisual()
    {
        CombatTurnImageHolder.GetChild(0).SetAsLastSibling();
    }


    public void RemoveImageFromTurnVisual(EntityTurnBehaviour EntityToRemove)
    {


        foreach (Transform i in CombatTurnImageHolder)
        {
            if (i.GetComponent<TurnVisualComponent>().ObjectParent == EntityToRemove)
            {
                Destroy(i.gameObject);
            }

        }
    }

}

