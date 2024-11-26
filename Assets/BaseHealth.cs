using UnityEngine;
using UnityEngine.SceneManagement;
public class BaseHealth : MonoBehaviour
{

    public static BaseHealth Instance;



    [SerializeField] MaterialChange ModelHolder;

    public float MaxHealth = 50;


    private void Awake()
    {
        Instance = this;
    }


    public void TakeDamage(int Damage)
    {
        MaxHealth -= Damage;



        if (MaxHealth <= 0)
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
        SceneManager.LoadScene("Lose Scene");
    }
}
