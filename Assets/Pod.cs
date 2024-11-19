using TMPro;
public class Pod : Health
{
    
    public override void Died()
    {
        Exploring.Instance.OnExploringComplete();
        PodStroingScript.instance.CollecedPod += 1;
        base.Died();
    }
}
