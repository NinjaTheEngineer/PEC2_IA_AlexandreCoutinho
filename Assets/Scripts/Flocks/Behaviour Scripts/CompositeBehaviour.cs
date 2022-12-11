using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Composite")]
public class CompositeBehaviour : FlockBehaviour {
    public FlockBehaviour[] behaviours;
    public float[] weights;
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> nearbyAgents, FlockManager flock) {
        int behavioursCount = behaviours.Length;
        if(weights.Length != behavioursCount) {
            Debug.LogError("Data mismatched in " + name, this);
            return Vector3.forward;
        }

        //Set up move
        Vector3 move = Vector3.zero;
        for (int i = 0; i < behavioursCount; i++) {
            Vector3 partialMove = behaviours[i].CalculateMove(agent, nearbyAgents, flock) * weights[i];

            if(partialMove != Vector3.zero) {
                if(partialMove.sqrMagnitude > (weights[i]*weights[i])) {
                    partialMove.Normalize();
                    partialMove *= weights[i];
                }

                move += partialMove;
            }
        }
        return move;
    }
}
