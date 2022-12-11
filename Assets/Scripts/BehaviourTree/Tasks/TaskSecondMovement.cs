using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TaskSecondMovement : Node {
    private Transform transform;
    private Transform[] waypoints;
    private Animator animator;

    private int currentWaypoinyIndex = 0;

    private float waitTime = 1f;
    private float waitCounter = 0f;
    private bool waiting = false;
    private NavMeshAgent agent;
    public TaskSecondMovement(NavMeshAgent agent, Transform transform, Transform[] waypoints) {
        this.agent = agent;
        this.transform = transform;
        this.waypoints = waypoints;
        this.animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate() {
        object b = GetData("firstMoveDone");
        if(b!=null) {
            bool firstMoveDone = (bool) b;
            if(firstMoveDone){
                if(waiting) {
                    waitCounter += Time.deltaTime;
                    if(waitCounter >= waitTime) {
                        waiting = false;
                        //animator.SetBool("walking", true);
                    }
                } else {
                    object t = GetData("secondMovementDoneCount");
                    if(t==null) {
                        parent.parent.SetData("secondMovementDoneCount", 0);
                        state = NodeState.FAILURE;
                        return state;
                    }
                    int secondMovementDoneCount = t!=null?(int)t:0;
                    Transform wp = waypoints[currentWaypoinyIndex];
                    Vector3 waypointPos = new Vector3(wp.position.x, transform.position.y, wp.position.z);
                    if(Vector3.Distance(transform.position, waypointPos) < 0.1f) {
                        waitCounter = 0f;
                        waiting = true;

                        currentWaypoinyIndex = (currentWaypoinyIndex + 1) % waypoints.Length;
                        if(currentWaypoinyIndex==0){
                            parent.parent.SetData("secondMovementDoneCount", ++secondMovementDoneCount);
                        }
                        //animator.SetBool("walking", false);
                    } else {
                        agent.SetDestination(waypointPos);
                        //transform.position = Vector3.MoveTowards(transform.position, waypointPos, TeacherBT.speed * Time.deltaTime);
                        //transform.LookAt(waypointPos);
                     }
                }
            }
        }
        
        state = NodeState.RUNNING;
        return state;
    }

}
