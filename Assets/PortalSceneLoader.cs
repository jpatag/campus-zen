using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;   // ← THIS is the missing namespace

public class PortalSceneLoader : MonoBehaviour
{
    public string sceneName;

    private void OnTriggerEnter(Collider other)
    {
        // Use GetComponentInParent in case the collider is on a child object
        XROrigin rig = other.GetComponentInParent<XROrigin>();

        if (rig != null)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
