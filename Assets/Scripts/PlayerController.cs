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
    }

    // Update is called once per frame
    void Update()
    {
        _rb.useGravity = true;
        if (Input.GetButtonDown("Swap"))
        {
            ToggleActivePlayer();
        }
    }

    private void ToggleActivePlayer()
    {
        _pMovement.enabled = !_pMovement.enabled;
       //_pointMover.enabled = !_pMovement.enabled;

        _agent.enabled = !_agent.enabled;
        _follower.enabled = !_follower.enabled;
        _linkMover.enabled = !_linkMover.enabled;

        if (_pMovement.enabled)
        {
            _cam.Follow = this.gameObject.transform;
            _cam.LookAt = this.gameObject.transform;
        }

    }
}
