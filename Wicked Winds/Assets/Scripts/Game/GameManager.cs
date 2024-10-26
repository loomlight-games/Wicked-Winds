using System;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.Rendering.DebugUI.Table;

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

    #region CLOUD SERVICES
    private bool eventsInitialized = false;

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

        StartClientService();
        //PanelManager.Open("Leaderboard");
  
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
 
   /////////////////////////////////////////////////////////////////////////////////////////
   //UNITY SERVICES ZONE (LEADERBOARD)
  private void OpenLeaderboards()
    {//funcion con idea de hacer un swithc que diferencie los diferentes rankings, de momento solo hay uno
        PanelManager.Open("LeaderboardElapsedTime");
    }
    
    /// <summary>
    /// starts the authentification function for leaderbaord and users
    /// </summary>
    public async void StartClientService()
    {
        PanelManager.CloseAll();
        PanelManager.Open("loading");
        try
        {   // check if the service is inicialized
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                var options = new InitializationOptions();
                options.SetProfile("default_profile");
                await UnityServices.InitializeAsync();
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
            
            if (!eventsInitialized)
            {
                SetupEvents();
            }

            //to avoid repeating the authentificaction process
            if (AuthenticationService.Instance.SessionTokenExists)
            {
                //if user already sign in
                Debug.Log("session token exists");
                SignInAnonymouslyAsync();
            }
            else
            {
                PanelManager.Open("auth");
            }
        }
        catch (Exception exception)
        {
            ShowError(ErrorPanel.Action.StartService, "Failed to connect to the network.", "Retry");
        }
    }


    /// <summary>
    ///  Sign in without username
    ///  if you already log in w username (you have a session token) it loges automatically w that provider
    /// </summary>
    public async void SignInAnonymouslyAsync()
    {
        PanelManager.Open("loading");
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        // something is wrong with the credentials
        catch (AuthenticationException exception)
        {
            ShowError(ErrorPanel.Action.OpenAuthMenu, "Failed to sign in.", "OK");
        }
        //somethings wrong with the connection
        catch (RequestFailedException exception)
        {
            ShowError(ErrorPanel.Action.SignIn, "Failed to connect to the network.", "Retry");
        }
    }

    public void SingOut()
    {
        AuthenticationService.Instance.SignOut();
        PanelManager.CloseAll();
        PanelManager.Open("auth");
    }

    private void SetupEvents()
    {
        eventsInitialized = true;
        AuthenticationService.Instance.SignedIn += () =>
        {
            SignInConfirmAsync();
            // Shows how to get a playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
            // Shows how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");

        };
        AuthenticationService.Instance.SignInFailed += (err) => {
            Debug.LogError(err);
        };

        AuthenticationService.Instance.SignedOut += () =>
        {
            PanelManager.CloseAll();
            PanelManager.Open("auth");
            Debug.Log("Player signed out.");
        };
        
        AuthenticationService.Instance.Expired += () =>
        {
            SignInAnonymouslyAsync();
            Debug.Log("Player session could not be refreshed and expired.");
        };
    }

    private async void SignInConfirmAsync()
    {
        try
        {   
            if (string.IsNullOrEmpty(AuthenticationService.Instance.PlayerName))
            {
                await AuthenticationService.Instance.UpdatePlayerNameAsync("Player");
            }
            PanelManager.CloseAll();
            PanelManager.Open("main");
        }
        catch
        {

        }
    }
    private void ShowError(ErrorPanel.Action action = ErrorPanel.Action.None, string error = "", string button = "")
    {
        PanelManager.Close("loading");
        ErrorPanel panel = (ErrorPanel)PanelManager.GetSingleton("error");
        panel.Open(action, error, button);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////

    public void DestroyGO(GameObject gameObject){
        Destroy(gameObject);
    }

    public GameObject InstantiateGO(GameObject prefab, Vector3 position, Quaternion rotation, Transform bodyPartTransform)
    {
        return Instantiate(prefab, position, rotation, bodyPartTransform);
    }
}
