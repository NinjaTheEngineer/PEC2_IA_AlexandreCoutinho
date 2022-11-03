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
        agent.speed = Random.Range(1,1.33f);
        targetGhost?.AddFollowingAgent(this);
        StartCoroutine(CloseToGhostRoutine());
    }
    bool IsAtTargetLocation => TargetPosition!=null?(transform.position - TargetPosition).sqrMagnitude < GoalDistance:false;
    Vector3 _targetPosition;
    public Vector3 TargetPosition {
        get => _targetPosition;
        private set {
            if(value==null || value==_targetPosition) {
                return;
            }
            _targetPosition = value;
            agent.destination = _targetPosition;
        } 
    } 
    bool isMouseTargetLocation = false;
    private void Update() {
        HandleMouseClick();
        if(isMouseTargetLocation && !IsAtTargetLocation){
            return;
        }
        isMouseTargetLocation = false;
        SetTargetPosition(targetGhost.transform.position);
    }
    private void HandleMouseClick() {
        if (Input.GetMouseButtonDown(1)) {
            RaycastHit hit;
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(camRay, out hit, 100)) {
                SetTargetPosition(hit.point);
                isMouseTargetLocation = true;
            }
        }
    }
    private IEnumerator CloseToGhostRoutine() {
        while(true) {
            yield return new WaitForSecondsRealtime(Random.Range(0.1f,0.3f));
            if(!targetGhost) {
                continue;
            }
            float distanceFromGhostSqr = (transform.position-targetGhost.transform.position).sqrMagnitude;
            IsCloseToGhost = distanceFromGhostSqr < GhostTargetDistance;
           //Debug.Log("TargetDistance="+GhostTargetDistance+" distanceFromGhostSqr="+distanceFromGhostSqr);
        }

    }

    private void SetTargetPosition(Vector3 target) {
        string logId = "SetAgentTargetPosition::";
        if(target==null) {
            System.Console.WriteLine(logId + "Target is null");
        }
        TargetPosition = target;
    }
}
