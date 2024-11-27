using TMPro;
using UnityEngine;
using UnityEngine.Events;
public class EnemyHealth : Health
{
    [SerializeField] UnityEvent OnDiedEvent;
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
        base.Died();
        this.GetComponent<Character>().characterTile.occupyingCharacter = null;
        this.GetComponent<Character>().characterTile.Occupied = false;

    }

    public override void OnDied()
    {
        OnDiedEvent.Invoke();
        base.OnDied();

        if (TurnBaseSystem.instance.CurrentEnemyTurn == this.GetComponent<EntityTurnBehaviour>())
        {
            TurnBaseSystem.instance.ActionEnd = true;
        }
        if (this.transform.GetChild(1).GetComponent<Animator>().GetBool("Walking"))
        {
            TurnBaseSystem.instance.ActionEnd = true;
        }
        this.GetComponent<Biomass>().OnDie();

    }


    public bool CanbeTarget()
    {
        return true;
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