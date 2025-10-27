using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    // Name of the scene to load
    public string sceneName;

    private void OnTriggerEnter(Collider other)
    {
        // Only trigger when player enters
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
