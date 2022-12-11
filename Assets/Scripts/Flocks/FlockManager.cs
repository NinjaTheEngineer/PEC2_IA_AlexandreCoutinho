using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour {
    public FlockAgent agentPrefab;
    public List<Transform> obstacles = new List<Transform>();
    List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehaviour flockBehaviour;

    [Range(10, 250)]
    public int flockSize = 100;
    const float AgentDensity = 0.08f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;

    [Range(0f, 10f)]
    public float neighbourRadius = 1.5f;

    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    float squareMaxSpeed;
    float squareNeighbourRadius;
    public float SquareAvoidanceRadius { private set; get;}
    private void Start() {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighbourRadius = neighbourRadius * neighbourRadius;
        SquareAvoidanceRadius = squareNeighbourRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for (int i = 0; i < flockSize; i++) {
            FlockAgent newAgent = Instantiate(
                agentPrefab,
                transform.position + Random.insideUnitSphere*flockSize*AgentDensity,
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                transform);
            newAgent.name = "Agent_"+i;
            newAgent.Initialize(this);
            agents.Add(newAgent);
        }
    }

    private void Update() {
        for (int i = 0; i < flockSize; i++) {
            FlockAgent currentAgent = agents[i];
            List<Transform> context = GetNearbyObjects(currentAgent);
            
            Vector3 move = flockBehaviour.CalculateMove(currentAgent, context, this);
            Debug.Log("Move="+move);
            move *= driveFactor;

            if(move.sqrMagnitude > squareMaxSpeed) {
                move = move.normalized * maxSpeed;
            }
            currentAgent.Move(move);
        }
    }

    List<Transform> GetNearbyObjects(FlockAgent agent) {
        List<Transform> nearbyAgents = new List<Transform>();
        for (int i = 0; i < flockSize; i++) {
            float sqrDistanceFromAgent = (agents[i].transform.position - agent.transform.position).sqrMagnitude;
            if(sqrDistanceFromAgent < (neighbourRadius*neighbourRadius)) {
                nearbyAgents.Add(agents[i].transform);
            }
        }
        Physics.OverlapSphere(agent.transform.position, neighbourRadius);
        return nearbyAgents;
    }
}
