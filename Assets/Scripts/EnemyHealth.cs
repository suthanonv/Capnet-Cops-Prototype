using TMPro;
using UnityEngine;
using UnityEngine.Events;
public class EnemyHealth : Health
{
    [SerializeField] UnityEvent OnDied;
    public TextMeshProUGUI HealthText;

    public override void Start()
    {
        base.Start();
        MaxBulletQuque = MathScript.instance.Ceiling(Maxhealth);
        HealthText.text = Maxhealth.ToString();


    }
    public override void TakeDamage(int Damage)
    {
        base.TakeDamage(Damage);
        RemoveBulletQuque();
        MaxBulletQuque = MathScript.instance.Ceiling(Maxhealth);
        HealthText.text = Maxhealth.ToString();

    }

    public override void Died()
    {
        OnDied.Invoke();
        if (this.transform.GetChild(1).GetComponent<Animator>().GetBool("Walking"))
        {
            TurnBaseSystem.instance.ActionEnd = true;
        }
        base.Died();
        this.GetComponent<Biomass>().OnDie();
    }

    public bool CanbeTarget()
    {
        return CurrentBulletQuque < MaxBulletQuque;


    }

    int MaxBulletQuque;
    int CurrentBulletQuque;

    public void SetBulletQuque()
    {
        CurrentBulletQuque++;
    }

    public void RemoveBulletQuque()
    {
        CurrentBulletQuque--;
        if (CurrentBulletQuque <= 0)
        {
            CurrentBulletQuque = 0;
        }
    }
}