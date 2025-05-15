using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : View
{
    private Animator _anim;
    public override void Initialize()
    {
        _anim = GetComponent<Animator>();
        //StartCoroutine(DoSceneLoadHold());
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

    private IEnumerator DoSceneLoadHold()
    {
        _anim.speed = 0f;
        yield return new WaitForSeconds(.5f);
        _anim.speed = 1f;
    }
}
