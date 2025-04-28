using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallsAnim : MonoBehaviour
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

    private int _FallsIndex = 1;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        deltaT = 0;
        rb = GetComponentInParent<Rigidbody>();
        //audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _frameReset = 0;
        animationSpeed = 8f;
        _frameLoop = 15;
        

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

