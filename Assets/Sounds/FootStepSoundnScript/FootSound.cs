using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSound : MonoBehaviour
{
 
    public AudioClip footstepClip;
    private AudioSource audioSource;

    [Range(0.8f, 1.2f)] public float pitchMin = 0.8f;
    [Range(0.8f, 1.2f)] public float pitchMax = 1.2f;

    void Start()
    {
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        
        audioSource.clip = footstepClip;
        audioSource.playOnAwake = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Step Ground");
        PlayFootstepSound();
    }

    public void PlayFootstepSound()
    {
        if (footstepClip == null) return;
        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        audioSource.Play();
    }
}
