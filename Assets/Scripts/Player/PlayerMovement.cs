/*
Player Movement
Used on:    Player
For:    Allows for keyboard control of player while exploring
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    protected Rigidbody _rb;

    // Control variables
    public float MovementSpeed = 7f;
    public float JumpForce = 70f;
    public float groundThreshold = .1f;

    public float HorizontalInput;
    public float VerticalInput;

    protected Vector3 _velocity;
    public bool Grounded = false;
    public bool RaycastGrounded = false;
    protected bool _jumping = false;

    protected bool _isInteractionMoving;
    private Vector3 _jumpDir = new Vector3(0, 1, 0);

    // Animation variables
    public enum Direction { Forwards, ForwardsLeft, ForwardsRight, Left, Right, Backwards, BackwardsLeft, BackwardsRight, Null };
    public Direction _currentDir;

    public enum Action { Walk, Idle, JumpUp, JumpDown };
    public Action _currentAction;

    protected void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    protected void Update()
    {
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        // Raycast grounded check
        if (Physics.Raycast(_rb.position, Vector3.down, out RaycastHit hit) && hit.distance < groundThreshold)
        {
            RaycastGrounded = true;
        }
        else
        {
            RaycastGrounded = false;
        }

        // Player control is only given during gameplay
        if (GameManager.Instance._currentGameState == GameManager.GameState.Gameplay)
        {
            HorizontalInput = Input.GetAxis("Horizontal");
            VerticalInput = Input.GetAxis("Vertical");

            if (Grounded && RaycastGrounded && Input.GetButtonDown("Jump"))
                _jumping = true;
        }
        else
        {
            HorizontalInput = 0;
            VerticalInput = 0;
        }
        _velocity = new Vector3(HorizontalInput * MovementSpeed, _rb.velocity.y, VerticalInput * MovementSpeed);
    }

    protected void FixedUpdate()
    {
        MovePlayer3D();
        
    }

    protected void MovePlayer3D()
    {
        if (GameManager.Instance._currentGameState == GameManager.GameState.Gameplay)
        {
            _rb.velocity = _velocity;

            Vector3 jump = _jumpDir;
            //Debug.Log("Initial jump: " + jump);
            if (Physics.Raycast(_rb.position, Vector3.down, out RaycastHit hit) && hit.distance < groundThreshold)
            {
                //Debug.Log("Normal vector: " + hit.normal);
                jump = Vector3.ProjectOnPlane(jump, hit.normal);
            }
            //Debug.Log("After normalization jump: " + jump);
            jump *= JumpForce;
            //Debug.Log("After mult jump: " + jump);
            Vector3 finalJump = new Vector3(jump.x, JumpForce, jump.z);

            if (_jumping)
            {
                _rb.AddForce(finalJump, ForceMode.Impulse);
                _jumping = false;
            }
        }
        else if(GameManager.Instance._currentGameState == GameManager.GameState.Load)
        {
            _rb.velocity = new Vector3(0, _velocity.y, 0);
        }
            
    }

    public Direction GetDirection() // Returns the direction the player is facing
    {
        float threshold = .01f;
        Vector2 dir = Vector2.zero;
        _rb = GetComponent<Rigidbody>();
        dir.x = _rb.velocity.x;
        dir.y = _rb.velocity.z;

        if (Mathf.Abs(dir.x) < threshold)
            dir.x = 0;
        if (Mathf.Abs(dir.y) < threshold)
            dir.y = 0;

        if(HorizontalInput != 0 || VerticalInput != 0) // Make sure input is the cause of change and not minute movements
        {
            if (dir.x > 0) // Positive X motion (right)
            {
                if (dir.y > 0) // Positive Z motion (away)
                {
                    _currentDir = Direction.BackwardsRight; // AA
                    return _currentDir;
                }
                else if (dir.y < 0) // Negative Z motion (towards)
                {
                    _currentDir = Direction.ForwardsRight;
                }
                else // No Z motion (just right)
                {
                    _currentDir = Direction.Right;
                }
            }
            else if (dir.x < 0) // Negative X motion (left)
            {
                if (dir.y > 0) // Positive Z motion (away)
                {
                    _currentDir = Direction.BackwardsLeft;
                    return _currentDir;
                }
                else if (dir.y < 0) // Negative Z motion (towards)
                {
                    _currentDir = Direction.ForwardsLeft; // AAA
                }
                else // No Z motion (just right)
                {
                    _currentDir = Direction.Left;
                }
            }
            else if (dir.y > 0) // Positive Z motion (away)
            {
                _currentDir = Direction.Backwards;
            }
            else if (dir.y < 0) // Negative Z motion (towards)
            {
                _currentDir = Direction.Forwards;
            }
        }
        

        return _currentDir;

    }

    public Action GetAction() // Returns the general action the player is undertaking
    {
        if(HorizontalInput != 0 || VerticalInput != 0) // Determining whether we are walking or idling
        {
            _currentAction = Action.Walk;
        }
        else
        {
            _currentAction = Action.Idle;
        }

        if (!Grounded) // Jumping overrides any previous state determination
        {
            if (_rb.velocity.y > 0)
                _currentAction = Action.JumpUp;
            else
                _currentAction = Action.JumpDown;
        }

        return _currentAction;
    }

    #region COLLISION STAY AND EXIT
    protected void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 6) // Level geometry
        {
            Grounded = true;
        }
    }

    protected void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 6) 
        {
            Grounded = false;
        }
    }
    #endregion

}
