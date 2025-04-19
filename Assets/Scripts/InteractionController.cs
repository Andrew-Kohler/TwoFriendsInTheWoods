using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.AI;
using Unity.Mathematics;

public class InteractionController : MonoBehaviour
{

    // Cinemachine cameras
    [SerializeField] private CinemachineVirtualCamera _interactionCamera;
    private CinemachineVirtualCamera _mainCamera;

    [SerializeField] private Conversation _dialogue;

    // Player GameObjects
    [SerializeField, Tooltip ("Player 1")] private GameObject _p1;
    [SerializeField, Tooltip("Player 2")] private GameObject _p2;

    // Player components
    private PlayerMovement _p1Move;
    private Follower _p1Follow;
    private NavMeshAgent _p1Agent;

    private PlayerMovement _p2Move;
    private Follower _p2Follow;
    private NavMeshAgent _p2Agent;

    private bool _wasP1Leader;

    // Points the characters will navigate to for cutscene
    [SerializeField] private Transform _character1InteractPoint;
    [SerializeField] private Transform _character2InteractPoint;
    [SerializeField] private ParticleSystem _sys;

    // Private variables
    private bool _canInteract;
    private bool _activeCoroutine = false;
    bool isSkippingLine;

    void Start()
    {
        _mainCamera = GameObject.Find("VirtualMain").GetComponent<CinemachineVirtualCamera>();

        _p1Move = _p1.GetComponent<PlayerMovement>();
        _p1Follow = _p1.GetComponent<Follower>();
        _p1Agent = _p1.GetComponent<NavMeshAgent>();

        _p2Move = _p2.GetComponent<PlayerMovement>();
        _p2Follow = _p2.GetComponent<Follower>();
        _p2Agent = _p2.GetComponent<NavMeshAgent>();
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
        // Blend the camera from its current position to the virtual camera we have set up
        _interactionCamera.gameObject.SetActive(true);
        _mainCamera.gameObject.SetActive(false);

        ParticleSystem.EmissionModule e = _sys.emission;
        e.rateOverTime = 0;
        GetComponent<MeshRenderer>().enabled = false;

        // Move the players to certain points
        if (_p1Move.enabled)
        {
            _wasP1Leader = true;

            _p1Move.enabled = false;

            _p1Follow.enabled = true;
            _p1Agent.enabled = true;
            _p1Agent.isStopped = false;
        }
        else
        {
            _wasP1Leader = false;

            _p2Move.enabled = false;

            _p2Follow.enabled = true;
            _p2Agent.enabled = true;
            _p2Agent.isStopped = false;
        }

        Transform _oldP1 = _p1Follow.GetGoal();
        Transform _oldP2 = _p2Follow.GetGoal();
        _p1Follow.SetGoal(_character1InteractPoint);
        _p2Follow.SetGoal(_character2InteractPoint);
        yield return new WaitForSeconds(2f);

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
                    break;
                }

            }
            StopCoroutine(DoTextEscapeSubroutine());
            ViewManager.GetView<Dialogue>().setText(_dialogue.lines[i]);
            yield return new WaitUntil(() => !Input.GetButtonDown("Interact")); // Make the player lift the button so they don't hold through
            yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
        }
        // Wait until that's done
        ViewManager.Show<Standard>(false);

        // Blend the camera back to main camera
        _mainCamera.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        // Return control to the player ---------------------------------------------------------
        GameManager.Instance._currentGameState = GameManager.GameState.Gameplay;

        _p1Follow.SetGoal(_oldP1);
        _p2Follow.SetGoal(_oldP2);

        if (_wasP1Leader)
        {

            _p1Move.enabled = true;

            _p1Follow.enabled = false;
            _p1Agent.isStopped = true;
        }
        else
        {
            _p2Move.enabled = true;

            _p2Follow.enabled = false;
            _p2Agent.isStopped = true;
        }

        _activeCoroutine = false;
        Destroy(gameObject);
        yield return null;  
    }

    private IEnumerator DoTextEscapeSubroutine()
    {
        yield return new WaitUntil(() => !Input.GetButtonDown("Interact")); // Make the player lift the button so they don't hold through
        yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
        isSkippingLine = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_activeCoroutine)
        {
            StartCoroutine(DoInteraction());
            _activeCoroutine = true;
        }
    }

}
