using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class GhostController : MonoBehaviour {
    public List<Vector3> waypoints;
    public Path path;
    public Vector3 targetPos; 
    public float goalDistance = 0.5f;
    public bool ReachedTargetValue;
    public int currentWaypointIndex;
    public bool fetchedWaypoints;
    public float maxSpeed = 4f;
    public float minSpeed = 1f;
    public NavMeshAgent agent;
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
    public Vector3 NextWaypoint() {
        int waypointsCount = waypoints.Count;
        //Debug.Log("currentWaypointIndex="+currentWaypointIndex+" waypointsCount="+waypointsCount);
        if(waypointsCount==0) {
            Debug.Log("No waypoints => return targetPosition");
            return targetPos;
        }
        currentWaypointIndex = currentWaypointIndex==waypointsCount-1 ? 0 : currentWaypointIndex+1;
        //Debug.Log("currentWaypointIndex="+currentWaypointIndex+" waypointsCount="+waypointsCount);
        if(waypointsCount<currentWaypointIndex) {
            return waypoints[waypointsCount];
        }

        return waypoints[currentWaypointIndex];
    }
    private void Update() {
        if(ReachedTarget) {
            SetAgentTargetPosition(NextWaypoint());
        }
        agent.speed = IsAgentCloseBy ? maxSpeed : minSpeed;
        ReachedTargetValue = ReachedTarget;
        RemainingDistance = agent.remainingDistance;
    }
    private IEnumerator HandleTargetRoutine() {
        while(true) {
            yield return new WaitForSecondsRealtime(Random.Range(0.1f,0.2f));
           //Debug.Log("TargetDistance="+GhostTargetDistance+" distanceFromGhostSqr="+distanceFromGhostSqr);
        }

    }
    public float RemainingDistance;
    public void AddFollowingAgent(AgentController agent) {
        if(!agent) {
            Debug.Log("Agent is null => returning");
            return;
        }
        if(!agentsFollowing.Contains(agent)) {
            agentsFollowing.Add(agent);
        }
    }
    public bool IsAgentCloseBy{
        get{
            int agentsFollowingCount = agentsFollowing.Count;
            if(agentsFollowingCount==0){
                return false;
            }  
            bool isAgentClose = false;
            for (int i = 0; i < agentsFollowingCount; i++) {
                if(agentsFollowing[i].IsCloseToGhost){
                    isAgentClose = true;
                }
            }
            return isAgentClose;
        }
    }

    public Vector3 TargetPosition => agent.destination;
    private bool ReachedTarget{
        get{
            float remaningDistance = agent.remainingDistance;
            return remaningDistance <= goalDistance;
        }
    } 
    private void SetAgentTargetPosition(Vector3 target){
        string logId = "SetAgentTargetPosition::";
        if(target==null){
            System.Console.WriteLine(logId + "Target is null");
        }
        agent.destination = target;
    }
}
