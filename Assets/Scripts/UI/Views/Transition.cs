using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : View
{
    private Animator _anim;
    public override void Initialize()
    {
        _anim = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySceneExit()
    {
        _anim.Play("SceneExit");
    }
}
