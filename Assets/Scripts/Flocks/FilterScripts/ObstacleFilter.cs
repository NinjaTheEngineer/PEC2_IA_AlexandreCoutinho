using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Filter/Obstacle Filter")]
public class ObstacleFilter : ContextFilter {
    public override List<Transform> Filter(FlockAgent agent, List<Transform> neighbourAgents) {
        List<Transform> avoidObstacles = new List<Transform>();
        int obstaclesCount = agent.AgentFlock.obstacles.Count;
        
        for (int i = 0; i < obstaclesCount; i++) {
            float sqrDistanceFromAgent = (agent.AgentFlock.obstacles[i].transform.position - agent.transform.position).sqrMagnitude;
            if(sqrDistanceFromAgent < (agent.AgentFlock.SquareAvoidanceRadius)) {
                avoidObstacles.Add(agent.AgentFlock.obstacles[i].transform);
            }
        }
        return avoidObstacles;
    }
}
