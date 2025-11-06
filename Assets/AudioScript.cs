using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        if (audioSource != null)
        {
            StartCoroutine(PlayAudioLoop());
        }
    }

    IEnumerator PlayAudioLoop()
    {
        while (true)
        {
            // Wait a random time between 0 and 5 seconds
            float randomWaitTime = Random.Range(0f, 3f);
            yield return new WaitForSeconds(randomWaitTime);
            
            // Play the audio
            audioSource.Play();
            
            // Wait until the audio finishes playing
            yield return new WaitWhile(() => audioSource.isPlaying);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}