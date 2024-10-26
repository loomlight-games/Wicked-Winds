using System;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

/// <summary>
/// Manages the game states
/// </summary>
public class GameManager : AStateController
{
    public static GameManager Instance; //only one GameManager in the game (singleton)
    public event EventHandler<string> ButtonClicked;

    #region STATES
    public readonly GamePauseState pauseState = new();
    public readonly MainMenuState mainMenuState = new();
    public readonly GamePlayState playState = new();
    public readonly FinalState endState = new();
    public readonly CreditsState creditsState = new();
    public readonly SettingsState settingsState = new();
    public readonly ShopState shopState = new();
    public readonly LeaderboardGameState leaderboardState = new();
    #endregion

    [HideInInspector] public readonly string PLAYER_SCORE_FILE = "PlayerScore";

    public override void Awake()
    { 
        //if there's not an instance, it creates one
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
            
        //StartClientService();

    }


    public override void Start()
    {
        if (SceneManager.GetActiveScene().name == "Main menu")
            SetState(mainMenuState);
        else if (SceneManager.GetActiveScene().name == "Shop")
            SetState(shopState);
        else if (SceneManager.GetActiveScene().name == "Leaderboard2")
            SetState(leaderboardState);
        else
            SetState(playState);
        
    }
    public void ClickButton(string buttonName)
    {
        // Send button
        ButtonClicked?.Invoke(this, buttonName);

        switch (buttonName)
        {
            case "Play":
                SceneManager.LoadScene("Gameplay 1");
                break;
            case "Pause":
                SwitchState(pauseState);
                break;
            case "Resume":
                SwitchState(playState);
                break;
            case "Retry":
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case "Main menu":
                SceneManager.LoadScene("Main menu");
                break;
            case "Leaderboard":
                // SwitchState(endState);
                SceneManager.LoadScene("Leaderboard2");
                OpenLeaderboards();
                break;
            case "Credits":
                SwitchState(creditsState);
                break;
            case "Settings":
                SwitchState(settingsState);
                break;
            case "Shop":
                SceneManager.LoadScene("Shop");
                break;
            case "Quit":
                Debug.Log("Quit");
                Application.Quit();
                break;
            default:
                break;
        }
    }

  private void OpenLeaderboards()
    {//funcion con idea de hacer un swithc que diferencie los diferentes rankings, de momento solo hay uno
        PanelManager.Open("LeaderboardElapsedTime");
    }
    /*
    /// <summary>
    /// starts the authentification function for leaderbaord and users
    /// </summary>
    public async void StartClientService()
    {
        PanelManager.CloseAll();
        PanelManager.Open("loading");
        try
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                var options = new InitializationOptions();
                options.SetProfile("default_profile");
                await UnityServices.InitializeAsync();
            }

            if (!eventsInitialized)
            {
                SetupEvents();
            }

            if (AuthenticationService.Instance.SessionTokenExists)
            {
                SignInAnonymouslyAsync();
            }
            else
            {
                PanelManager.Open("auth");
            }
        }
        catch (Exception exception)
        {
            ShowError(ErrorMenu.Action.StartService, "Failed to connect to the network.", "Retry");
        }
    }*/

    /////////////////////////////////////////////////////////////////////////////////////////////
    public void DestroyGO(GameObject gameObject){
        Destroy(gameObject);
    }

    public GameObject InstantiateGO(GameObject prefab, Vector3 position, Quaternion rotation, Transform bodyPartTransform)
    {
        return Instantiate(prefab, position, rotation, bodyPartTransform);
    }
}
