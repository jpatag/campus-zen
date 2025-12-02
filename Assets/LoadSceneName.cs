using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    // Name of the scene to load
    public string tagName;
    public string sceneName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagName))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
