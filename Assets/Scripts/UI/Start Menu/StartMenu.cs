using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Animator _anim; // Animator for the start sequence
    [SerializeField] private GameObject _title;
    [SerializeField] private GameObject _credits;

    private bool _activeCoroutine;
    private bool _introDone;
    void Start()
    {
        
        
    }

    private void OnEnable()
    {
        if (!_introDone)
        {
            StartCoroutine(DoOpenAnim());
        }
        else
        {
            _anim.Play("Static");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        if(!_activeCoroutine)
            StartCoroutine(DoStartAnim());
    }

    public void Credits()
    {
        if (!_activeCoroutine)
        {
            _credits.SetActive(true);
            _title.SetActive(false);
        }
            
    }

    public void Back()
    {
        if (!_activeCoroutine)
        {
            _credits.SetActive(false);
            _title.SetActive(true);
        }
        
    }

    public void Quit()
    {
        if(!_activeCoroutine)
            Application.Quit();
    }

    private IEnumerator DoOpenAnim()
    {
        _activeCoroutine = true;
        _anim.Play("Open", 0, 0);
        yield return new WaitForSeconds(11f);
        _activeCoroutine = false;
        _introDone = true;
    }

    private IEnumerator DoStartAnim()
    {
        _activeCoroutine = true;
        _anim.Play("Start", 0, 0);
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Area1");

    }
}
