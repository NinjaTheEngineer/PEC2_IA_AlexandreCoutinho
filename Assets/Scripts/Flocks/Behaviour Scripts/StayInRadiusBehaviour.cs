using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/StayInRadius")]
public class StayInRadiusBehaviour : FlockBehaviour {
    public bool flockManagerAsCenter = true;
    public Vector3 center;
    public float radius = 5f;
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> nearbyAgents, FlockManager flock) {
        if(flockManagerAsCenter) {
            center = flock.transform.position;
        }
        Vector3 centerOffset = center - agent.transform.position;
        float t = centerOffset.magnitude/radius;
        
        if(t<0.9f){
            return Vector3.zero;
        }
        
        return centerOffset*t*t;
    }
}