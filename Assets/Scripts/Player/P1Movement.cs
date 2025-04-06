using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1Movement : PlayerMovement
{
    [SerializeField] private GameObject _normalCol;
    [SerializeField] private GameObject _crouchCol;
    [SerializeField] private float _crouchSpeed;

    new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        base.Update();

        float currMoveSpeed = MovementSpeed;
        // Special ability check
        if (Input.GetButton("Ability"))
        {
            currMoveSpeed = _crouchSpeed;
            _normalCol.SetActive(false);
            _crouchCol.SetActive(true);
            
        }
        else
        {
            _normalCol.SetActive(true);
            _crouchCol.SetActive(false);
        }

        _velocity = new Vector3(HorizontalInput * currMoveSpeed, _rb.velocity.y, VerticalInput * currMoveSpeed);
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();
    }

}
