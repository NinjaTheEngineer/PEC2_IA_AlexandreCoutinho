using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Cohesion")]
public class CohesionBehaviour : FilteredFlockBehaviour {
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> neighbourAgents, FlockManager flock) {
        int neighbourAgentsCount = neighbourAgents.Count;
        //If no neighbours, return no adjustment
        if(neighbourAgentsCount==0){
            Debug.Log("No neighbours nearby.");
            return Vector3.forward;
        }

        //Add all points together and average
        Vector3 cohesionMove = Vector3.zero;
        List<Transform> filteredNeighbours = (filter==null)?neighbourAgents:filter.Filter(agent, neighbourAgents);
        int filteredNeighboursCount = filteredNeighbours.Count;
        for (int i = 0; i < filteredNeighboursCount; i++) {
            cohesionMove += filteredNeighbours[i].position;
        }
        if(filteredNeighboursCount>0){
            cohesionMove /= filteredNeighboursCount;
        }

        //Create offset from agent position
        cohesionMove -= agent.transform.position;
        return cohesionMove;
    }
}
