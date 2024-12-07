using System;
using System.Collections;
using System.Collections.Generic;
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
    public event EventHandler<string> TownSelected;

    public bool playingOnPC = false;

    #region STATES
    public readonly GamePauseState pauseState = new();
    public readonly MainMenuState mainMenuState = new();
    public readonly GamePlayState playState = new();
    public readonly FinalState endState = new();
    public readonly CreditsState creditsState = new();
    public readonly SettingsState settingsState = new();
    public readonly ShopState shopState = new();
    public readonly LeaderboardGameState leaderboardState = new();
    public readonly TownSelectionState selectTownState = new();
    #endregion

    #region SUB-MANAGERS
    public TownGenerator townGenerator = new();
    #endregion

    #region CLOUD SERVICES
    bool eventsInitialized = false;
    [HideInInspector] public readonly string PLAYER_USERNAME_FILE = "PlayerUsername";
    [HideInInspector] public readonly string PLAYER_SCORE_FILE = "PlayerScore";
    [HideInInspector] public readonly string PLAYER_MISSIONTIME_FILE = "PlayerMisionTime"; //average mision/time
    [HideInInspector] public readonly string PLAYER_MISSIONCOUNT_FILE = "PlayerMisionCount"; //number of missions completed
    #endregion

    #region PROPERTIES
    [Header("Town generator")]
    public float tileSize = 50f;
    public int townSize = 4; // In tiles
    public TownGenerator.Town town;

    [Header("Stardust Town")]
    public GameObject landscape1;
    public List<GameObject> townTiles1 = new();
    [Header("Sandy Landy")]
    public GameObject landscape2;
    public List<GameObject> townTiles2 = new();
    [Header("Frostpeak Hollow")]
    public GameObject landscape3;
    public List<GameObject> townTiles3 = new();

    [Header("Gameplay")]
    public float initialTime = 120f;
    public float remainingTime;
    public List<float> missionsTimes = new();
    public float missionTime = 0;
    public float averageMissionTime = 0;
    #endregion

    [HideInInspector] public string sceneToLoad = "Gameplay";

    public override void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
            Destroy(gameObject);

        StartClientService();

        remainingTime = initialTime;
    }


    public override void Start()
    {
        // Initialize state based on the currently active scene
        SetStateBasedOnScene(SceneManager.GetActiveScene());
    }

    private void SetStateBasedOnScene(Scene scene)
    {
        if (scene.name == "Main menu")
        {
            SoundManager.PlaySound(SoundType.MenuMusic);
            SetState(mainMenuState);
        }
        else if (scene.name == "Shop")
        {
            SoundManager.PlaySound(SoundType.MenuMusic);
            SetState(shopState);
        }
        else if (scene.name == "Leaderboard")
        {
            SoundManager.PlaySound(SoundType.MenuMusic);
            SetState(leaderboardState);
        }
        else if (scene.name == "Gameplay")
        {
            SoundManager.PlaySound(SoundType.GameplayMusic);
            RandomHour();
            remainingTime = initialTime;
            townGenerator.GenerateTown();
            SetState(playState);
        }
        else
        {
            SoundManager.PlaySound(SoundType.GameplayMusic);
            RandomHour();
            remainingTime = initialTime;
            SetState(playState);
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Set the state based on the newly loaded scene
        SetStateBasedOnScene(scene);

        // Unsubscribe from the event to avoid multiple calls
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void ClickButton(string buttonName)
    {
        Debug.Log(buttonName);

        // Play sound effect
        SoundManager.PlaySound(SoundType.ButtonClick);

        // Send button
        TownSelected?.Invoke(this, buttonName);

        switch (buttonName)
        {
            case "Start":
                SwitchState(selectTownState);
                break;
            case "Play":
                LoadSceneDirectly("Gameplay");
                break;
            case "Replay":
                LoadSceneDirectly(SceneManager.GetActiveScene().name);
                break;
            case "Pause":
                SwitchState(pauseState);
                break;
            case "Return":
                ReturnToPreviousState();
                break;
            case "Resume":
                SwitchState(playState);
                break;
            case "Credits":
                SwitchState(creditsState);
                break;
            case "Settings":
                SwitchState(settingsState);
                break;
            case "Main menu leaderboard":
                AuthenticationService.Instance.SignOut();
                LoadSceneDirectly("Main menu");
                break;
            case "Main menu":
                LoadSceneDirectly("Main menu");
                break;
            case "Leaderboard":
                LoadSceneDirectly("Leaderboard");
                break;
            case "Shop":
                LoadSceneDirectly("Shop");
                break;
            case "Playing on PC":
                playingOnPC = !playingOnPC;
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
        catch (Exception)
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
        catch (AuthenticationException)
        {
            ShowError(ErrorPanel.Action.OpenAuthMenu, "Failed to sign in.", "OK");
        }
        //somethings wrong with the connection
        catch (RequestFailedException)
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
        catch (AuthenticationException)
        {
            ShowError(ErrorPanel.Action.OpenAuthMenu, "Username or password is wrong.", "OK");
        }
        catch (RequestFailedException)
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
        catch (AuthenticationException)
        {
            ShowError(ErrorPanel.Action.OpenAuthMenu, "Failed to sign you up.", "OK");
        }
        catch (RequestFailedException)
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
                string username = PlayerPrefs.GetString(GameManager.Instance.PLAYER_USERNAME_FILE, "PlayerU");

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

    /// <summary>
    /// For scenes that take time to load - through loading screen
    /// </summary>
    void LoadingSceneScreen(string sceneName)
    {
        sceneToLoad = sceneName;
        SceneManager.LoadScene("Loading screen");
    }

    /// <summary>
    /// For scenes that don't take time to load
    /// </summary>
    void LoadSceneDirectly(string sceneName)
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);
    }

    public void DestroyGO(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    public GameObject InstantiateGO(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
    {
        return Instantiate(prefab, position, rotation, parent);
    }
    public GameObject InstantiateGO(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        return Instantiate(prefab, position, rotation);
    }

    private void RandomHour()
    {
        // Finds game object with Sun tag
        GameObject sun = GameObject.FindGameObjectWithTag("Sun");
        if (sun == null)
        {
            Debug.LogWarning("Sun object not found!");
            return;
        }

        // Random value from 15 to 60 - X axis
        float randomX = UnityEngine.Random.Range(15f, 60f);

        // Random value from 0 to 360 - Y axis
        float randomY = UnityEngine.Random.Range(0f, 360f);

        // Apply new rotations to Sun
        sun.transform.rotation = Quaternion.Euler(randomX, randomY, sun.transform.rotation.eulerAngles.z);
    }

    public void AddTime(int timeBonus)
    {
        remainingTime += timeBonus;

        playState.timerText.color = Color.green;

        // Start the scaling animation coroutine
        StartCoroutine(ScaleTimerText(1, Color.white));
    }

    public IEnumerator ScaleTimerText(int times, Color colorToReturn)
    {
        Transform timerText = playState.timerText.transform;

        Vector3 originalScale = timerText.localScale;
        Vector3 targetScale = originalScale * 1.5f; // Scale up by 20%
        float duration = 0.5f; // Duration of the scaling animation

        for (int i = 0; i < times; i++)
        {
            // Reset elapsed time for scaling up
            float elapsed = 0f;

            // Scale up
            while (elapsed < duration)
            {
                timerText.transform.localScale = Vector3.Lerp(originalScale, targetScale, Mathf.Clamp01(elapsed / duration));
                elapsed += Time.deltaTime;
                yield return null;
            }

            // Ensure the target scale is set
            timerText.transform.localScale = targetScale;

            // Reset elapsed time for scaling down
            elapsed = 0f;

            // Scale down
            while (elapsed < duration)
            {
                timerText.transform.localScale = Vector3.Lerp(targetScale, originalScale, Mathf.Clamp01(elapsed / duration));
                elapsed += Time.deltaTime;
                yield return null;
            }

            // Ensure the original scale is restored
            timerText.transform.localScale = originalScale;
        }

        playState.timerText.color = colorToReturn; // Reset color after scaling
    }
}
