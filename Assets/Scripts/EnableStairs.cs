using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;


public class EnableStairs : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject newTerrain;
    [SerializeField] NavMeshLinkData data;

    [SerializeField] GameObject mainCam;
    [SerializeField] GameObject newCam;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        newTerrain.SetActive(true);
        GetComponent<BoxCollider>().enabled = false;
        GetComponentInChildren<MeshRenderer>().enabled = false;

        StartCoroutine(DoCam());
    }

    IEnumerator DoCam()
    {
        mainCam.SetActive(false);
        newCam.SetActive(true);
        yield return new WaitForSeconds(4f);
        mainCam.SetActive(true);
        newCam.SetActive(false);
    }
}
