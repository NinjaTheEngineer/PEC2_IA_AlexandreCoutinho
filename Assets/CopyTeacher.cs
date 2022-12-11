using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CopyTeacher : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject target;
    public Vector3 pos;
    public Quaternion rot;

    Vector3 offset;

    void Start() {
        agent = gameObject.GetComponent<NavMeshAgent>();
        //transform.rotation = target.transform.rotation;
        //transform.position = target.transform.TransformPoint(pos);
        offset = target.transform.position - transform.position;
        agent.destination = target.transform.position - offset;
    }

    void Update()
    {
        agent.destination = target.transform.position - offset;
        //agent.destination = target.transform.TransformPoint(transform.position);
    }
}