using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPointMover : MonoBehaviour
{
    [SerializeField] private Transform _followPoint; // The point the follower tracks to
    [SerializeField] private float _followDistX;     // How far away the point should be (x)
    [SerializeField] private float _followDistZ;     // How far away the point should be (x)
    private float _xVal;
    private float _zVal;

    [Header("Direction Determining")]
    [SerializeField, Tooltip("Player movement script")] private PlayerMovement _pMovement;
    [SerializeField, Tooltip("Player follower AI script")] private Follower _pFollower;

    private PlayerMovement.Direction _currentDir;

    public float HorizontalInput;
    public float VerticalInput;

    // Essentially, horizontal and vertical inputs will influence this thing to be at 1 of 8 points
    void Start()
    {
        _xVal = this.transform.position.x;
        _zVal = this.transform.position.z;
        if (_pMovement.enabled)
        {
            _currentDir = _pMovement.GetDirection();
        }
        else
        {
            _currentDir = _pFollower.GetDirection();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance._currentGameState == GameManager.GameState.Gameplay)
        {
            HorizontalInput = Input.GetAxis("Horizontal");
            VerticalInput = Input.GetAxis("Vertical");

            // Get our animation data from the correct source
            if (_pMovement.enabled)
            {
                _currentDir = _pMovement.GetDirection();
            }
            else
            {
                _currentDir = _pFollower.GetDirection();
            }

            /*if(HorizontalInput != 0 || VerticalInput != 0) // Make sure we're moving
            {*/
                switch (_currentDir)
                {
                    case PlayerMovement.Direction.Forwards:
                        _xVal = this.transform.position.x;
                        _zVal = this.transform.position.z + _followDistZ;
                        break;
                    case PlayerMovement.Direction.ForwardsLeft:
                        _xVal = this.transform.position.x + _followDistX;
                        _zVal = this.transform.position.z + _followDistZ;
                        break;
                    case PlayerMovement.Direction.ForwardsRight:
                        _xVal = this.transform.position.x - _followDistX;
                        _zVal = this.transform.position.z + _followDistZ;
                        break;
                    case PlayerMovement.Direction.Left:
                        _xVal = this.transform.position.x + _followDistX;
                        _zVal = this.transform.position.z;
                        break;
                    case PlayerMovement.Direction.Right:
                        _xVal = this.transform.position.x - _followDistX;
                        _zVal = this.transform.position.z;
                        break;
                    case PlayerMovement.Direction.BackwardsLeft:
                        _xVal = this.transform.position.x + _followDistX;
                        _zVal = this.transform.position.z - _followDistZ;
                        break;
                    case PlayerMovement.Direction.BackwardsRight:
                        _xVal = this.transform.position.x - _followDistX;
                        _zVal = this.transform.position.z - _followDistZ;
                        break;
                    case PlayerMovement.Direction.Backwards:
                        _xVal = this.transform.position.x;
                        _zVal = this.transform.position.z - _followDistZ;
                        break;
                    default:
                        _xVal = this.transform.position.x;
                        _zVal = this.transform.position.z;
                        break;
                }
           // }
            /*// Checks for 0 input only occur as long as at least some movement is happening
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
            }*/

            _followPoint.position = new Vector3(_xVal, _followPoint.position.y, _zVal);
            
        }
    }
}
