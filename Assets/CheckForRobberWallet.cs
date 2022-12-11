using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForRobberWallet : MonoBehaviour
{
    public BehaviorExecutor CopWander;
    public BehaviorExecutor CopFollowRobber;
    public GameObject robber;
    public GameObject robberWallet;

    public bool hasWallet = false;
    public bool behaviourSet = false;
    private void Start() {
        CopFollowRobber.enabled = false;
        CopFollowRobber.paused = true;
        CopWander.enabled = true;
        CopWander.paused = false;
    }
    private void Update() {
        
        hasWallet = robber!=null && robberWallet.activeSelf;    
        if(hasWallet && !behaviourSet) {
            behaviourSet=true;
            CopWander.enabled = false;
            CopWander.paused = true;
            CopFollowRobber.enabled = true;
            CopFollowRobber.paused = false;
        }
    }
}
