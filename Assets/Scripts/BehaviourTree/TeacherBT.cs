using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TeacherBT : Tree {
    public Transform[] firstMovementWaypoints;
    public Transform[] secondMovementWaypoints;
    public NavMeshAgent agent;
    public static int exercisesGoal = 2;
    public static float speed = 2f;

    protected override Node SetupTree() {
        Node root = new Selector(new List<Node> {
            new CheckSecondMovement(),
            new Sequence(new List<Node> {
                new CheckFirstMovement(),
                new TaskSecondMovement(agent, transform, secondMovementWaypoints),
            }),
            new TaskFirstMovement(agent, transform, firstMovementWaypoints),
        });

        return root;
    }
}
