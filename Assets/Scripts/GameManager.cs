/*
Game Manager
Used on:    ---
For:    Manages the state of the game and tells everything else what's going on
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    public enum GameState { Gameplay, Interaction, Pause, Load };
    public GameState _currentGameState;

    public static bool P1Leading;

    private static void DefaultData()
    {
        P1Leading = true;
    }

    #region SETTINGS VALUES
    private float _volumeMain = 1;
    private float _volumeAmbient = 1;
    private float _volumeText = 1;
    #endregion

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject managerHolder = new GameObject("[Game Manager]");
                managerHolder.AddComponent<GameManager>();
                DontDestroyOnLoad(managerHolder);
                _instance = managerHolder.GetComponent<GameManager>();
                DefaultData();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        //_instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region VOLUME METHODS
    public void setVolumeMain(float vol)
    {
        _volumeMain = vol;
    }

    public float getVolumeMain()
    {
        return _volumeMain;
    }

    public void setVolumeAmbient(float vol)
    {
        _volumeAmbient = vol;
    }

    public float getVolumeAmbient()
    {
        return _volumeMain * _volumeAmbient;
    }

    public void setVolumeText(float vol)
    {
        _volumeText = vol;
    }

    public float getVolumeText()
    {
        return _volumeMain * _volumeText;
    }
    #endregion
}
