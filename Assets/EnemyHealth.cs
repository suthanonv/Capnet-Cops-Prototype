using TMPro;

public class EnemyHealth : Health
{
    public TextMeshProUGUI HealthText;

    public override void Start()
    {
        base.Start();
        HealthText.text = Maxhealth.ToString();
    }
    public override void TakeDamage(int Damage)
    {
        base.TakeDamage(Damage);
        HealthText.text = Maxhealth.ToString();

    }
}
