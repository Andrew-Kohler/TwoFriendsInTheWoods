using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class InteractionController : MonoBehaviour
{

    // Cinemachine cameras
    [SerializeField] private CinemachineVirtualCamera _interactionCamera;
    private CinemachineVirtualCamera _mainCamera;

    [SerializeField] private TextAsset _dialogue;

    // Points the characters will navigate to for cutscene
    [SerializeField] private Transform _character1InteractPoint;
    [SerializeField] private Transform _character2InteractPoint;

    // Points the characters will navigate to before resuming play
    [SerializeField] private Transform _character1ResumePoint;
    [SerializeField] private Transform _character2ResumePoint;

    // Private variables
    private bool _canInteract;
    private bool _activeCoroutine = false;

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

        // Trigger the dialogue sequence
        // Wait until that's done
        // Move the players back to a resumable spot

        // Blend the camera back to main camera
        _mainCamera.gameObject.SetActive(true);

        // Return control to the player
        GameManager.Instance._currentGameState = GameManager.GameState.Gameplay;

        _activeCoroutine = false;
        yield return null;  
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
