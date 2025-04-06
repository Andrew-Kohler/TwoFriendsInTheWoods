using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2Movement : PlayerMovement
{
    [Header("P1 Values")]
    [SerializeField] private Rigidbody _player1RB;
    [SerializeField] private Transform _player1TF;
    [SerializeField] private Follower _player1Follower;

    [Header("P2 Values")]
    [SerializeField] private Transform _holdTF;
    private FollowPointMover _fpMover;
    [SerializeField] private float _holdSpeed;


    [SerializeField] private float _holdResetTime = 2f;

    private GameObject _mainCam;
    [SerializeField] private GameObject _angleCam;

    private float _holdResetTimer;

    private bool _holding;
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
            _holding = true;
            _player1Follower.enabled = false;
            _player1TF.position = _holdTF.position;
            _player1RB.velocity = Vector3.zero;
            _angleCam.SetActive(true);
            _mainCam.SetActive(false);

        }
        else if(!Input.GetButton("Ability") && _holding)
        {
            _player1RB.constraints = RigidbodyConstraints.FreezeRotation;
            _player1RB.AddForce(new Vector3(0f, 15f, 7f), ForceMode.Impulse);
            _holding = false;
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
}
