using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElderStopState : State {
    public float minTimeStopped = 2;
    public float maxTimeStopped = 6;
    ElderController elderController;
    ElderWanderState wanderState;
    private void Awake() {
        elderController = GetComponent<ElderController>();
        wanderState = GetComponent<ElderWanderState>();
    }
    public override IEnumerator Start() {
        elderController.AgentController.SetAgentSpeed(0);
        yield return new WaitForSeconds(Random.Range(minTimeStopped, maxTimeStopped));
        elderController.SetState(wanderState);
    }
}