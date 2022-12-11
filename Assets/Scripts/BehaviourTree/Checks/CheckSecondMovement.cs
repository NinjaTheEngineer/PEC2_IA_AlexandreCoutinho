using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSecondMovement : Node {

    public override NodeState Evaluate() {
        object t = GetData("secondMovementDoneCount");
        state = NodeState.FAILURE;
        if(t==null) {
            parent.SetData("secondMovementDoneCount", 0);
            return state;
        }
        int secondMovementDoneCount = (int) t;

        if(TeacherBT.exercisesGoal <= secondMovementDoneCount) {
            state = NodeState.SUCCESS;
            parent.SetData("secondMovementDoneCount", 0);
            parent.SetData("firstMovementDoneCount", 0);
            parent.SetData("firstMoveDone", false);
            return state;
        }

        return state;
    }
}
