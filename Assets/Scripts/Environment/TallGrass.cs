using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TallGrass : MonoBehaviour
{
    private Animator _anim;
    void Start()
    {
        _anim = GetComponent<Animator>();
        _anim.enabled = false;
        StartCoroutine(DoRandomStart());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator DoRandomStart()
    {
        yield return new WaitForSeconds(Random.Range(0, .6f));
        _anim.enabled = true;
    }
}
