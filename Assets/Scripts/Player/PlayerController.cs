using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    // Things enabled for active P1
    private PlayerMovement _pMovement;
    private FollowPointMover _pointMover;

    // Things enabled for follower P2
    private NavMeshAgent _agent;
    private Follower _follower;
    private AgentLinkMover _linkMover;

    // The camera (to change its target)
    private CinemachineVirtualCamera _cam;
    [SerializeField] private CinemachineVirtualCamera _cam2;

    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _pMovement = GetComponent<PlayerMovement>();
        //_pointMover = GetComponent<FollowPointMover>();
        _agent = GetComponent<NavMeshAgent>();
        _follower = GetComponent<Follower>();
        _linkMover = GetComponent<AgentLinkMover>();

        _cam = GameObject.FindWithTag("Cam_V-Main").GetComponent<CinemachineVirtualCamera>();

        // Determines who the player controls on the next scene load
        StartCoroutine(DoInitial());
        
    }

    // Update is called once per frame
    void Update()
    {
        _rb.useGravity = true;
        if (Input.GetButtonDown("Swap") && !Input.GetButton("Ability") && !(GameManager.Instance._currentGameState == GameManager.GameState.Interaction))
        {
            ToggleActivePlayer();
        }
    }

    private void ToggleActivePlayer()
    {
        _pMovement.enabled = !_pMovement.enabled;
        //_pointMover.enabled = !_pMovement.enabled;
        if (_pMovement.enabled)
        {
            GameManager.P1Leading = !GameManager.P1Leading;
            _agent.isStopped = true;
            _follower.enabled = false;
            _linkMover.enabled = false;
        }
        else
        {
            _agent.enabled = true;
            _agent.isStopped = false;
            _follower.enabled = true;
            _linkMover.enabled = true;
        }

        

        if (_pMovement.enabled)
        {
            _cam.Follow = this.gameObject.transform;
            _cam.LookAt = this.gameObject.transform;

            if(_cam2 != null)
            {
                _cam2.Follow = this.gameObject.transform;
                _cam2.LookAt = this.gameObject.transform;
            }
            
            
        }

    }

    private IEnumerator DoInitial()
    {
        yield return new WaitForEndOfFrame();
        if (GameManager.P1Leading)
        {
            if (this.gameObject.name == "P1")
                _pMovement.enabled = true;
            else
                _pMovement.enabled = false;
        }
        else
        {
            if (this.gameObject.name == "P1")
                _pMovement.enabled = false;
            else
                _pMovement.enabled = true;
        }


        if (_pMovement.enabled)
        {
            _agent.isStopped = true;
            _agent.enabled = false;
            _follower.enabled = false;
            _linkMover.enabled = false;

            _cam.Follow = this.gameObject.transform;
            _cam.LookAt = this.gameObject.transform;
        }
        else
        {
            _agent.isStopped = false;
            _follower.enabled = true;
            _linkMover.enabled = true;
        }
    }
}
