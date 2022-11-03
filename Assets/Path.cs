using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Path : MonoBehaviour
{
    [SerializeField]List<Transform> waypoints;
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
            }
        }
        return closestWaypoint;
    }
    public List<Vector3> GetWaypoints(bool reversed=false){
        List<Vector3> waypointsToReturn = new List<Vector3>();
        int waypointsCount = waypoints.Count;
        for (int i = reversed?waypointsCount:0; reversed ? i>0 :i<waypointsCount; i=reversed?i-1:i+1) {
            waypointsToReturn.Add(waypoints[i].transform.position);
        }
        if(reversed){
            waypointsToReturn.Reverse();
        }
        return waypointsToReturn;
    }
}
