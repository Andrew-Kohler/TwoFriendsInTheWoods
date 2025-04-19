using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class P2Movement : PlayerMovement
{
    [Header("P1 Values")]
    [SerializeField] private Rigidbody _player1RB;
    [SerializeField] private Transform _player1TF;
    [SerializeField] private Follower _player1Follower;
    [SerializeField] private NavMeshAgent _player1Agent;

    [Header("P2 Values")]
    [SerializeField] private Transform _holdTF;
    [SerializeField] private PlayerMovement _player2Movement;
    private FollowPointMover _fpMover;
    [SerializeField] private float _holdSpeed;


    [SerializeField] private float _holdResetTime = 2f;

    private GameObject _mainCam;
    [SerializeField] private GameObject _angleCam;

    private float _holdResetTimer;

    public bool Holding;
    new void Start()
    {
        base.Start();
        _fpMover = GetComponent<FollowPointMover>();
        _mainCam = GameObject.FindWithTag("Cam_V-Main");
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        float currMoveSpeed = MovementSpeed;

        if(_holdResetTimer > 0)
        {
            _holdResetTimer -= Time.deltaTime;
        }

        if (Input.GetButton("Ability") && Vector3.Distance(this.transform.position, _player1TF.position) < 4f && _holdResetTimer <= 0)
        {
            currMoveSpeed = _holdSpeed;
            Holding = true;

            _player1Follower.Navigate = false;
            //_player1Agent.isStopped = true;

            _player1TF.position = _holdTF.position;
            _player1RB.velocity = Vector3.zero;
            _angleCam.SetActive(true);
            _mainCam.SetActive(false);

        }
        else if(!Input.GetButton("Ability") && Holding)
        {
            _player1RB.constraints = RigidbodyConstraints.FreezeRotation;
            ThrowInDirection();
            Holding = false;
            _holdResetTimer = _holdResetTime;
            _mainCam.SetActive(true);
            _angleCam.SetActive(false);
        }

        _velocity = new Vector3(HorizontalInput * currMoveSpeed, _rb.velocity.y, VerticalInput * currMoveSpeed);
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void ThrowInDirection()
    {
        PlayerMovement.Direction _dir = _player2Movement.GetDirection();
        float throwStrength = 7f;
        float throwHeight = 15f;

        switch (_dir)
        {
            case PlayerMovement.Direction.Forwards:
                _player1RB.AddForce(new Vector3(0f, throwHeight, -throwStrength), ForceMode.Impulse);
                break;
            case PlayerMovement.Direction.ForwardsLeft:
                _player1RB.AddForce(new Vector3(-throwStrength, throwHeight, -throwStrength), ForceMode.Impulse);
                break;
            case PlayerMovement.Direction.ForwardsRight:
                _player1RB.AddForce(new Vector3(throwStrength, throwHeight, -throwStrength), ForceMode.Impulse);
                break;
            case PlayerMovement.Direction.Left:
                _player1RB.AddForce(new Vector3(-throwStrength, throwHeight, 0f), ForceMode.Impulse);
                break;
            case PlayerMovement.Direction.Right:
                _player1RB.AddForce(new Vector3(throwStrength, throwHeight, 0f), ForceMode.Impulse);
                break;
            case PlayerMovement.Direction.BackwardsLeft:
                _player1RB.AddForce(new Vector3(-throwStrength, throwHeight, throwStrength), ForceMode.Impulse);
                break;
            case PlayerMovement.Direction.BackwardsRight:
                _player1RB.AddForce(new Vector3(throwStrength, throwHeight, throwStrength), ForceMode.Impulse);
                break;
            case PlayerMovement.Direction.Backwards:
                _player1RB.AddForce(new Vector3(0f, throwHeight, throwStrength), ForceMode.Impulse);
                break;
            default:
                _player1RB.AddForce(new Vector3(0f, 15f, 0f), ForceMode.Impulse);
                break;
        }
    }

    // Allows P2 to wake up P1 with a tap
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("P1_Wake"))
        {
            _player1Agent.nextPosition = _player1Agent.transform.position; // Stop any snapping before bringing P1 back to life
            _player1Follower.Navigate = true;   
        }
    }
}
