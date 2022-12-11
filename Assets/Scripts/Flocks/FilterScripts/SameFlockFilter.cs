using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Filter/Same Flock")]
public class SameFlockFilter : ContextFilter {
    public override List<Transform> Filter(FlockAgent agent, List<Transform> neighbourAgents) {
        List<Transform> filteredNeighbours = new List<Transform>();
        int neighbourAgentsCount = neighbourAgents.Count;
        
        for (int i = 0; i < neighbourAgentsCount; i++) {
            FlockAgent currentAgent = neighbourAgents[i].GetComponent<FlockAgent>();
            if(currentAgent!=null && currentAgent.AgentFlock==agent.AgentFlock) {
                filteredNeighbours.Add(neighbourAgents[i]);
            }
        }
        return filteredNeighbours;
    }
}
