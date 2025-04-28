using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : View
{
    public override void Initialize()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        ViewManager.ShowLast();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
