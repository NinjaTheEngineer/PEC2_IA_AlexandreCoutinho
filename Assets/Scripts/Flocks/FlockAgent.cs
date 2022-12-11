using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class FlockAgent : MonoBehaviour {
    public FlockManager AgentFlock { private set; get;}
    SphereCollider _agentCollider;
    public SphereCollider AgentCollider {
        private set{
            _agentCollider = value;
        } 
        get => _agentCollider;
    }

    private void Start() {
        AgentCollider = GetComponent<SphereCollider>();
    }

    public void Initialize(FlockManager flock) {
        AgentFlock = flock;
    }

    public void Move(Vector3 velocity) {
        if(velocity==null){
            return;
        }
        transform.forward = velocity;
        transform.position += velocity * Time.deltaTime;
    }
}
