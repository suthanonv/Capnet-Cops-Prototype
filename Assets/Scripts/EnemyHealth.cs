using TMPro;
using UnityEngine;
using UnityEngine.Events;
public class EnemyHealth : Health
{
    [SerializeField] UnityEvent OnDied;
    public TextMeshProUGUI HealthText;
    private ParticleSystem blood;

    public override void Start()
    {
        base.Start();
        MaxBulletQuque = MathScript.instance.Ceiling(Maxhealth);
        HealthText.text = Maxhealth.ToString();
        blood = this.transform.GetChild(4).gameObject.GetComponent<ParticleSystem>();
        blood.Stop();

    }
    public override void TakeDamage(int Damage)
    {
        base.TakeDamage(Damage);
        RemoveBulletQuque();
        MaxBulletQuque = MathScript.instance.Ceiling(Maxhealth);
        HealthText.text = Maxhealth.ToString();
        blood.Play();
    }

    public override void Died()
    {
        OnDied.Invoke();
        if (this.transform.GetChild(1).GetComponent<Animator>().GetBool("Walking"))
        {
            TurnBaseSystem.instance.ActionEnd = true;
        }
        Invoke("newDIed", 0.5f);
        this.GetComponent<Biomass>().OnDie();
    }

    void newDIed()
    {

        base.Died();
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