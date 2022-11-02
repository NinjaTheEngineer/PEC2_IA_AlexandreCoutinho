using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour {
    public float GoalDistance = 2f;
    public float GhostTargetDistance = 5f;
    public bool IsCloseToGhost;
    NavMeshAgent agent;
    [SerializeField]GhostController targetGhost;
    Vector3 currentLocation;
    private void Awake() {
        if(!agent){
            agent = GetComponent<NavMeshAgent>();
        }
        if(!targetGhost){
            GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
            int ghostsCount = ghosts.Length;
            if(ghostsCount==0){
                Debug.Log("No ghosts for " + this);
            }
            GhostController closestGhost = null;
            float closestGhostDistanceSqr=0;
            for (int i = 0; i < ghostsCount; i++) {
                GhostController ghost = ghosts[i].GetComponent<GhostController>();
                if(!ghost){
                    continue;
                }
                float distanceFromGhostSqr = (transform.position-ghost.transform.position).sqrMagnitude;
                if(!closestGhost || closestGhostDistanceSqr>distanceFromGhostSqr){
                    closestGhostDistanceSqr = distanceFromGhostSqr;
                    closestGhost = ghost;
                }
            }
            targetGhost = closestGhost;
        }
        StartCoroutine(CloseToGhostRoutine());
    }
    bool IsAtTargetLocation => TargetPosition!=null?(transform.position - TargetPosition).sqrMagnitude < GoalDistance:false;
    public Vector3 TargetPosition => agent.destination;

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(camRay, out hit, 100)) {
                SetAgentTargetPosition(hit.point);
            }
        }
        if(IsAtTargetLocation){
            SetAgentTargetPosition(targetGhost.transform.position);
        }
    }
    private IEnumerator CloseToGhostRoutine(){
        while(true) {
            yield return new WaitForSecondsRealtime(Random.Range(0.1f,0.2f));
            if(!targetGhost) {
                continue;
            }
            float distanceFromGhostSqr = (transform.position-targetGhost.transform.position).sqrMagnitude;
            IsCloseToGhost = distanceFromGhostSqr < GhostTargetDistance;
            Debug.Log("TargetDistance="+GhostTargetDistance+" distanceFromGhostSqr="+distanceFromGhostSqr);
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
