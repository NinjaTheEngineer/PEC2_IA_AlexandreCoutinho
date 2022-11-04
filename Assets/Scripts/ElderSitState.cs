using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElderSitState : State {
    public float agentSpeed = 0f;
    public float minSitTime = 5f;
    public float maxSitTime = 15f;
    ElderWanderState wanderState;
    ElderController elderController;
    public ElderSitState(StateMachine stateMachine) : base(stateMachine){
        if(!stateMachine) {
            throw new System.ArgumentNullException("stateMachine");
        }
        elderController = (ElderController)stateMachine;
    }
    private void Awake() {
        elderController = GetComponent<ElderController>();
        wanderState = GetComponent<ElderWanderState>();
    }
    public override IEnumerator Start() {
        elderController.SetAgentSpeed(agentSpeed);
        Transform sitTransform = elderController.NearbyBench?.GetAvailableSit();
        if(sitTransform==null){
            yield return new WaitForSeconds(Random.Range(2, 4));
            elderController.SetState(wanderState);
            yield break;
        }
        elderController.DeactivateAgent();
        elderController.SetElderSitPosition(sitTransform);
        yield return new WaitForSeconds(Random.Range(minSitTime, maxSitTime));
        elderController.NearbyBench.FreeSittingPosition(sitTransform);
        elderController.ActivateAgent();
        elderController.SetState(wanderState);
    }
    public override void Update() {
        base.Update();
    }
    public override IEnumerator Exit() {
        return base.Exit();
    }
}
