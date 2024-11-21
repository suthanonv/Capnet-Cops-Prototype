using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSoundController : MonoBehaviour
{
    
    public AudioSource audioSource; 
    public List<AudioClip> bgTracks; 

    private List<AudioClip> unusedTracks; 
    private AudioClip currentTrack;
    private TurnBaseSystem turnBaseSystem;

    void Start()
    {

        if (bgTracks == null || bgTracks.Count == 0)
        {
            Debug.LogError("No background tracks assigned");
            return;
        }

        turnBaseSystem = GetComponent<TurnBaseSystem>();
        unusedTracks = new List<AudioClip>(bgTracks);

        PlayRandomTrack();
    }

    void Update()
    {
       
        if (!audioSource.isPlaying && !audioSource.loop)
        {
            PlayNextTrack();
        }
    }

    private void PlayRandomTrack()
    {
        int randomIndex = Random.Range(0, unusedTracks.Count);
        currentTrack = unusedTracks[randomIndex];
        unusedTracks.RemoveAt(randomIndex);

        PlayTrack(currentTrack);
    }

    private void PlayNextTrack()
    {

        if (unusedTracks.Count == 0)
        {
            Debug.Log("All tracks played. Resetting track list.");
            unusedTracks = new List<AudioClip>(bgTracks);
        }

        PlayRandomTrack();
    }

    private void PlayTrack(AudioClip track)
    {
        if (track == null)
        {
            Debug.LogError("Attempted to play a null track.");
            return;
        }

        audioSource.clip = track;
        audioSource.Play();
    }
}
