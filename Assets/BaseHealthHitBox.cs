public class BaseHealthHitBox : Health
{
    public override void Start()
    {
        // Do Nothing
    }
    public override void TakeDamage(int Damage)
    {
        this.transform.GetComponentInParent<BaseHealth>().TakeDamage(Damage);
    }

    public override void Died()
    {
        // Do Nothing cuz Cant Died 
    }
}
