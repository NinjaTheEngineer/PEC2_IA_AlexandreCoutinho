using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSeekerController : AgentController {
    [SerializeField]AgentController targetAgent;
    public float AgentSafeTargetDistance = 5f;
    public bool AgentNearby;

    public override void Awake() {
        base.Awake();
        if(!targetAgent){
            SetTargetGhost();
        }
        targetAgent?.AddFollowingAgent(this);
        StartCoroutine(CloseToGhostRoutine());   
    }
    public override void Update() {
        base.Update();

        if(ReachedTarget) {
            SetTargetPosition(targetAgent.transform.position);
        }
    }
    private void SetTargetGhost() {
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
            int ghostsCount = ghosts.Length;
            if(ghostsCount==0){
                Debug.Log("No ghosts for " + this);
                return;
            }
            AgentController closestGhost = null;
            float closestGhostDistanceSqr=0;
            for (int i = 0; i < ghostsCount; i++) {
                AgentController ghost = ghosts[i].GetComponent<AgentController>();
                if(!ghost || !ghost.HasFollowers){
                    continue;
                }
                float distanceFromGhostSqr = (transform.position-ghost.transform.position).sqrMagnitude;
                if(!closestGhost || closestGhostDistanceSqr>distanceFromGhostSqr){
                    closestGhostDistanceSqr = distanceFromGhostSqr;
                    closestGhost = ghost;
                }
            }
            targetAgent = closestGhost;
    }
    private IEnumerator CloseToGhostRoutine() {
        while(true) {
            yield return new WaitForSecondsRealtime(Random.Range(0.1f,0.3f));
            if(!targetAgent) {
                continue;
            }
            float distanceFromGhostSqr = (transform.position-targetAgent.transform.position).sqrMagnitude;
            AgentNearby = distanceFromGhostSqr < AgentSafeTargetDistance;
           //Debug.Log("TargetDistance="+GhostTargetDistance+" distanceFromGhostSqr="+distanceFromGhostSqr);
        }
    }
}
    
