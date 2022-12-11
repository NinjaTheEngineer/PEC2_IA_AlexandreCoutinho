using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFirstMovement : Node {

    public override NodeState Evaluate() {
        object t = GetData("firstMovementDoneCount");
        state = NodeState.FAILURE;
        if(t==null) {
            parent.parent.SetData("firstMovementDoneCount", 0);
            
            return state;
        }
        int firstMovementDoneCount = (int) t;

        if(TeacherBT.exercisesGoal <= firstMovementDoneCount) {
            state = NodeState.SUCCESS;
            parent.parent.SetData("firstMoveDone", true);
            return state;
        }

        return state;
    }
}
