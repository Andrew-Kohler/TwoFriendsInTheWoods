using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Animator _anim; // Animator for the start sequence
    [SerializeField] private GameObject _logo;
    [SerializeField] private GameObject _title;
    [SerializeField] private GameObject _options;
    [Header("Options Values")]
    [SerializeField] private Toggle _casualToggle;
    [SerializeField] private Toggle _gamerToggle;
    [SerializeField] private Toggle _tapThruToggle;
    [SerializeField] private Slider _volSlider;
    [SerializeField] private TextMeshProUGUI _volReadout;
    [Header("Sounds")]
    [SerializeField] private AudioClip _splash1;
    [SerializeField] private AudioClip _splash2;
    private AudioSource _source;
    [SerializeField] private GameObject _soundBoxGO;
    [SerializeField] private SoundBox _soundBox;

    private bool _activeCoroutine;
    private bool _introDone;
    private bool _loadToCredits;
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
            if (GameManager.GameComplete)
                _anim.Play("Static2");
            else
                _anim.Play("Static");
        }

        GameManager.Instance._currentGameState = GameManager.GameState.Gameplay;
        // Options config
        if (GameManager.IsGamerControls)
        {
            _gamerToggle.isOn = true;
            _casualToggle.isOn = false;
        }
        else
        {
            _gamerToggle.isOn = false;
            _casualToggle.isOn = true;
        }

        if (GameManager.IsTapThru)
            _tapThruToggle.isOn = true;
        else
            _tapThruToggle.isOn = false;

        _volSlider.value = GameManager.GameVol * 10;

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

    public void Options()
    {
        if (!_activeCoroutine)
        {
            _options.SetActive(true);
            _title.SetActive(false);
        }
            
    }

    public void Credits()
    {
        _loadToCredits = true;
        if (!_activeCoroutine)
            StartCoroutine(DoStartAnim());
    }

    public void Back()
    {
        if (!_activeCoroutine)
        {
            _options.SetActive(false);
            _title.SetActive(true);

            if (GameManager.GameComplete)
                _anim.Play("Static2");
            else
                _anim.Play("Static");
        }
        
    }

    public void Quit()
    {
        if(!_activeCoroutine)
            Application.Quit();
    }

    #region OPTIONS BUTTONS
    public void ToggleToGamer()
    {
        GameManager.IsGamerControls = _gamerToggle.isOn;
        _casualToggle.isOn = !_gamerToggle.isOn;

    }

    public void ToggleToCasual()
    {
        _gamerToggle.isOn = !_casualToggle.isOn;
        GameManager.IsGamerControls = _gamerToggle.isOn;

    }

    public void ToggleTapThru()
    {
        GameManager.IsTapThru = _tapThruToggle.isOn;
    }

    public void VolumeSlider()
    {
        GameManager.GameVol = _volSlider.value / 10;
        _volReadout.text = _volSlider.value * 10f + "%";
    }
    #endregion

    private IEnumerator DoOpenAnim()
    {
        _source = GetComponent<AudioSource>();
        _activeCoroutine = true;
        if (!GameManager.GameComplete) // If we've beaten the game, skip the studio logo on bootup (Farflung-core)
        {
            yield return new WaitForSeconds(.3f);
            _source.PlayOneShot(_splash1);
            yield return new WaitForSeconds(2f);
            _source.PlayOneShot(_splash2);
            yield return new WaitForSeconds(3.7f);
        }
        _logo.SetActive(false);
        yield return new WaitForSeconds(1f);
        _title.SetActive(true);
        _soundBoxGO.SetActive(true);
        _soundBox.newSceneCheck();

        if (!GameManager.GameComplete)
            _anim.Play("Open", 0, 0);
        else
            _anim.Play("Open2", 0, 0);

        yield return new WaitForSeconds(11f);
        _activeCoroutine = false;
        _introDone = true;
    }

    private IEnumerator DoStartAnim()
    {
        _activeCoroutine = true;

        if (!GameManager.GameComplete)
            _anim.Play("Start", 0, 0);
        else
            _anim.Play("Start2", 0, 0);

        GameManager.Instance._currentGameState = GameManager.GameState.Load;
        yield return new WaitForSeconds(2f);

        _soundBox.FadeOut();
        yield return new WaitForSeconds(3f);

        if (_loadToCredits)
        {
            SceneManager.LoadScene("13 - Credits");
        }
        else
        {
            SceneManager.LoadScene("Area1");
        }
        

    }
}
