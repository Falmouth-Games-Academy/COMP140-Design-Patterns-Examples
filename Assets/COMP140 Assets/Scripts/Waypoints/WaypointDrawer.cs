using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WaypointDrawer : MonoBehaviour
{
    
    List<Transform> waypoints=new List<Transform>();

    [SerializeField]
    Color lineColour = Color.yellow;

    void Start()
    {
        for (int i=0;i<transform.childCount;i++)
        {
            Transform waypoint = transform.GetChild(i);
            waypoints.Add(waypoint);
        }

        foreach(Transform waypoint in waypoints)
        {
            Debug.Log("Waypoints " + waypoint.name);
        }
    }

    void OnDrawGizmos()
    {
        if (waypoints.Count > 0)
        {
            Vector3 prev = waypoints[0].position;
            for (int i = 0; i < waypoints.Count; i++)
            {
                Vector3 next = waypoints[(i + 1) % waypoints.Count].position;
                Gizmos.color = lineColour;
                Gizmos.DrawLine(prev, next);
                prev = next;
            }
        }
    }
}
