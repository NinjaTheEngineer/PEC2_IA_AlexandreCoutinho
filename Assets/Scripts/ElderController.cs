using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class ElderController : StateMachine {
    public List<Vector3> waypoints;
    public Path path;
    public Vector3 targetPos; 
    public float goalDistance = 0.5f;
    public bool ReachedTargetValue;
    public int currentWaypointIndex;
    public bool fetchedWaypoints;
    private NavMeshAgent agent;
    public Bench NearbyBench;
    List<AgentController> agentsFollowing = new List<AgentController>();
    private void Awake() {
        if(!agent) {
            agent = GetComponent<NavMeshAgent>();
        }
    }
    private void Start() {
        if(!path){
            GameObject[] paths = GameObject.FindGameObjectsWithTag("Path");
            int pathsCount = paths.Length;
            if(pathsCount==0){
                Debug.Log("No paths for Ghost");
            }
            Path closestPath = null;
            float closestPathDistanceSqr=0;
            for (int i = 0; i < pathsCount; i++) {
                Path currentPath = paths[i].GetComponent<Path>();
                if(!currentPath){
                    continue;
                }
                Vector3 pathClosestWaypointPosition = currentPath.ClosestWaypoint(transform.position).transform.position;
                float pathDistanceSqr = (transform.position - pathClosestWaypointPosition).sqrMagnitude;
                if(!closestPath || closestPathDistanceSqr>pathDistanceSqr){
                    closestPath = currentPath;
                }
            }
            path = closestPath;
        }
        bool reversed = Random.Range(0, 2) == 0;
        waypoints = path.GetWaypoints(reversed);
        fetchedWaypoints = true;
        SetAgentTargetPosition(ClosestWaypoint());
        targetPos = TargetPosition;
        StartCoroutine(HandleTargetRoutine());
    }

    public void DeactivateAgent() {
        agent.enabled = false;
    }

    public void ActivateAgent() {
        agent.enabled = true;
    }

    public void SetAgentSpeed(float agentSpeed) {
        if(agent.speed == agentSpeed){
            Debug.Log("ElderController_SetAgentSpeed:: speed="+agent.speed+" is equal to new speed=" + agentSpeed+", returning");
            return;
        }
        Debug.Log("ElderController_SetAgentSpeed:: Changing from speed="+agent.speed+" to " + agentSpeed);
        agent.speed = agentSpeed;
    }

    public Vector3 ClosestWaypoint(){
        string logId = "GhostController_ClosestWaypoint::";
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
    public bool orderedPath = false;
    public Vector3 NextWaypoint() {
        if(!orderedPath){
            //Without Order
            int waypointsCount = waypoints.Count;
            Debug.Log("currentWaypointIndex="+currentWaypointIndex+" waypointsCount="+waypointsCount);
            if(waypointsCount==0) {
                Debug.Log("No waypoints => return targetPosition");
                return targetPos;
            }
            currentWaypointIndex = Random.Range(0, waypointsCount);
            Debug.Log("currentWaypointIndex="+currentWaypointIndex+" waypointsCount="+waypointsCount);
            return waypoints[currentWaypointIndex];
        } else {
            //With Order
            int waypointsCount = waypoints.Count;
            Debug.Log("currentWaypointIndex="+currentWaypointIndex+" waypointsCount="+waypointsCount);
            if(waypointsCount==0) {
                Debug.Log("No waypoints => return targetPosition");
                return targetPos;
            }
            currentWaypointIndex = currentWaypointIndex==waypointsCount-1 ? 0 : currentWaypointIndex+1;
            Debug.Log("currentWaypointIndex="+currentWaypointIndex+" waypointsCount="+waypointsCount);
            if(waypointsCount<currentWaypointIndex) {
                return waypoints[waypointsCount];
            }
            return waypoints[currentWaypointIndex];
        }
    }
    private void Update() {
        if(!ActiveState){
            return;
        }
        ActiveState.Update();
    }
    private IEnumerator HandleTargetRoutine() {
        while(true) {
            yield return new WaitForSecondsRealtime(Random.Range(0.1f,0.2f));
           //Debug.Log("TargetDistance="+GhostTargetDistance+" distanceFromGhostSqr="+distanceFromGhostSqr);
        }
    }
    
    private void OnTriggerEnter(Collider other) {
        NearbyBench = other.gameObject.GetComponent<Bench>();
        if(!NearbyBench) {
            Debug.Log("OnCollisionEnter bench is null => returning");
            return;
        }
    }
    private void OnTriggerExit(Collider other) {
        Bench bench = other.gameObject.GetComponent<Bench>();
        if(!bench) {
            Debug.Log("OnCollisionExit bench is null => returning");
        }
        NearbyBench = null;
    }
    public Vector3 TargetPosition => agent.destination;
    public bool ReachedTarget{
        get{
            float remaningDistance = agent.remainingDistance;
            return remaningDistance <= goalDistance;
        }
    } 
    public void SetElderSitPosition(Transform sit) {
        transform.position = sit.position;
        transform.rotation = sit.rotation;
    }
    public void SetAgentTargetPosition(Vector3 target){
        string logId = "SetAgentTargetPosition::";
        if(target==null){
            System.Console.WriteLine(logId + "Target is null");
        }
        agent.destination = target;
    }
}
