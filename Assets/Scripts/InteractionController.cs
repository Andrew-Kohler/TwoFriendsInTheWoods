using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.AI;
using Unity.Mathematics;
using UnityEngine.SceneManagement;

public class InteractionController : MonoBehaviour
{

    // Cinemachine cameras
    [SerializeField] private CinemachineVirtualCamera _interactionCamera;
    private CinemachineVirtualCamera _mainCamera;

    [SerializeField] private Conversation _dialogue;
    [Header("Final Interaction")]
    [SerializeField] private bool _endGame = false;
    [SerializeField] private SceneLoader _sceneLoader;
    [SerializeField] private CinemachineVirtualCamera _interactionCamera2;

    // Player GameObjects
    [Header("Player Objects")]
    [SerializeField, Tooltip ("Player 1")] private GameObject _p1;
    [SerializeField, Tooltip("Player 2")] private GameObject _p2;

    // Player components
    private PlayerMovement _p1Move;
    private Follower _p1Follow;
    private NavMeshAgent _p1Agent;
    private PlayerAnimatior _p1Anim;
    private AgentLinkMover _p1LinkMover;

    private PlayerMovement _p2Move;
    private P2Movement _p2Special;
    private Follower _p2Follow;
    private NavMeshAgent _p2Agent;
    private PlayerAnimatior _p2Anim;
    private AgentLinkMover _p2LinkMover;

    private bool _wasP1Leader;

    // Points the characters will navigate to for cutscene
    [Header("Player Interaction Positions")]
    [SerializeField] private Transform _character1InteractPoint;
    [SerializeField] private Transform _character2InteractPoint;

    [Header("Player Interaction Directions")]
    [SerializeField] private PlayerMovement.Direction _character1Dir;
    [SerializeField] private PlayerMovement.Direction _character2Dir;
    [SerializeField] private ParticleSystem _sys;

    // Private variables
    private bool _activeCoroutine = false;
    bool isSkippingLine;

