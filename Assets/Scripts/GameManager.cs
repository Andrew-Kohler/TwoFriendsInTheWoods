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
    public static bool CanLoadAgent;    // Whether the characters are together and we can move along
    public static bool CanToss;         // Whether P2 can toss (used to prevent cam jank)
    public static bool IsGamerControls; // What control scheme is read onto the trail signs
    public static bool IsTapThru;       // Whether the player can rapid tap through dialogue
    public static bool GameComplete;    // If the player has been to the credits

    public static bool IsSit;           // The worst boolean ever invented, for a specific late implementation animation bypass
    public static bool TowardsCam;      // The second worst boolean ever invented

    public static float GameVol;

    private static void DefaultData()
    {
        P1Leading = true;
        CanLoadAgent = true;
        CanToss = true;

        IsGamerControls = true;
        IsTapThru = false;
        GameComplete = false;

        IsSit = false;
        TowardsCam = false;

        GameVol = 1;
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
