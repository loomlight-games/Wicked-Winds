using System;
using System.Collections.Generic;
using TMPro;
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
            Instance = this;
        else
            Destroy(gameObject);
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
            case "Leaderboard":
                // SwitchState(endState);
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
            case "Quit":
                Debug.Log("Quit");
                Application.Quit();
                break;
            case "Submit":
                if (currentState == leaderboardState)
                {
                    leaderboardState.SubmitScore();
                }
                break;
            default:
                break;
        }
    }

    /*
    // end game 
    public void GameOver(float elapsedTime)
    {
        playerScore = (int)elapsedTime; // save played time as score
        SwitchState(endState);
    }
    */

    /////////////////////////////////////////////////////////////////////////////////////////////
    public void DestroyGO(GameObject gameObject){
        Destroy(gameObject);
    }

    public GameObject InstantiateGO(GameObject prefab, Vector3 position, Quaternion rotation, Transform bodyPartTransform)
    {
        return Instantiate(prefab, position, rotation, bodyPartTransform);
    }
}
