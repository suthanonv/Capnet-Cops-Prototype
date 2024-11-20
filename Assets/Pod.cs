using UnityEngine;

public class Pod : Health
{
    //private bool Used = false;
    public override void Died()
    {
        //if (Used) return;
        Exploring.Instance.OnExploringComplete();
        PodStroingScript.instance.CollecedPod += 1;
        this.GetComponent<EntityTeam>().EntityTeamSide = Team.Pod;
        this.transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("Open");
        //Used = true;
        //Destroy(this.gameObject);
    }
}
