using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour {
    protected StateMachine stateMachine;

    protected State(StateMachine stateMachine) {
        if(!stateMachine) {
            Debug.Log("stateMachine is null => no-op");
            throw new System.ArgumentNullException("stateMachine");
        }
        this.stateMachine = stateMachine;
    }
    public virtual IEnumerator Start() {
        yield break;
    }

    public virtual void Update() {
    }

    public virtual IEnumerator Exit() {
        yield break;
    }
}
