using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostController : MonoBehaviour
{
    public Path path;
    public float reachDistance = 0.5f;
    public NavMeshAgent agent;
    List<AgentController> agentsFollowing = new List<AgentController>();
    int currentWaypointIndex;
    private void Awake() {
        if(!agent){
            agent = GetComponent<NavMeshAgent>();
        }
    }
    private void Start() {
        if(!path){
            GameObject[] paths = GameObject.FindGameObjectsWithTag("Path");
            int pathsCount = paths.Length;
            if(pathsCount==0){
                Debug.Log("No paths for " + this);
            }
            Path closestPath = null;
            float closestPathDistanceSqr=0;
            for (int i = 0; i < pathsCount; i++) {
                Path path = paths[i].GetComponent<Path>();
                if(!path){
                    continue;
                }
                Vector3 pathClosestWaypointPosition = path.ClosestWaypoint(transform.position).transform.position;
                float pathDistanceSqr = (transform.position - pathClosestWaypointPosition).sqrMagnitude;
                if(!closestPath || closestPathDistanceSqr>pathDistanceSqr){
                    closestPath = path;
                }
            }
            path = closestPath;
        }
        SetAgentTargetPosition(path.CurrentWaypoint.position);
    }

    private void Update() {
        if(ReachedTarget){
            SetAgentTargetPosition(path.NextWaypoint.position);
        }
        agent.speed = IsAgentCloseBy?5:0;
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
    private bool ReachedTarget => agent.remainingDistance < reachDistance;
    private void SetAgentTargetPosition(Vector3 target){
        string logId = "SetAgentTargetPosition::";
        if(target==null){
            System.Console.WriteLine(logId + "Target is null");
        }
        agent.destination = target;
    }
}
