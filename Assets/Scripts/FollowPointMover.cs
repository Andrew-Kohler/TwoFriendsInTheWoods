using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPointMover : MonoBehaviour
{
    [SerializeField] private Transform _followPoint; // The point the follower tracks to
    [SerializeField] private float _followDistX;     // How far away the point should be (x)
    [SerializeField] private float _followDistZ;     // How far away the point should be (x)
    private float _xVal;
    [SerializeField] private float _yVal;
    private float _zVal;

    public float HorizontalInput;
    public float VerticalInput;

    // Essentially, horizontal and vertical inputs will influence this thing to be at 1 of 8 points
    void Start()
    {
        _xVal = this.transform.position.x;
        _zVal = this.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance._currentGameState == GameManager.GameState.Gameplay)
        {
            HorizontalInput = Input.GetAxis("Horizontal");
            VerticalInput = Input.GetAxis("Vertical");

            // Checks for 0 input only occur as long as at least some movement is happening
            // This ensures the follow point will never snap back inside the player
            if (HorizontalInput < 0)
            {
                _xVal = this.transform.position.x + _followDistX;
                if (VerticalInput == 0)
                    _zVal = this.transform.position.z;
            }
                
            else if (HorizontalInput > 0)
            {
                _xVal = this.transform.position.x - _followDistX;
                if (VerticalInput == 0)
                    _zVal = this.transform.position.z;
            }
                
            

            if (VerticalInput < 0)
            {
                _zVal = this.transform.position.z + _followDistZ;
                if (HorizontalInput == 0)
                    _xVal = this.transform.position.x;
            }
            else if (VerticalInput > 0)
            {
                _zVal = this.transform.position.z - _followDistZ;
                if (HorizontalInput == 0)
                    _xVal = this.transform.position.x;
            }

            _followPoint.position = new Vector3(_xVal, _followPoint.position.y, _zVal);
            
        }
    }
}
