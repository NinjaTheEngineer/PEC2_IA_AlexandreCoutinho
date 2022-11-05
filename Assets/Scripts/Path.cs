using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Path : MonoBehaviour
{
    [SerializeField]List<Transform> waypoints;
    public bool visible = false;
    private void Awake() {
        waypoints = GetComponentsInChildren<Transform>().ToList();
        waypoints.RemoveAt(0);
        int waypointsCount = waypoints.Count;
        for (int i = 0; i < waypointsCount; i++) {
            waypoints[i].GetComponent<MeshRenderer>().enabled = visible;
        }
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
        Debug.Log("Closest waypoint from " + this.name + " is " + closestWaypoint.position);
        return closestWaypoint;
    }
    public List<Vector3> GetWaypoints(bool reversed=false){
        string logId = "Path_GetWaypoints::";
        List<Vector3> waypointsToReturn = new List<Vector3>();
        int waypointsCount = waypoints.Count;
        if(waypointsCount==0){
            Debug.Log(logId+"There are no waypoints => return null");
            return null;
        }
        for (int i = 0; i < waypointsCount; i++) {
            waypointsToReturn.Add(waypoints[i].transform.position);
        }
        if(reversed) {
            waypointsToReturn.Reverse();
        }
        return waypointsToReturn;
    }
}
