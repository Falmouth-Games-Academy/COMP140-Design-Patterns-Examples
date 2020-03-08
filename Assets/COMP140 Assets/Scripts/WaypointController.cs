using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(AICharacterControl))]
public class WaypointController : MonoBehaviour
{
    //This drives the character
    [SerializeField]
    AICharacterControl AIController;

    //The parent of all waypoints
    [SerializeField]
    Transform waypointParent;

    //All waypoints, these are populated from the parent above
    List<Transform> waypoints = new List<Transform>();

    //Controls what waypoint we are moving to
    Transform currentWaypoint=null;
    int currentWaypointIndex = 0;

    //The distance to check if we have hit a waypoint
    [SerializeField]
    float distanceThreshold = 2.0f;


    // Start is called before the first frame update
    void Start()
    {
        //Grab the AI Controller
        AIController = GetComponent<AICharacterControl>();
        //Cycle through all children to grab the waypoints
        for (int i = 0; i < waypointParent.childCount; i++)
        {
            //Add the waypoint to our list
            Transform waypoint = waypointParent.GetChild(i);
            waypoints.Add(waypoint);
        }
        //Asign the first waypoint
        currentWaypoint = waypoints[0];
        //Set the AI target
        AIController.SetTarget(currentWaypoint);
    }

    void Update()
    {
            UpdateWalk();
    }


    //Function to return the distance to a waypoint
    float GetDistanceToCurrentWaypoint()
    {
        Vector3 distance = currentWaypoint.position - transform.position;
        return distance.magnitude;
    }

    //Update Walking of the waypoints
    void UpdateWalk()
    {
        //Get the Distance to the waypoint and check the threshold distance
        if (GetDistanceToCurrentWaypoint() < distanceThreshold)
        {
            //move to the next waypoint
            currentWaypointIndex++;
            //Should we cycle ?
            if (currentWaypointIndex>waypoints.Count-1)
            {
                //Return to the new waypoint
                currentWaypointIndex = 0;
            }
            Debug.Log("Next Waypoint is " + currentWaypointIndex.ToString());
            //get the new waypoint
            currentWaypoint = waypoints[currentWaypointIndex];
            //Set the AI Target
            AIController.SetTarget(currentWaypoint);
        }
    }
}
