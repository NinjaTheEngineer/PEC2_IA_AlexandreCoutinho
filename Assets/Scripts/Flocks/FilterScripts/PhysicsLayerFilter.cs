using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Filter/Physics Layer")]
public class PhysicsLayerFilter : ContextFilter {
    public LayerMask mask;
    public override List<Transform> Filter(FlockAgent agent, List<Transform> neighbourAgents) {
        List<Transform> filteredNeighbours = new List<Transform>();
        int neighbourAgentsCount = neighbourAgents.Count;
        
        for (int i = 0; i < neighbourAgentsCount; i++) {
            if(mask == (mask | (1 << neighbourAgents[i].gameObject.layer))) {
                filteredNeighbours.Add(neighbourAgents[i]);
            }
        }
        return filteredNeighbours;
    }
}

