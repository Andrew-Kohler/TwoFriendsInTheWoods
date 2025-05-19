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
    public float groundThreshold = .1f;
    public bool RaycastGrounded = false;
    public bool Navigate = true;

    private PlayerMovement.Direction _currentDir;
    private PlayerMovement.Action _currentAction;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        _rb = GetComponent<Rigidbody>();    
    }

    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.nextPosition = this.transform.position;
        agent.Warp(this.gameObject.transform.position);
        Navigate = true;
    }

    private void Update()
    {
        // Debug.Log(agent.pathStatus);
        if (GameManager.Instance._currentGameState == GameManager.GameState.Load){
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }

        agent.destination = goal.position;
        if (agent.pathStatus != NavMeshPathStatus.PathPartial && Navigate)
        {
            GameManager.CanLoadAgent = true;
            // Debug.Log(Mathf.Abs(Vector3.Distance(goal.position, this.transform.position)));
            if (Mathf.Abs(Vector3.Distance(goal.position, this.transform.position)) < StopDistance)
                _rb.useGravity = false;
            else
                _rb.useGravity = true;
        }
        else
        {
            GameManager.CanLoadAgent = false;
        }

        // Grounded check here exists purely for animation purposes
        if (Physics.Raycast(_rb.position, Vector3.down, out RaycastHit hit) && hit.distance < groundThreshold)
        {
            RaycastGrounded = true;
        }
        else
        {
            RaycastGrounded = false;
        }


    }

    private void FixedUpdate()
    {
        if (agent.pathStatus != NavMeshPathStatus.PathPartial)
        {
            if (Mathf.Abs(Vector3.Distance(goal.position, this.transform.position)) > StopDistance)
            {
                _rb.constraints = RigidbodyConstraints.FreezeRotation;
                if(Navigate)
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

    public PlayerMovement.Direction GetDirection() // Returns the direction the player is facing
    {
        float significanceThreshold = .01f; // Any value below this is essentially 0
        /*Vector2 dir = Vector2.zero;
        dir.x = _rb.velocity.x;
        dir.y = _rb.velocity.z;

        if (Mathf.Abs(dir.x) < threshold)
            dir.x = 0;
        if (Mathf.Abs(dir.y) < threshold)
            dir.y = 0;*/

        if (Mathf.Abs(Vector3.Distance(goal.position, this.transform.position)) > StopDistance) // Only update direction in motion
        {
            float xDist = goal.position.x - this.transform.position.x;
            float zDist = goal.position.z - this.transform.position.z;

            if (Mathf.Abs(xDist) < significanceThreshold)
                xDist = 0;
            if (Mathf.Abs(zDist) < significanceThreshold)
                zDist = 0;

            if (xDist > 0) // Goal X greater than Follower X (go right)
            {
                if (zDist > 0) // Goal Z greater than Follower Z (go away)
                {
                    _currentDir = PlayerMovement.Direction.BackwardsRight;
                }
                else if (zDist < 0) // Goal Z less than Follower Z (go towards)
                {
                    _currentDir = PlayerMovement.Direction.ForwardsRight;
                }
                else // No Z motion (just right)
                {
                    _currentDir = PlayerMovement.Direction.Right;
                }
            }
            else if (xDist < 0) // Goal X less than Follower X (go left)
            {
                if (zDist > 0) // Goal Z greater than Follower Z (go away)
                {
                    _currentDir = PlayerMovement.Direction.BackwardsLeft;
                }
                else if (zDist < 0) // Goal Z less than Follower Z (go towards)
                {
                    _currentDir = PlayerMovement.Direction.ForwardsLeft;
                }
                else // No Z motion (just left)
                {
                    _currentDir = PlayerMovement.Direction.Left;
                }
            }
            else if (zDist > 0) // Goal Z greater than Follower Z (go away)
            {
                _currentDir = PlayerMovement.Direction.Backwards;
            }
            else if (zDist < 0) // Goal Z less than Follower Z (go towards)
            {
                _currentDir = PlayerMovement.Direction.Forwards;
            }
        }

        return _currentDir;

    }

    public PlayerMovement.Action GetAction() // Returns the general action the player is undertaking
    {
        //float threshold = .001f;
        /* Vector2 dir = Vector2.zero;
         dir.x = _rb.velocity.x;
         dir.y = _rb.velocity.z;

         if (Mathf.Abs(dir.x) < threshold)
             dir.x = 0;
         if (Mathf.Abs(dir.y) < threshold)
             dir.y = 0;*/

        // Using positions on the same plane so that the agent doesn't try to walk just because you're jumping
        Vector3 planarPos = new Vector3(this.transform.position.x, 0, this.transform.position.z);
        Vector3 planarGoal = new Vector3(goal.position.x, 0, goal.position.z);

        if (Mathf.Abs(Vector3.Distance(planarGoal, planarPos)) > StopDistance && Navigate) // Determining whether we are walking or idling
        {
            _currentAction = PlayerMovement.Action.Walk;
        }
        else
        {
            _currentAction = PlayerMovement.Action.Idle;
        }

        if (!RaycastGrounded) // Jumping overrides any previous state determination
        {
            _rb = GetComponent<Rigidbody>();
            if (_rb.velocity.y > 0)
                _currentAction = PlayerMovement.Action.JumpUp;
            else
                _currentAction = PlayerMovement.Action.JumpDown;
        }

        return _currentAction;
    }

    public Transform GetGoal() // Gets the current goal point of the follower
    {
        return goal;
    }

    public void SetGoal(Transform newGoal)
    {
        goal = newGoal;
    }

    public void UpdateCurrentPosition()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.nextPosition = this.transform.position;
        agent.Warp(this.gameObject.transform.position);
    }
}
