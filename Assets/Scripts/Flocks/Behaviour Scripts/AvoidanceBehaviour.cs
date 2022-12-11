using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Avoidance")]
public class AvoidanceBehaviour : FilteredFlockBehaviour {
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> neighbourAgents, FlockManager flock) {
        int neighbourAgentsCount = neighbourAgents.Count;
        //If no neighbours, maintain current aligment
        if(neighbourAgentsCount==0){
            Debug.Log("No neighbours nearby.");
            return Vector3.forward;
        }

        //Add all points together and average
        Vector3 avoidanceMove = Vector3.zero;
        List<Transform> filteredNeighbours = (filter==null)?neighbourAgents:filter.Filter(agent, neighbourAgents);
        int filteredNeighboursCount = filteredNeighbours.Count;
        int avoidedAgentsCount = 0;
        for (int i = 0; i < filteredNeighboursCount; i++) {
            if(Vector3.SqrMagnitude(filteredNeighbours[i].position - agent.transform.position) < flock.SquareAvoidanceRadius){
               avoidedAgentsCount++;
               avoidanceMove += agent.transform.position - filteredNeighbours[i].position; 
            }
        }
        if(avoidedAgentsCount>0) {
            avoidanceMove /= avoidedAgentsCount;
        }
        return avoidanceMove;
    }
}