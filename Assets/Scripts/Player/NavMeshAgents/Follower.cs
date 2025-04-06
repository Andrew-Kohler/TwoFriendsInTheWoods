using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Follower : MonoBehaviour
{
    public Transform goal;
    NavMeshAgent agent;
    public float StopDistance = .1f; // How far away we need to be before we don't need to track anymore
    private Rigidbody _rb;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        _rb = GetComponent<Rigidbody>();    
    }

    private void Update()
    {
        // Debug.Log(agent.pathStatus);
        agent.destination = goal.position;
        if (agent.pathStatus != NavMeshPathStatus.PathPartial)
        {
            
            // Debug.Log(Mathf.Abs(Vector3.Distance(goal.position, this.transform.position)));
            if (Mathf.Abs(Vector3.Distance(goal.position, this.transform.position)) < StopDistance)
                _rb.useGravity = false;
            else
                _rb.useGravity = true;
        }
        
    }

    private void FixedUpdate()
    {
        if (agent.pathStatus != NavMeshPathStatus.PathPartial)
        {
            if (Mathf.Abs(Vector3.Distance(goal.position, this.transform.position)) > StopDistance)
            {
                _rb.constraints = RigidbodyConstraints.FreezeRotation;
                this.transform.position = agent.nextPosition;
            }
            else
            {
                _rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
            }
        }
        else
        {
            _rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        }



        // _rb.velocity = agent.desiredVelocity;
        //else
        //    _rb.velocity = Vector3.zero;
    }
}
