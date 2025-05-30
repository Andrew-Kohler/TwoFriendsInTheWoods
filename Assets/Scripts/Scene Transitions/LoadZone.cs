using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadZone : MonoBehaviour
{
    [SerializeField] private SceneLoader _loader;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.CanLoadAgent && !Input.GetButton("Ability"))
        {
            if(other.GetComponentInParent<PlayerMovement>().enabled){
                ViewManager.Show<Transition>(false);
                _loader.LoadNextScene();
            }
            
        }
    }
}
