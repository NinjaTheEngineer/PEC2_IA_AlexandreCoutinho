using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Path : MonoBehaviour
{
    [SerializeField]List<Transform> waypoints;
    [SerializeField]int currentWaypointIndex;
    private void Awake() {
        waypoints = GetComponentsInChildren<Transform>().ToList();
        waypoints.RemoveAt(0);
    }
    public Transform ClosestWaypoint(Vector3 position){
        string logId = "Path_ClosestWaypoint::";
        if(position==null){
            Debug.Log(logId+"Position is null => return null");
            return null;
        }
        int waypointsCount = waypoints.Count;
        if(waypointsCount==0){
            Debug.Log(logId+"There are no waypoints => return null");
            return null;    
        }
        Transform closestWaypoint = null;
        float minDistanceSqr = 0;
        for (int i = 0; i < waypointsCount; i++) {
            float distanceFromWaypointSqr = (position-waypoints[i].localPosition).sqrMagnitude;
            if(!closestWaypoint || minDistanceSqr>distanceFromWaypointSqr){
                minDistanceSqr = distanceFromWaypointSqr;
                closestWaypoint = waypoints[i];
                currentWaypointIndex = i;
            }
        }
        return closestWaypoint;
    }
    public Transform NextWaypoint{
        get{
            int waypointsCount = waypoints.Count;
            if(waypointsCount==0){
                Debug.Log("No waypoints => return null");
                return null;
            }
            currentWaypointIndex = currentWaypointIndex==waypointsCount?0:currentWaypointIndex+1;
            if(waypointsCount<currentWaypointIndex){
                return waypoints[waypointsCount];
            }
            Debug.Log("currentWaypointIdex="+currentWaypointIndex+",waypointsCount="+waypointsCount);

            return waypoints[currentWaypointIndex];
        }
    }
    public Transform CurrentWaypoint => waypoints[currentWaypointIndex];
}
