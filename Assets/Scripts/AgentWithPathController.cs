using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentWithPathController : AgentController {
    public bool randomPathing = false;
    public Path Path;
    private List<Vector3> waypoints;
    private Vector3 targetPos; 
    private int currentWaypointIndex;
    private bool fetchedWaypoints;
    
    private void Start() {
        if(!Path){
            SetPath();
        }
        bool reversed = Random.Range(0, 2) == 0;
        waypoints = Path.GetWaypoints(reversed);
        fetchedWaypoints = true;
        SetTargetPosition(ClosestWaypoint());
        targetPos = TargetPosition;
    }    
    public override void Update() {
        base.Update();
        if(Active && ReachedTarget) {
            SetTargetPosition(NextWaypoint());
        }
    }

    private void SetPath() {
        GameObject[] paths = GameObject.FindGameObjectsWithTag("Path");
            int pathsCount = paths.Length;
            if(pathsCount==0){
                Debug.Log("No paths for Ghost");
                return;
            }
            Path closestPath = null;
            float closestPathDistanceSqr=0;
            for (int i = 0; i < pathsCount; i++) {
                Path currentPath = paths[i].GetComponent<Path>();
                if(!currentPath){
                    continue;
                }
                Debug.Log("Path=" + currentPath.name);

                Vector3 pathClosestWaypointPosition = currentPath.ClosestWaypoint(transform.position).transform.position;
                float pathDistanceSqr = (transform.position - pathClosestWaypointPosition).sqrMagnitude;
                if(!closestPath || closestPathDistanceSqr>pathDistanceSqr){
                    closestPath = currentPath;
                    closestPathDistanceSqr = pathDistanceSqr;
                }
            }
            Path = closestPath;
    }
    public Vector3 ClosestWaypoint(){
        string logId = "AgentWithPathController_ClosestWaypoint::";
        int waypointsCount = waypoints.Count;
        if(waypointsCount==0){
            Debug.Log(logId+"There are no waypoints => return targetPosition");
            return targetPos;    
        }
        Vector3 closestWaypoint = targetPos;
        float minDistanceSqr = 0;
        for (int i = 0; i < waypointsCount; i++) {
            float distanceFromWaypointSqr = (transform.position-waypoints[i]).sqrMagnitude;
            if(minDistanceSqr==0 || minDistanceSqr>distanceFromWaypointSqr){
                minDistanceSqr = distanceFromWaypointSqr;
                closestWaypoint = waypoints[i];
                currentWaypointIndex = i;
            }
        }
        return closestWaypoint;
    }
    public Vector3 NextWaypoint() {
        int waypointsCount = waypoints.Count;
        if(waypointsCount==0) {
            Debug.Log("No waypoints => return targetPosition");
            return targetPos;
        }

        if(randomPathing) {
            Debug.Log("currentWaypointIndex="+currentWaypointIndex+" waypointsCount="+waypointsCount);
            currentWaypointIndex = Random.Range(0, waypointsCount);
            Debug.Log("currentWaypointIndex="+currentWaypointIndex+" waypointsCount="+waypointsCount);
            return waypoints[currentWaypointIndex];
        } else {
            Debug.Log("currentWaypointIndex="+currentWaypointIndex+" waypointsCount="+waypointsCount);
            currentWaypointIndex = currentWaypointIndex==waypointsCount-1 ? 0 : currentWaypointIndex+1;
            Debug.Log("currentWaypointIndex="+currentWaypointIndex+" waypointsCount="+waypointsCount);
            if(waypointsCount<currentWaypointIndex) {
                return waypoints[waypointsCount];
            }
            return waypoints[currentWaypointIndex]; 
        }
    }

}
