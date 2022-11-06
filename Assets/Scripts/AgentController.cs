using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class AgentController : MonoBehaviour {
    #region Variables
    public float GoalDistance = 2f;
    public bool RightClickToMove = true;
    public float MaxSpeed = 3f;
    public float MinSpeed = 2f;
    [Tooltip("If default speed is 0 the agent speed will be set randomly between the max and min speed values. Otherwise the speed will be set to the default speed.")]
    [Range(0, 10)]
    [SerializeField]private float _defaultSpeed = 0f;
    public bool HasFollowers = false;
    bool isMouseTargetLocation = false;
    public float DefaultSpeed {
        get => _defaultSpeed;
        private set {
            if(_defaultSpeed != value) {
                Debug.Log("Changed default speed from " + _defaultSpeed + " to " + value);
                _defaultSpeed = value;
            }
        }
    }
    private bool _active = true;
    public bool Active {
        get => _active;
        private set {
            if(_active != value){
                _active = value;
            }
        }
    }
    private float remaningDistance => Active?agent.remainingDistance:0;
    public bool ReachedTarget => remaningDistance <= GoalDistance;
    List<AgentSeekerController> agentsFollowing = new List<AgentSeekerController>();
    NavMeshAgent agent;
    Vector3 currentLocation;
    Vector3 _targetPosition;
    private float _currentSpeed;
    public float CurrentSpeed {
        get {
            if(!agent && _currentSpeed!=agent.speed) {
                _currentSpeed = agent.speed;
            }
            return _currentSpeed;
        }
        private set {}
    }
    public Vector3 TargetPosition {
        get => _targetPosition;
        private set {
            if(value==null || value==_targetPosition) {
                return;
            }
            _targetPosition = value;
            agent.destination = _targetPosition;
        } 
    }
    #endregion 
    public virtual void Awake() {
        if(!agent){
            agent = GetComponent<NavMeshAgent>();
        }
        DefaultSpeed = DefaultSpeed==0?Random.Range(MinSpeed,MaxSpeed):DefaultSpeed;
        agent.speed = DefaultSpeed;
    } 
    public virtual void Update() {
        if(!Active){
            return;
        }
        HandleMouseClick();
        if(isMouseTargetLocation && !ReachedTarget){
            return;
        }
        isMouseTargetLocation = false;
        if(HasFollowers){    
            agent.speed = AgentCloseBy ? DefaultSpeed : 1;
        }
    }
    private void HandleMouseClick() {
        if (RightClickToMove && Input.GetMouseButtonDown(1)) {
            RaycastHit hit;
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(camRay, out hit, 100)) {
                SetTargetPosition(hit.point);
                isMouseTargetLocation = true;
            }
        }
    }
    public void SetTargetPosition(Vector3 target) {
        string logId = "SetAgentTargetPosition::";
        if(target==null) {
            System.Console.WriteLine(logId + "Target is null");
        }
        TargetPosition = target;
    }
    public void SetAgentSpeed(float speed) {
        if(!agent) {
            Debug.Log("SetAgentSpeed:: agent is null => no-op");
            return;
        }
        if(!agent.isActiveAndEnabled) {
            Debug.Log("SetAgentSpeed:: agent not active or enabled => no-op");
            return;
        }
        if(CurrentSpeed == speed){
            Debug.Log("SetAgentSpeed:: speed="+CurrentSpeed+" is equal to new speed=" + speed+", returning");
            return;
        }
        agent.speed = speed;
    }
    public void AddFollowingAgent(AgentSeekerController agent) {
        if(!agent) {
            Debug.Log("Agent is null => returning");
            return;
        }
        if(!agentsFollowing.Contains(agent)) {
            agentsFollowing.Add(agent);
        }
    }
    public bool AgentCloseBy{
        get{
            int agentsFollowingCount = agentsFollowing.Count;
            if(agentsFollowingCount==0){
                return false;
            }  
            bool isAgentClose = false;
            for (int i = 0; i < agentsFollowingCount; i++) {
                if(agentsFollowing[i].AgentNearby){
                    isAgentClose = true;
                }
            }
            return isAgentClose;
        }
    }
    public void ActivateAgent() {
        if(!agent){
            Debug.Log("Agent is null => no-op");
            
            return;
        }
        Active = true;
        agent.enabled = true;
    } 
    public void DeactivateAgent() {
        if(!agent){
            Debug.Log("Agent is null => no-op");
            return;
        }
        Active = false;
        agent.enabled = false;
    } 
}
