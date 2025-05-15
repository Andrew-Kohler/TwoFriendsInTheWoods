using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TallGrass : MonoBehaviour
{
    private Animator _anim;
    [Header("Grass 1")]
    [SerializeField] private GameObject _g1arm;
    [SerializeField] private GameObject _g1model;
    [SerializeField] private Animator _anim1;
    [Header("Grass 2")]
    [SerializeField] private GameObject _g2;
    [SerializeField] private Animator _anim2;

    void Start()
    {
        if(GetComponent<SpriteRenderer>() != null)
        {
            _anim = GetComponent<Animator>();
        }
        else
        {
            _anim = _anim1;

            float rand = Random.Range(0.0f, 1.01f);
            if (rand > .5f)
            {
                _anim = _anim2;
                _g2.SetActive(true);
                _g1arm.SetActive(false);
                _g1model.SetActive(false);
            }
        }
        
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
