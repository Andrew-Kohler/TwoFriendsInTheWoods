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
    private Rigidbody rb;

    // Control variables
    public float MovementSpeed = 7f;
    public float JumpForce = 70f;
    public float groundThreshold = .1f;

    public float HorizontalInput;
    public float VerticalInput;

    private Vector3 velocity;
    public bool Grounded = false;
    public bool RaycastGrounded = false;
    private bool _jumping = false;

    private bool _isInteractionMoving;
    private Vector3 _toMoveTowards;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Raycast grounded check
        if(Physics.Raycast(rb.position, Vector3.down, out RaycastHit hit) && hit.distance < groundThreshold)
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

        velocity = new Vector3(HorizontalInput * MovementSpeed, rb.velocity.y, VerticalInput * MovementSpeed);
    }

    private void FixedUpdate()
    {
        MovePlayer3D();
    }

    private void MovePlayer3D()
    {
        if (GameManager.Instance._currentGameState == GameManager.GameState.Gameplay)
        {
            rb.velocity = velocity;
            if (_jumping)
            {
                rb.AddForce(new Vector3(0, JumpForce, 0), ForceMode.Impulse);
                _jumping = false;
            }
        }

        if (_isInteractionMoving)
        {
            if(Vector3.Distance(transform.position, _toMoveTowards) > .1f)
                transform.position = Vector3.MoveTowards(transform.position, _toMoveTowards, MovementSpeed * Time.deltaTime);
        }
            
    }

    public void MoveToPoint(bool active, Vector3 point)
    {
        _toMoveTowards = point;
        _isInteractionMoving = active;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 6) // Level geometry
        {
            Grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 6) 
        {
            Grounded = false;
        }
    }

}
