using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatior : MonoBehaviour
{
    
    public enum AnimationAxis { Rows, Columns }

    private MeshRenderer meshRenderer;
    private Rigidbody rb;
    private AudioSource audioSource;
    [SerializeField] private string rowProperty = "_CurrRow", colProperty = "_CurrCol";

    [Header("Spritesheet Reader Values")]
    [SerializeField] private AnimationAxis axis;
    [SerializeField] private float animationSpeed = 5.4f;
    [SerializeField] private int _aIndex = 0;
    [Header("Player Related Values")]
    [SerializeField, Tooltip("Player movement script")] private PlayerMovement _pMovement;
    [SerializeField, Tooltip("Player follower AI script")] private Follower _pFollower;
    [SerializeField] private List<AudioClip> playerSounds;

    private bool faceAwayBool;
    private bool faceDirBool;

    public int walkType = 0;    // 0 = stone, 1 = earth, 2 = wood, 3 = water
    private bool footBool1 = false;
    private bool footBool2 = false;

    private float deltaT;
    private float _horizontalInput;
    private float _verticalInput;

    private int _frame;
    private int _frameLoop = 0;  // A value to hold the number of the frame that the current animation loops on (e.g. after frame 13, loop it)
    private int _frameReset = 0; // A value to hold the number of the frame that the current animation loops back to (e.g. the loop starts on frame 0)
    public bool activeCoroutine = false;    // The classic boolean to use when Update() needs to be quiet during a coroutine

    public bool standStill = false;         // A bool to make the player quit their moving when something important is happening

    private PlayerMovement.Direction _interactionDir = PlayerMovement.Direction.Null;

    #region ANIM INDICIES
    private int _IdleIndex = 16;

    // Forwards = towards camera
    private int _WalkForwardsIndex = 1;
    private int _WalkForwardsLeftIndex = 8;
    private int _WalkForwardsRightIndex = 9;

    private int _IdleForwardsIndex = 5;
    private int _IdleForwardsLeftIndex = 6;
    private int _IdleForwardsRightIndex = 4;

    private int _JumpForwardsIndex = 2;
    private int _JumpForwardsLeftIndex = 10;
    private int _JumpForwardsRightIndex = 11;

    // Left and right
    private int _WalkLIndex = 12;
    private int _WalkRIndex = 13;

    private int _IdleLIndex = 7;
    private int _IdleRIndex = 3;

    private int _JumpLIndex = 14;
    private int _JumpRIndex = 15;

    // Backwards = away from camera
    private int _WalkBackwardsIndex = 0;
    private int _WalkBackwardsLeftIndex = 4;
    private int _WalkBackwardsRightIndex = 5;

    private int _IdleBackwardsIndex = 1;
    private int _IdleBackwardsLeftIndex = 0;
    private int _IdleBackwardsRightIndex = 2;

    private int _JumpBackwardsIndex = 3;
    private int _JumpBackwardsLeftIndex = 6;
    private int _JumpBackwardsRightIndex = 7;
    #endregion

    private PlayerMovement.Direction _animDir;
    private PlayerMovement.Action _pAction;

    // All of the event controls that trigger special animations
    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        deltaT = 0;
        rb = GetComponentInParent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!IsSettings())
        {
            _horizontalInput = _pMovement.HorizontalInput;
            _verticalInput = _pMovement.VerticalInput;
        }
        else
        {
            _horizontalInput = 0;
            _verticalInput = 0;
        }

        // Get our animation data from the correct source
        if (_pMovement.enabled)
        {
            _animDir = _pMovement.GetDirection();
            _pAction = _pMovement.GetAction();
        }
        else
        {
            _pAction = _pFollower.GetAction();
            if (GameManager.Instance._currentGameState == GameManager.GameState.Interaction)
            {
                if(_interactionDir != PlayerMovement.Direction.Null && _pAction != PlayerMovement.Action.Walk)
                {
                    _animDir = _interactionDir;
                }
            }
            else
            {
                _animDir = _pFollower.GetDirection();
            }
            
        }


        // The logic that determines what animation should be played 
        if (!IsSettings() && !activeCoroutine)
        {
            if (IsGameplay() || IsInteraction()) // Logic for idle and walk cycles that occur during normal exploration
            {
                _frameReset = 0;
                animationSpeed = 8f;

                if (_pAction == PlayerMovement.Action.Walk) // If we're walking
                {
                    _frameLoop = 8;
                    
                    switch (_animDir)
                    {
                        case PlayerMovement.Direction.Forwards:
                            _aIndex = _WalkForwardsIndex;
                            break;
                        case PlayerMovement.Direction.ForwardsLeft:
                            _aIndex = _WalkForwardsLeftIndex;
                            break;
                        case PlayerMovement.Direction.ForwardsRight:
                            _aIndex = _WalkForwardsRightIndex;
                            break;
                        case PlayerMovement.Direction.Left:
                            _aIndex = _WalkLIndex;
                            break;
                        case PlayerMovement.Direction.Right:
                            _aIndex = _WalkRIndex;
                            break;
                        case PlayerMovement.Direction.BackwardsLeft:
                            _aIndex = _WalkBackwardsLeftIndex;
                            break;
                        case PlayerMovement.Direction.BackwardsRight:
                            _aIndex = _WalkBackwardsRightIndex;
                            break;
                        case PlayerMovement.Direction.Backwards:
                            _aIndex = _WalkBackwardsIndex;
                            break;
                        default:
                            _aIndex = _WalkBackwardsIndex;
                            break;
                    }
                }
                else if(_pAction == PlayerMovement.Action.JumpUp) // Jumping
                {
                    _frameLoop = 1;
                    switch (_animDir)
                    {
                        case PlayerMovement.Direction.Forwards:
                            _aIndex = _JumpForwardsIndex;
                            break;
                        case PlayerMovement.Direction.ForwardsLeft:
                            _aIndex = _JumpForwardsLeftIndex;
                            break;
                        case PlayerMovement.Direction.ForwardsRight:
                            _aIndex = _JumpForwardsRightIndex;
                            break;
                        case PlayerMovement.Direction.Left:
                            _aIndex = _JumpLIndex;
                            break;
                        case PlayerMovement.Direction.Right:
                            _aIndex = _JumpRIndex;
                            break;
                        case PlayerMovement.Direction.BackwardsLeft:
                            _aIndex = _JumpBackwardsLeftIndex;
                            break;
                        case PlayerMovement.Direction.BackwardsRight:
                            _aIndex = _JumpBackwardsRightIndex;
                            break;
                        case PlayerMovement.Direction.Backwards:
                            _aIndex = _JumpBackwardsIndex;
                            break;
                        default:
                            _aIndex = _JumpBackwardsIndex;
                            break;
                    }
                }

                else if (_pAction == PlayerMovement.Action.JumpDown) // Falling
                {
                    _frameReset = 1;
                    _frameLoop = 2;
                    switch (_animDir)
                    {
                        case PlayerMovement.Direction.Forwards:
                            _aIndex = _JumpForwardsIndex;
                            break;
                        case PlayerMovement.Direction.ForwardsLeft:
                            _aIndex = _JumpForwardsLeftIndex;
                            break;
                        case PlayerMovement.Direction.ForwardsRight:
                            _aIndex = _JumpForwardsRightIndex;
                            break;
                        case PlayerMovement.Direction.Left:
                            _aIndex = _JumpLIndex;
                            break;
                        case PlayerMovement.Direction.Right:
                            _aIndex = _JumpRIndex;
                            break;
                        case PlayerMovement.Direction.BackwardsLeft:
                            _aIndex = _JumpBackwardsLeftIndex;
                            break;
                        case PlayerMovement.Direction.BackwardsRight:
                            _aIndex = _JumpBackwardsRightIndex;
                            break;
                        case PlayerMovement.Direction.Backwards:
                            _aIndex = _JumpBackwardsIndex;
                            break;
                        default:
                            _aIndex = _JumpBackwardsIndex;
                            break;
                    }
                }

                else if (_pAction == PlayerMovement.Action.Idle)    // If we're idling
                {
                    animationSpeed = 1f;
                    _aIndex = _IdleIndex;

                    switch (_animDir)
                    {
                        case PlayerMovement.Direction.Forwards:
                            _frameReset = _IdleForwardsIndex;

                            break;
                        case PlayerMovement.Direction.ForwardsLeft:
                            _frameReset = _IdleForwardsLeftIndex;

                            break;
                        case PlayerMovement.Direction.ForwardsRight:
                            _frameReset = _IdleForwardsRightIndex;

                            break;
                        case PlayerMovement.Direction.Left:
                            _frameReset = _IdleLIndex;

                            break;
                        case PlayerMovement.Direction.Right:
                            _frameReset = _IdleRIndex;

                            break;
                        case PlayerMovement.Direction.BackwardsLeft:
                            _frameReset = _IdleBackwardsLeftIndex;

                            break;
                        case PlayerMovement.Direction.BackwardsRight:
                            _frameReset = _IdleBackwardsRightIndex;
 
                            break;
                        case PlayerMovement.Direction.Backwards:
                            _frameReset = _IdleBackwardsIndex;

                            break;
                        default:
                            _frameReset = _IdleBackwardsIndex;

                            break;
                    }

                    _frameLoop = _frameReset;
                    _frame = _frameReset;
                    deltaT = _frame / animationSpeed;
                }
            }

            else if (IsLoad())
            {
                animationSpeed = 1f;
                _aIndex = _IdleIndex;

                switch (_animDir)
                {
                    case PlayerMovement.Direction.Forwards:
                        _frameReset = _IdleForwardsIndex;
                        break;
                    case PlayerMovement.Direction.ForwardsLeft:
                        _frameReset = _IdleForwardsLeftIndex;
                        break;
                    case PlayerMovement.Direction.ForwardsRight:
                        _frameReset = _IdleForwardsRightIndex;
                        break;
                    case PlayerMovement.Direction.Left:
                        _frameReset = _IdleLIndex;
                        break;
                    case PlayerMovement.Direction.Right:
                        _frameReset = _IdleRIndex;
                        break;
                    case PlayerMovement.Direction.BackwardsLeft:
                        _frameReset = _IdleBackwardsLeftIndex;
                        break;
                    case PlayerMovement.Direction.BackwardsRight:
                        _frameReset = _IdleBackwardsRightIndex;
                        break;
                    case PlayerMovement.Direction.Backwards:
                        _frameReset = _IdleBackwardsIndex;
                        break;
                    default:
                        _frameReset = _IdleBackwardsIndex;
                        break;
                }

                _frameLoop = _frameReset;
            }

            string clipKey, frameKey; 
            // The clip key is which animation is playing, the frame key is which animation it's on
            // Our clip key is set to ROWS, so that each row is an animation
            if (axis == AnimationAxis.Rows) 
            {
                clipKey = rowProperty;
                frameKey = colProperty;
            }
            else
            {
                clipKey = colProperty;
                frameKey = rowProperty;
            }

            // Animate
            _frame = (int)(deltaT * animationSpeed);

            //TODO: Footstep sound logic
            /*if (walkType != 3)
            {
                if ((frame == 3) && !footBool1 && isMoving(horizontalInput, verticalInput))
                {
                    footBool1 = true;
                    playFootstep(frame);
                }
                if ((frame == 7) && !footBool2 && isMoving(horizontalInput, verticalInput))
                {
                    footBool2 = true;
                    playFootstep(frame);
                }
            }
            else
            {
                if ((frame == 2) && !footBool1 && isMoving(horizontalInput, verticalInput))
                {
                    footBool1 = true;
                    playFootstep(frame);
                }
                if ((frame == 6) && !footBool2 && isMoving(horizontalInput, verticalInput))
                {
                    footBool2 = true;
                    playFootstep(frame);
                }
            }*/

            deltaT += Time.deltaTime;
            if (_frame >= _frameLoop)
            {
                _frame = _frameReset;
                deltaT = _frame / animationSpeed; // 0
                

                footBool1 = false;
                footBool2 = false;
            }
            /*Debug.Log("Frame = " + _frame + " Frame Reset = " + _frameReset + " Frame Loop = " 
                + _frameLoop + " Action = " + _pAction + " Direction = " + _animDir);*/
            meshRenderer.material.SetFloat(clipKey, _aIndex);
            meshRenderer.material.SetFloat(frameKey, _frame);
        } // End of the settings / activeCoroutine lockout

    }

    // Private methods ----------------------------------------------------------

    private void playFootstep(int frame)
    {

        /*if (walkType == 0) // Stone
        {
            audioSource.Stop();
            if (frame == 3)
                audioSource.PlayOneShot(playerSounds[15], GameManager.Instance.entityVolume * GameManager.Instance.masterVolume);
            if (frame == 7)
                audioSource.PlayOneShot(playerSounds[16], GameManager.Instance.entityVolume * GameManager.Instance.masterVolume);
        }
        else if (walkType == 1) // Earth
        {
            audioSource.Stop();
            if (frame == 3)
                audioSource.PlayOneShot(playerSounds[19], GameManager.Instance.entityVolume * GameManager.Instance.masterVolume);
            if (frame == 7)
                audioSource.PlayOneShot(playerSounds[20], GameManager.Instance.entityVolume * GameManager.Instance.masterVolume);
        }
        else if (walkType == 2) // Wood
        {
            audioSource.Stop();
            if (frame == 3)
                audioSource.PlayOneShot(playerSounds[21], GameManager.Instance.entityVolume * GameManager.Instance.masterVolume);
            if (frame == 7)
                audioSource.PlayOneShot(playerSounds[22], GameManager.Instance.entityVolume * GameManager.Instance.masterVolume);
        }
        else // Water
        {
            if (frame == 2)
                audioSource.PlayOneShot(playerSounds[17], GameManager.Instance.entityVolume * GameManager.Instance.masterVolume);
            if (frame == 6)
                audioSource.PlayOneShot(playerSounds[18], GameManager.Instance.entityVolume * GameManager.Instance.masterVolume);
        }*/
    }

    public void SetInteractionDir(PlayerMovement.Direction dir)
    {
        _interactionDir = dir;
    }

    #region GAME STATE HELPERS
    private bool IsSettings()
    {
        return GameManager.Instance._currentGameState == GameManager.GameState.Pause;
    }

    private bool IsGameplay()
    {
        return GameManager.Instance._currentGameState == GameManager.GameState.Gameplay;
    }

    private bool IsInteraction()
    {
        return GameManager.Instance._currentGameState == GameManager.GameState.Interaction;
    }

    private bool IsLoad()
    {
        return GameManager.Instance._currentGameState == GameManager.GameState.Load;
    }
    #endregion
}
