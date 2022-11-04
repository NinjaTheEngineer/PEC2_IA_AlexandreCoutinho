using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElderStopState : State {
    public float agentSpeed = 0f;
    ElderController elderController;
    ElderWanderState wanderState;
    public ElderStopState(StateMachine stateMachine) : base(stateMachine) {
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
        yield return new WaitForSeconds(Random.Range(2, 6));
        elderController.SetState(wanderState);
    }
    public override void Update() {
        base.Update();
    }
    public override IEnumerator Exit() {
        return base.Exit();
    }
}