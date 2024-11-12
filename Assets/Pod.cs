public class Pod : Health
{
    public override void Died()
    {
        Exploring.Instance.OnExploringComplete();
        base.Died();
    }
}
