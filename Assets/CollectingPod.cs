using UnityEngine;

public class CollectingPod : MonoBehaviour
{

    private GameObject effect;
    private ParticleSystem smoke;
    private ParticleSystem spark;
    private ParticleSystem explosion;
    private int n = 0;

    private void Start()
    {
        effect = this.transform.GetChild(2).gameObject;
        if (effect != null)
        {
            spark = effect.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
            smoke = effect.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
            explosion = effect.transform.GetChild(2).gameObject.GetComponent<ParticleSystem>();
            explosion.Stop();
        }
    }

    private void OnMouseOver()
    {

    }

    private void Update()
    {

        if (transform.position.y <= 1)
        {
            this.GetComponent<Character>().FindTileAtStart();
            spark.Stop();
            smoke.Stop();
            Explosion();
        }
        else if (transform.position.y >= 1)
        {
            spark.Play();
            smoke.Play();
            n = 0;
        }
    }

    private void Explosion()
    {
        if (n <= 0)
        {
            explosion.Play();
            n = 1;
        }
        else
        {
            explosion.Stop();
        }
    }
}
