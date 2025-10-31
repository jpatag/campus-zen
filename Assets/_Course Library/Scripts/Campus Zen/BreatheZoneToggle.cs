using UnityEngine;

public class BreathZoneToggle : MonoBehaviour
{
    [Tooltip("Tag used by the player GameObject.")]
    public string playerTag = "Player";

    [Tooltip("Root UI GameObject to show/hide (e.g., BreathCircle or its parent).")]
    public GameObject breathUIRoot;

    void Start()
    {
        // Force hidden at scene start
        if (breathUIRoot) breathUIRoot.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && breathUIRoot != null)
            breathUIRoot.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag) && breathUIRoot != null)
            breathUIRoot.SetActive(false);
    }
}
