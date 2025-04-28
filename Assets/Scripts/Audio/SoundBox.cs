using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SoundBox : MonoBehaviour
{
    private AudioSource _audioSource;
    private bool silent;
    private bool activeCoroutine = false;

    [SerializeField] private AudioClip _forest;
    [SerializeField] private AudioClip _ridge;
    [SerializeField] private AudioClip _valley;
    [SerializeField] private AudioClip _falls;

    float songTime;
    private float _maxVol;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
        //newSceneCheck();

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneLoader.onLoadOut += FadeOut;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneLoader.onLoadOut -= FadeOut;
    }

    private void Update()
    {
        

    }

    public void newSceneCheck()
    {
        _audioSource = GetComponent<AudioSource>();
        int val = sceneCheck();

        if (val == 0)
        {
            _maxVol = .5f;
            if (_audioSource.clip != _forest)
                _audioSource.clip = _forest;
        }
        else if (val == 1)
        {
            _audioSource.clip = _ridge;
            _maxVol = .15f;
        }
        else if(val == 2)
        {
            _audioSource.clip = _valley;
        }
        else if (val == 4)
        {
            _maxVol = .1f;
            _audioSource.clip = _falls;
        }

        silent = false;
        if (!_audioSource.isPlaying)
            _audioSource.Play();
        StartCoroutine(DoFadeIn());


        if (val == 3)
        {
            silent = true;
            StopAllCoroutines();
            activeCoroutine = false;
            _audioSource.volume = 0f;
            if (_audioSource.isPlaying)
                _audioSource.Stop();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // Not really sure about these paramenters, but they're required, so sure?
    {
        newSceneCheck();
    }

    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(DoFadeIn());
    }

    public void FadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(DoFadeOut());
    }

    public void HardStop()
    {
        silent = true;
    }

    private int sceneCheck()
    {
        if (SceneManager.GetActiveScene().name == "Area4" || SceneManager.GetActiveScene().name == "Area6")
        {
            return 1; // Ridge
        }
        else if (SceneManager.GetActiveScene().name == "Area8")
        {
            return 2; // Valley
        }
        else if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            return 3;
        }
        else if (SceneManager.GetActiveScene().name == "Area11")
        {
            return 4;
        }
        return 0; // Forest
    }

    private IEnumerator DoFadeIn()
    {
        activeCoroutine = true;
        while (_audioSource.volume < _maxVol)
        {
            _audioSource.volume += Time.deltaTime * _maxVol;
            yield return null;
        }
        //_audioSource.time = songTime;
        _audioSource.volume = _maxVol;
        activeCoroutine = false;
        yield return null;
    }

    private IEnumerator DoFadeOut()
    {
        activeCoroutine = true;
        while (_audioSource.volume > 0)
        {
            _audioSource.volume -= Time.deltaTime * _maxVol;
            yield return null;
        }
        //_audioSource.time = songTime;
        _audioSource.volume = 0;
        activeCoroutine = false;
        yield return null;
    }

}
