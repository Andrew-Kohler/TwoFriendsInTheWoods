using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseListener : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause") && GameManager.Instance._currentGameState != GameManager.GameState.Load && Time.timeScale == 1f)
        {
            ViewManager.Show<Pause>(true);
            Time.timeScale = 0f;
        }
    }
}
