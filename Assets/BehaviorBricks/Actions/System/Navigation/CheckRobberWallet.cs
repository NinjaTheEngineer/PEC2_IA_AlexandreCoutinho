using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{
    /// <summary>
    /// It is an action to move towards the given goal using a NavMeshAgent.
    /// </summary>
    [Action("Navigation/CheckRobberWallet")]
    [Help("Moves the game object towards a given target by using a NavMeshAgent")]
    public class CheckRobberWallet : GOAction
    {
        ///<value>Input target game object towards this game object will be moved Parameter.</value>
        [InParam("robber")]
        [Help("Target game object towards this game object will be moved")]
        public GameObject robber;
        ///<value>Input target game object towards this game object will be moved Parameter.</value>
        [InParam("robberWallet")]
        [Help("Target game object towards this game object will be moved")]
        public GameObject robberWallet;
        [OutParam("hasWallet")]
        [Help("output variable")]
        public bool hasWallet;

        /// <summary>Initialization Method of MoveToGameObject.</summary>
        /// <remarks>Check if GameObject object exists and NavMeshAgent, if there is no NavMeshAgent, the default one is added.</remarks>
        public override void OnStart()
        {
            if (robber == null)
            {
                Debug.LogError("The movement target of this game object is null", gameObject);
                return;
            }
            hasWallet = robberWallet!=null && robberWallet.activeSelf;
            if(hasWallet) Debug.LogWarning("Has wallet! Should follow");
            else Debug.Log("Bla");
        }

        /// <summary>Method of Update of MoveToGameObject.</summary>
        /// <remarks>Verify the status of the task, if there is no objective fails, if it has traveled the road or is near the goal it is completed
        /// y, the task is running, if it is still moving to the target.</remarks>
        public override TaskStatus OnUpdate()
        {
            if(hasWallet)
                return TaskStatus.FAILED;
            return TaskStatus.COMPLETED;
        }
        /// <summary>Abort method of MoveToGameObject </summary>
        /// <remarks>When the task is aborted, it stops the navAgentMesh.</remarks>
    }
}
