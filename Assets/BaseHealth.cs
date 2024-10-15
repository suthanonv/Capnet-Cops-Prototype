using UnityEngine;
using UnityEngine.SceneManagement;
public class BaseHealth : MonoBehaviour
{

    public static BaseHealth Instance;



    [SerializeField] MaterialChange ModelHolder;

    [SerializeField] float MaxHealth;
    float CurrentHealth;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }
    public void TakeDamage(int Damage)
    {
        CurrentHealth -= Damage;



        if (CurrentHealth <= 0)
        {
            Died();
        }
        else
        {
            ModelHolder.OnHitMeterial();
        }
    }

    public void Died()
    {
        SceneManager.LoadScene("Lose");
    }
}
