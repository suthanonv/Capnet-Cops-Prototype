using System.Collections.Generic;
using UnityEngine;

public class CollectingPod : MonoBehaviour
{
    public AudioSource audioSource; 
    public AudioClip impact;        

    private GameObject effect;
    private ParticleSystem smoke;
    private ParticleSystem spark;
    private ParticleSystem explosion;

    private bool hasExploded = false;
    private bool impactSoundPlayed = false;

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

    private void Update()
    {
        
        if (transform.position.y <= 1 && !hasExploded)
        {
            this.GetComponent<Character>().FindTileAtStart();
            spark.Stop();
            smoke.Stop();
            Explosion();
        }
        else if (transform.position.y > 1)
        {
            spark.Play();
            smoke.Play();
            hasExploded = false; 
        }

        
        if (transform.position.y <= 80 && !impactSoundPlayed)
        {
            PlayImpactSound();
        }
    }

    private void Explosion()
    {
        if (!hasExploded)
        {
            explosion.Play();
            hasExploded = true;

            
            StartCoroutine(StopExplosionAfterDuration(explosion.main.duration));
        }
    }

    private System.Collections.IEnumerator StopExplosionAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        explosion.Stop();
    }

    private void PlayImpactSound()
    {
        if (audioSource != null && impact != null)
        {
            audioSource.PlayOneShot(impact);
            impactSoundPlayed = true; 
        }
        else
        {
            Debug.LogWarning("AudioSource or Impact AudioClip not set!");
        }
    }
}
