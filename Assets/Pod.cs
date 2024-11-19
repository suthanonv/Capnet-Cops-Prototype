using System.Diagnostics;
using TMPro;
public class Pod : Health
{
    //private bool Used = false;
    public override void Died()
    {
        //if (Used) return;
        Exploring.Instance.OnExploringComplete();
        PodStroingScript.instance.CollecedPod += 1;
        base.Died();
        //Used = true;
        //Destroy(this.gameObject);
    }
}
