using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;


public class EnableStairs : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject newTerrain;
    [SerializeField] GameObject oldTerrain;
    [SerializeField] NavMeshLinkData data;

    [SerializeField] private GameObject _mainCam;
    [SerializeField] GameObject newCam;
    [SerializeField] private float _time = 4f;

    [SerializeField] private ParticleSystem _sys;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !Input.GetButton("Ability"))
        {
            if (other.GetComponentInParent<PlayerMovement>().enabled)
            {
                newTerrain.SetActive(true);
                oldTerrain.SetActive(false);

                // Turn everything off
                GetComponent<BoxCollider>().enabled = false;
                GetComponent<MeshRenderer>().enabled = false;
                ParticleSystem.EmissionModule e = _sys.emission;
                e.rateOverTime = 0;

                StartCoroutine(DoCam());
            }
            
        }
        
    }

    IEnumerator DoCam()
    {
        _mainCam.SetActive(false);
        newCam.SetActive(true);
        GameManager.CanToss = false;
        yield return new WaitForSeconds(_time);
        _mainCam.SetActive(true);
        newCam.SetActive(false);

        yield return new WaitForSeconds(2f);
        GameManager.CanToss = true;
    }
}
