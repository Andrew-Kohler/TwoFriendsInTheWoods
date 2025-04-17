using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailSign : MonoBehaviour
{
    [SerializeField] private GameObject text;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.SetActive(false);
        }
    }
}
