using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour {
    public State ActiveState;
    public void SetState(State newState) {
        string logId = "StateMachine_SetState:: ";
        if(newState==null) {
            Debug.Log(logId+"new state is null => no-op");
            return;
        }
        if(!ActiveState) {
            ActiveState = newState;
            Debug.Log(logId+"ActiveState changed from null to " + newState);
            StartCoroutine(ActiveState.Start());
            return;
        }
        if(newState==ActiveState) {
            Debug.Log(logId+"new state is equal to currentState => no-op");
            return;
        }
        ActiveState.Exit();
        Debug.Log(logId+"ActiveState changed from " + ActiveState +" to " + newState);
        ActiveState = newState;
        StartCoroutine(ActiveState.Start());
    }
}