    void Start()
    {
        _mainCamera = GameObject.Find("VirtualMain").GetComponent<CinemachineVirtualCamera>();

        _p1Move = _p1.GetComponent<PlayerMovement>();
        _p1Follow = _p1.GetComponent<Follower>();
        _p1Agent = _p1.GetComponent<NavMeshAgent>();
        _p1Anim = _p1.GetComponentInChildren<PlayerAnimatior>();
        _p1LinkMover = _p1.GetComponent<AgentLinkMover>();

        _p2Move = _p2.GetComponent<PlayerMovement>();
        _p2Follow = _p2.GetComponent<Follower>();
        _p2Agent = _p2.GetComponent<NavMeshAgent>();
        _p2Anim = _p2.GetComponentInChildren<PlayerAnimatior>();
        _p2Special = _p2.GetComponent<P2Movement>();
        _p2LinkMover = _p2.GetComponent<AgentLinkMover>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator DoInteraction()
    {
        _activeCoroutine = true;

        // Remove control from the player
        GameManager.Instance._currentGameState = GameManager.GameState.Interaction;

        // Turn off the particle system
        ParticleSystem.EmissionModule e = _sys.emission;
        e.rateOverTime = 0;
        GetComponent<MeshRenderer>().enabled = false;

        // Blend the camera from its current position to the virtual camera we have set up
        _interactionCamera.gameObject.SetActive(true);
        _mainCamera.Follow = null;
        _mainCamera.LookAt = null;
        _mainCamera.gameObject.SetActive(false);

        // Move the players to certain points
        if (_p1Move.enabled)
        {
            _wasP1Leader = true;

            _p1Move.enabled = false;

            _p1Follow.UpdateCurrentPosition();
            _p1Follow.enabled = true;
            
            _p1Agent.enabled = true;
            _p1Agent.isStopped = false;
            _p1LinkMover.enabled = true;
        }
        else
        {
            _wasP1Leader = false;

            _p2Move.enabled = false;

            _p2Follow.UpdateCurrentPosition();
            _p2Follow.enabled = true;
            

            _p2Agent.enabled = true;
            _p2Agent.isStopped = false;
            _p2LinkMover.enabled = true;
        }

        // Save the old transforms of where both characters were going, and then give them new ones
        Transform _oldP1 = _p1Follow.GetGoal();
        Transform _oldP2 = _p2Follow.GetGoal();
        _p1Follow.SetGoal(_character1InteractPoint);
        _p2Follow.SetGoal(_character2InteractPoint);

        // Tell them how to pose when they get there
        _p1Anim.SetInteractionDir(_character1Dir);
        _p2Anim.SetInteractionDir(_character2Dir);

        // Wait for everything to happen
        
        yield return new WaitUntil(() => _p1Follow.GetAction() == PlayerMovement.Action.Idle);
        yield return new WaitUntil(() => _p2Follow.GetAction() == PlayerMovement.Action.Idle);

        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Area3" || sceneName == "Area8")
        {
            GameManager.IsSit = true;
            if (sceneName == "Area3")
                GameManager.TowardsCam = true;
        }

        yield return new WaitForSeconds(.7f);

        // Get the positions of the characters in terms of the screen so the speech bubbles work
        Vector3 chara1Raw = Camera.main.WorldToScreenPoint(_p1.transform.position);
        Vector2 chara1Screen = new Vector3(chara1Raw.x / Screen.width, chara1Raw.y / Screen.height);
        Vector3 chara2Raw = Camera.main.WorldToScreenPoint(_p2.transform.position);
        Vector2 chara2Screen = new Vector3(chara2Raw.x / Screen.width, chara2Raw.y / Screen.height);

        float chara1XTrue = math.remap(0, 1, -575, 575, chara1Screen.x);
        float chara2XTrue = math.remap(0, 1, -575, 575, chara2Screen.x);

        // Trigger the dialogue sequence
        ViewManager.Show<Dialogue>(false);
        //ViewManager.GetView<Dialogue>().WakeTri(true);
        for (int i = 0; i < _dialogue.lines.Count; i++)
        {
            isSkippingLine = false;
            if (_dialogue.bubbleChara1[i])
            {
                ViewManager.GetView<Dialogue>().setBubbleConnector(false, chara2XTrue);
            }
            else
            {
                ViewManager.GetView<Dialogue>().setBubbleConnector(true, chara1XTrue);
            }
            ViewManager.GetView<Dialogue>().setSpaceBar(false);
            StartCoroutine(DoTextEscapeSubroutine());
            for (int j = 0; j < _dialogue.lines[i].Length; j++)
            {
                if (!isSkippingLine)
                {
                    if (_dialogue.lines[i][j].Equals(' '))
                    {
                        j++;
                    }
                    ViewManager.GetView<Dialogue>().setText(_dialogue.lines[i].Substring(0, j));

                    yield return new WaitForSeconds(.07f);
                }
                else
                {
                    ViewManager.GetView<Dialogue>().setText(_dialogue.lines[i]);
                    ViewManager.GetView<Dialogue>().setSpaceBar(true);
                    break;
                }

            }
            StopCoroutine(DoTextEscapeSubroutine());
            ViewManager.GetView<Dialogue>().setText(_dialogue.lines[i]);
            ViewManager.GetView<Dialogue>().setSpaceBar(true);
            yield return new WaitUntil(() => !Input.GetButtonDown("Jump")); // Make the player lift the button so they don't hold through
            yield return new WaitUntil(() => Input.GetButtonDown("Jump"));
        }
        // Wait until that's done
        ViewManager.Show<Standard>(false);
        GameManager.IsSit = false;
        GameManager.TowardsCam = false;

        if (_endGame)
        {
            _interactionCamera2.gameObject.SetActive(true);
            _interactionCamera.gameObject.SetActive(false);
            _p1Anim.enabled = false;
            _p2Anim.enabled = false;
            yield return new WaitForSeconds(3f);
            ViewManager.Show<Transition>(false);
            _sceneLoader.LoadNextScene();
        }
        else
        {
            // Blend the camera back to main camera
            if(SceneManager.GetActiveScene().name != "Area8")
            {
                if (_wasP1Leader)
                {
                    _mainCamera.Follow = _p1.transform;
                    _mainCamera.LookAt = _p1.transform;
                }
                else
                {
                    _mainCamera.Follow = _p2.transform;
                    _mainCamera.LookAt = _p2.transform;
                }
            }
            
            _mainCamera.gameObject.SetActive(true);

            yield return new WaitForSeconds(2f);

            // Return control to the player ---------------------------------------------------------
            GameManager.Instance._currentGameState = GameManager.GameState.Gameplay;

            _p1Anim.SetInteractionDir(PlayerMovement.Direction.Null);
            _p2Anim.SetInteractionDir(PlayerMovement.Direction.Null);

            _p1Follow.SetGoal(_oldP1);
            _p2Follow.SetGoal(_oldP2);

            if (_wasP1Leader)
            {

                _p1Move.enabled = true;

                _p1Follow.enabled = false;
                _p1Agent.isStopped = true;

                _p1LinkMover.enabled = false;
            }
            else
            {
                _p2Move.enabled = true;

                _p2Follow.enabled = false;
                _p2Agent.isStopped = true;

                _p2LinkMover.enabled = false;
            }

            _activeCoroutine = false;
            Destroy(gameObject);
        }

        
        yield return null;  
    }

    private IEnumerator DoTextEscapeSubroutine()
    {
        if (GameManager.IsTapThru)
        {
            yield return new WaitUntil(() => !Input.GetButtonDown("Jump")); // Make the player lift the button so they don't hold through
            yield return new WaitUntil(() => Input.GetButtonDown("Jump"));
            isSkippingLine = true;
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        // If it's a player
        // If an interaction is not ongoing
        // If they aren't using their special abilities
        // If the follower isn't trapped on the other side of something
        // If this is the player the player has control over
        if (other.CompareTag("Player") && !_activeCoroutine && !Input.GetButton("Ability") && GameManager.CanLoadAgent && _p1Follow.Navigate == true)
        {
            if (other.GetComponentInParent<PlayerMovement>().enabled)
            {
                StartCoroutine(DoInteraction());
                _activeCoroutine = true;
            }
            
        }
    }

}
