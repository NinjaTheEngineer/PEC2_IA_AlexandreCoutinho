using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Alignment")]
public class AlignmentBehaviour : FilteredFlockBehaviour {
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> neighbourAgents, FlockManager flock) {
        int neighbourAgentsAgentsCount = neighbourAgents.Count;
        //If no neighbours, maintain current aligment
        if(neighbourAgentsAgentsCount==0){
            Debug.Log("No neighbours nearby.");
            return Vector3.forward;
        }

        //Add all points together and average
        Vector3 alignmentMove = Vector3.zero;
        List<Transform> filteredNeighbours = (filter==null)?neighbourAgents:filter.Filter(agent, neighbourAgents);
        int filteredNeighboursCount = filteredNeighbours.Count;
        for (int i = 0; i < filteredNeighboursCount; i++) {
            alignmentMove += filteredNeighbours[i].forward;
        }
        if(filteredNeighboursCount>0){
            alignmentMove /= filteredNeighboursCount;
        }
        return alignmentMove;
    }
}
