using UnityEngine;
using UnityEngine.InputSystem;
public class ExitMindfulTrigger : MonoBehaviour
{
    public Animator fadeAnimator;
    public GameObject exitPrompt;
    private bool playerInside = false;

    void Update()
    {
        if (playerInside && Keyboard.current.mKey.wasPressedThisFrame)
        {
            fadeAnimator.SetTrigger("StartFade");
            exitPrompt.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("MainCamera"))
        {
            playerInside = true;
            exitPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("MainCamera"))
        {
            playerInside = false;
            exitPrompt.SetActive(false);
        }
    }
}
