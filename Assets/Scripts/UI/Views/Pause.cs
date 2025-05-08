using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Pause : View
{
    [SerializeField] private GameObject _optionsSubmenu;
    [SerializeField] private GameObject _mainSubmenu;
    [Header("Options Values")]
    [SerializeField] private Toggle _casualToggle;
    [SerializeField] private Toggle _gamerToggle;
    [SerializeField] private Toggle _tapThruToggle;
    [SerializeField] private Slider _volSlider;
    [SerializeField] private TextMeshProUGUI _volReadout;

    public override void Initialize()
    {
        //StartCoroutine(InitializeCoroutine());
    }

    private void OnEnable()
    {
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

    public void Resume()
    {
        Time.timeScale = 1f;
        ViewManager.ShowLast();
    }

    public void Options()
    {
        _mainSubmenu.SetActive(false);
        _optionsSubmenu.SetActive(true);
    }

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

    public void OptionsBack()
    {
        _mainSubmenu.SetActive(true);
        _optionsSubmenu.SetActive(false);   
    }

    public void Quit()
    {
        Application.Quit();
    }

    /*private IEnumerator InitializeCoroutine()
    {
        yield return new WaitForEndOfFrame();
        // Start checks for the toggles
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

    }*/
}
