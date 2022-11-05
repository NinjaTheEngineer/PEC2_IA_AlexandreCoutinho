using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class ElderController : StateMachine {
    public AgentWithPathController AgentController;
    public Bench NearbyBench;
    List<AgentController> agentsFollowing = new List<AgentController>();
    private void Awake() {
        if(AgentController==null){
            AgentController = GetComponent<AgentWithPathController>();
        }
    }
    private void Start() {
        AgentController.SetTargetPosition(AgentController.ClosestWaypoint());
    }

    private void Update() {
        if(!ActiveState){
            return;
        }
        ActiveState.Update();
    }
    
    private void OnTriggerEnter(Collider other) {
        NearbyBench = other.gameObject.GetComponent<Bench>();
        if(!NearbyBench) {
            Debug.Log("OnCollisionEnter bench is null => returning");
            return;
        }
    }
    private void OnTriggerExit(Collider other) {
        Bench bench = other.gameObject.GetComponent<Bench>();
        if(!bench) {
            Debug.Log("OnCollisionExit bench is null => returning");
        }
        NearbyBench = null;
    }
    public void SetElderSitPosition(Transform sit) {
        transform.position = sit.position;
        transform.rotation = sit.rotation;
    }
}
