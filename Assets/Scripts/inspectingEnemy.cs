using UnityEngine;

public class inspectingEnemy : MonoBehaviour
{
    GameObject FollowObject;

    private void Start()
    {
        FollowObject = this.transform.parent.gameObject;
        this.transform.parent = null;
    }


    private void Update()
    {

        this.transform.position = FollowObject.transform.position;
        this.transform.rotation = FollowObject.transform.rotation;
    }




    private void OnMouseOver()
    {
        Debug.Log("Mouse Over");
        if (TurnBaseSystem.instance.currentTurn == Turn.Player)
        {

            this.transform.GetChild(0).gameObject.SetActive(true);
        }

    }

    private void OnMouseExit()
    {

        this.transform.GetChild(0).gameObject.SetActive(false);



    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
