using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public GamePlayState playState = new();
    public readonly FinalState endState = new();
    public readonly CreditsState creditsState = new();
    public readonly SettingsState settingsState = new();
    public readonly ShopState shopState = new();
    public readonly LeaderboardGameState leaderboardState = new();
    #endregion

    #region CLOUD SERVICES
    bool eventsInitialized = false;
    [HideInInspector] public readonly string PLAYER_USERNAME_FILE = "PlayerUsername";
    [HideInInspector] public readonly string PLAYER_SCORE_FILE = "PlayerScore";
    #endregion

    #region PROPERTIES
    [Header("Gameplay")]
    public float remainingTime;
    #endregion

    public override void Awake()
    { 

        //if there's not an instance, it creates one
        // Singleton
        if (Instance == null)
            Instance = this;
        
        else
            Destroy(gameObject);

        StartClientService();
    }


    public override void Start()
    {
        if (SceneManager.GetActiveScene().name == "Main menu")
            SetState(mainMenuState);
        else if (SceneManager.GetActiveScene().name == "Shop")
            SetState(shopState);
        else if (SceneManager.GetActiveScene().name == "Leaderboard")
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
                SceneManager.LoadScene("Gameplay");
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
            case "Main menu Leaderboard":
                AuthenticationService.Instance.SignOut();
                SceneManager.LoadScene("Main menu");
                break;
            case "Leaderboard":
                SceneManager.LoadScene("Leaderboard");
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
            default:
                break;
        }
    }
 
   /////////////////////////////////////////////////////////////////////////////////////////
   #region UNITY SERVICES ZONE (LEADERBOARD)
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
                AuthenticationService.Instance.SignOut();
                AuthenticationService.Instance.ClearSessionToken();
                //if user already sign in
                
                //SignInAnonymouslyAsync();
                
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

    public async void SignInWithUsernameAndPasswordAsync(string username, string password)
    {
        PanelManager.Open("loading");
        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            
            
        }
        catch (AuthenticationException exception)
        {
            ShowError(ErrorPanel.Action.OpenAuthMenu, "Username or password is wrong.", "OK");
        }
        catch (RequestFailedException exception)
        {
            ShowError(ErrorPanel.Action.OpenAuthMenu, "Failed to connect to the network.", "OK");
        }
    }

    public async void SignUpWithUsernameAndPasswordAsync(string username, string password)
    {
        PanelManager.Open("loading");
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            string username1 = PlayerPrefs.GetString(GameManager.Instance.PLAYER_USERNAME_FILE, "PlayerU");
            await AuthenticationService.Instance.UpdatePlayerNameAsync(username1);
        }
        catch (AuthenticationException exception)
        {
            ShowError(ErrorPanel.Action.OpenAuthMenu, "Failed to sign you up.", "OK");
        }
        catch (RequestFailedException exception)
        {
            ShowError(ErrorPanel.Action.OpenAuthMenu, "Failed to connect to the network.", "OK");
        }
    }
    public void SignOut()
    {
        AuthenticationService.Instance.SignOut();
        Debug.Log("cierra sesion");
        PanelManager.CloseAll();
        SceneManager.LoadScene("Main menu");
        //PanelManager.Open("auth");
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
        /*AuthenticationService.Instance.SignInFailed += (err) => {
            Debug.LogError(err);
            AuthenticationService.Instance.SignOut();
        };*/

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
                string username =PlayerPrefs.GetString(GameManager.Instance.PLAYER_USERNAME_FILE, "PlayerU");

                await AuthenticationService.Instance.UpdatePlayerNameAsync(username);
            }
            PanelManager.CloseAll();
            PanelManager.Open("profile");
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
    #endregion
    /////////////////////////////////////////////////////////////////////////////////////////////

    public void DestroyGO(GameObject gameObject){
        Destroy(gameObject);
    }

    public GameObject InstantiateGO(GameObject prefab, Vector3 position, Quaternion rotation, Transform bodyPartTransform)
    {
        return Instantiate(prefab, position, rotation, bodyPartTransform);
    }
}
