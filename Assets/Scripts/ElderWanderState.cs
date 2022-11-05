using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElderWanderState : State {
    ElderController elderController;
    ElderSitState sitState;
    ElderStopState stopState;
    private void Awake() {
        elderController = GetComponent<ElderController>();
        sitState = GetComponent<ElderSitState>();
        stopState = GetComponent<ElderStopState>();
    }
    public override IEnumerator Start() {
        elderController.AgentController.ActivateAgent();
        elderController.AgentController.SetAgentSpeed(elderController.AgentController.DefaultSpeed);
        elderController.AgentController.SetTargetPosition(elderController.AgentController.NextWaypoint());
        return base.Start();
    }
    public override void Update() {
        base.Update();
        if(elderController.ActiveState!=this){
            return;
        }
        if(elderController.AgentController.ReachedTarget) {
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
