using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform targetCamera; // Assign your Main Camera here in the Inspector
    public Vector3 offset; // Define the desired offset from the camera

    void LateUpdate() // Use LateUpdate to ensure camera has finished moving
    {
        if (targetCamera != null)
        {
            transform.position = targetCamera.position + offset;
            // If you also want the object to match the camera's rotation:
            // transform.rotation = targetCamera.rotation; 
        }
    }
}