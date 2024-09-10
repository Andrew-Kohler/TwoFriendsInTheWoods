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
    public float movementSpeed = 7f;
    public float jumpForce = 70f;
    public float groundThreshold = .1f;

    public float horizontalInput;
    public float verticalInput;

    private Vector3 velocity;
    public bool grounded = false;
    public bool raycastGrounded = false;
    private bool jumping = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // x
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if(Physics.Raycast(rb.position, Vector3.down, out RaycastHit hit) && hit.distance < groundThreshold)
        {
            raycastGrounded = true;
        }
        else
        {
            raycastGrounded = false;
        }

        if (grounded && raycastGrounded && Input.GetButtonDown("Jump"))
            jumping = true;

        velocity = new Vector3(horizontalInput * movementSpeed, rb.velocity.y, verticalInput * movementSpeed);
    }

    private void FixedUpdate()
    {
        MovePlayer3D();
    }

    private void MovePlayer3D()
    {
        rb.velocity = velocity;
        if (jumping)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            jumping = false;
        }
            
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 6) // Level geometry
        {
            grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 6) 
        {
            grounded = false;
        }
    }

}
