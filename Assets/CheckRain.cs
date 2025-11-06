using UnityEngine;
using UnityEngine.UI;

public class ToggleAudioParticleController : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Toggle toggle;

    [Header("Audio Components")]
    [SerializeField] private AudioSource audioSource;

    [Header("Particle System")]
    [SerializeField] private ParticleSystem particleSystem;

    private void Update()
    {
        if (toggle.isOn)
        {
            // Enable looping audio
            if (!audioSource.isPlaying)
            {
                audioSource.loop = true;
                audioSource.Play();
            }

            // Enable particle system
            if (!particleSystem.isPlaying)
            {
                particleSystem.Play();
            }
        }
        else
        {
            // Disable audio
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            // Disable particle system
            if (particleSystem.isPlaying)
            {
                particleSystem.Stop();
            }
        }
    }
}