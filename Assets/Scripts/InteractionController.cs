using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class InteractionController : MonoBehaviour
{

    // Cinemachine cameras
    [SerializeField] private CinemachineVirtualCamera _interactionCamera;
    private CinemachineVirtualCamera _mainCamera;

    [SerializeField] private Conversation _dialogue;

    [SerializeField] private PlayerMovement _move;

    // Points the characters will navigate to for cutscene
    [SerializeField] private Transform _character1InteractPoint;
    [SerializeField] private Transform _character2InteractPoint;

    // Points the characters will navigate to before resuming play
    [SerializeField] private Transform _character1ResumePoint;
    [SerializeField] private Transform _character2ResumePoint;

    // Private variables
    private bool _canInteract;
    private bool _activeCoroutine = false;
    bool isSkippingLine;

    void Start()
    {
        _mainCamera = GameObject.Find("VirtualMain").GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!_activeCoroutine && _canInteract && Input.GetButtonDown("Interact"))
        {
            StartCoroutine(DoInteraction());
        }
    }

    private IEnumerator DoInteraction()
    {
        _activeCoroutine = true;
        // Remove control from the player
        GameManager.Instance._currentGameState = GameManager.GameState.Interaction;
        // Blend the camera from its current position to the virtual camera we have set up
        _interactionCamera.gameObject.SetActive(true);
        _mainCamera.gameObject.SetActive(false);

        // Move the players to certain points
        _move.MoveToPoint(true, _character1InteractPoint.transform.position);
        yield return new WaitForSeconds(2f);
        _move.MoveToPoint(false, _character1InteractPoint.transform.position);

        Vector3 chara1Raw = Camera.main.WorldToScreenPoint(_character1InteractPoint.transform.position);
        Vector2 chara1Screen = new Vector3(chara1Raw.x / Screen.width, chara1Raw.y / Screen.height);
        Vector3 chara2Raw = Camera.main.WorldToScreenPoint(_character2InteractPoint.transform.position);
        Vector2 chara2Screen = new Vector3(chara2Raw.x / Screen.width, chara2Raw.y / Screen.height);

        // Trigger the dialogue sequence
        ViewManager.Show<Dialogue>(false);
        ViewManager.GetView<Dialogue>().WakeTri(true);
        for (int i = 0; i < _dialogue.lines.Count; i++)
        {
            isSkippingLine = false;
            if (_dialogue.bubbleChara1[i])
            {
                ViewManager.GetView<Dialogue>().setBubbleConnector(chara2Screen);
            }
            else
            {
                ViewManager.GetView<Dialogue>().setBubbleConnector(chara1Screen);
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
        ViewManager.GetView<Dialogue>().WakeTri(false);
        ViewManager.Show<Standard>(false);


        // Move the players back to a resumable spot

        // Blend the camera back to main camera
        _mainCamera.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        // Return control to the player
        GameManager.Instance._currentGameState = GameManager.GameState.Gameplay;

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

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canInteract = false;
        }
    }

}
