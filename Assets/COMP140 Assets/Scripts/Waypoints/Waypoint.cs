using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{

    [SerializeField]
    Color displayColour = Color.red;

    [SerializeField]
    float drawRadius=1.5f;
    
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = displayColour;
        Gizmos.DrawSphere(transform.position, drawRadius);
    }
}
