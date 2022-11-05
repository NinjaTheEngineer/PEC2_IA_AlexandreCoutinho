using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour {
    protected StateMachine stateMachine;
    public virtual IEnumerator Start() {
        yield break;
    }

    public virtual void Update() {
    }

    public virtual IEnumerator Exit() {
        yield break;
    }
}
