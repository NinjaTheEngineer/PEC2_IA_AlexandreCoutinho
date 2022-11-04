using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElderWanderState : State {
    public float agentSpeed = 0.5f;
    ElderController elderController;
    ElderSitState sitState;
    ElderStopState stopState;
    public ElderWanderState(StateMachine stateMachine) : base(stateMachine) {
        if(!stateMachine) {
            throw new System.ArgumentNullException("stateMachine");
        }
        elderController = (ElderController)stateMachine;
    }
    private void Awake() {
        elderController = GetComponent<ElderController>();
        sitState = GetComponent<ElderSitState>();
        stopState = GetComponent<ElderStopState>();
    }
    public override IEnumerator Start() {
        elderController.ActivateAgent();
        elderController.SetAgentSpeed(agentSpeed);
        elderController.SetAgentTargetPosition(elderController.NextWaypoint());
        return base.Start();
    }
    public override void Update() {
        base.Update();
        if(elderController.ActiveState!=this){
            return;
        }
        if(elderController.ReachedTarget) {
            if(elderController.NearbyBench) {
                elderController.SetState(sitState);
                return;
            }
            elderController.SetState(stopState);
            return;
        }
    }
    public override IEnumerator Exit() {
        return base.Exit();
    }
}
