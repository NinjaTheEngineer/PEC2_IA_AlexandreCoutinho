using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElderSitState : State {
    public float minSitTime = 5f;
    public float maxSitTime = 15f;
    ElderWanderState wanderState;
    ElderController elderController;
    private void Awake() {
        elderController = GetComponent<ElderController>();
        wanderState = GetComponent<ElderWanderState>();
    }
    public override IEnumerator Start() {
        elderController.AgentController.SetAgentSpeed(0);
        Transform sitTransform = elderController.NearbyBench?.GetAvailableSit();
        if(sitTransform==null){
            yield return new WaitForSeconds(Random.Range(2, 4));
            elderController.SetState(wanderState);
            yield break;
        }
        elderController.AgentController.DeactivateAgent();
        elderController.SetElderSitPosition(sitTransform);
        yield return new WaitForSeconds(Random.Range(minSitTime, maxSitTime));
        elderController.NearbyBench.FreeSittingPosition(sitTransform);
        elderController.AgentController.ActivateAgent();
        elderController.SetState(wanderState);
    }
}
