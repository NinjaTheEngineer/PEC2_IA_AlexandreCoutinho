using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/SteeredCohesion")]
public class SteeredCohesionBehaviour : FilteredFlockBehaviour {
    Vector3 currentVelocity;
    public float agentSmoothTime = 0.5f;
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> neighbourAgents, FlockManager flock) {
        int neighbourAgentsCount = neighbourAgents.Count;
        //If no neighbours, return no adjustment
        if(neighbourAgentsCount==0){
            Debug.Log("No neighbours nearby.");
            return Vector3.forward;
        }

        //Add all points together and average
        Vector3 cohesionMove = Vector3.forward;
        List<Transform> filteredNeighbours = (filter==null)?neighbourAgents:filter.Filter(agent, neighbourAgents);
        int filteredNeighboursCount = filteredNeighbours.Count;
        for (int i = 0; i < filteredNeighboursCount; i++) {
            cohesionMove += filteredNeighbours[i].position;
        }
        Debug.Log("1-CohesionMove=" + cohesionMove);
        if(filteredNeighboursCount>0){
            cohesionMove /= filteredNeighboursCount;
        }
        Debug.Log("2-CohesionMove=" + cohesionMove);
        //Create offset from agent position
        cohesionMove -= agent.transform.position;
        Debug.Log("3-CohesionMove=" + cohesionMove);
        currentVelocity = Vector3.zero;
        cohesionMove = Vector3.SmoothDamp(agent.transform.forward, cohesionMove, ref currentVelocity, agentSmoothTime);
        Debug.Log("4-agent.transform.forward=" + agent.transform.forward + " currentVelocity="+currentVelocity+" agentSmoothTime="+agentSmoothTime);
        Debug.Log("4-CohesionMove=" + (cohesionMove==null?"NULL":cohesionMove.ToString()));
        return cohesionMove;
    }
}
