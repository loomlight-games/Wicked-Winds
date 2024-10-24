using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Pipeline.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

// Manages the gameplay stages
public class GameManager : AStateController
{
    public static GameManager Instance; //only one GameManager in the game (singleton)
    public event EventHandler<string> ButtonClicked;
    public int playerScore;

    #region STATES
    readonly GamePauseState pauseState = new();
    readonly MainMenuState mainMenuState = new();
    readonly GamePlayState playState = new();
    public readonly FinalState endState = new();
    readonly CreditsState creditsState = new();
    readonly SettingsState settingsState = new();
    readonly ShopState shopState = new();
    #endregion

    public override void Awake()
    { 
        //if there's not an instance, it creates one
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
            
    }

    public void AddScore(float score)
    {
        playerScore += (int)score;
        Debug.Log("Puntuación actualizada en GameManager: " + playerScore);
    }

    public void SetScore(float score)
    {
        playerScore = (int)score;
        Debug.Log("Puntuación seteada en GameManager: " + playerScore);
    }

    public override void Start()
    {
        //if not in main menu, it's in the playing state
        if (SceneManager.GetActiveScene().name == "Main menu")
            SetState(mainMenuState);
        else if (SceneManager.GetActiveScene().name == "Shop")
            SetState(shopState);
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
            case "RetryButton":
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case "Main menu":
                SceneManager.LoadScene("Main menu");
                break;
            case "LeaderboardButton":
                SwitchState(endState);
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
            default:
                break;
        }
    }


    // end game 
    public void GameOver(float elapsedTime)
    {
        playerScore = (int)elapsedTime; // save played time as score
        SwitchState(endState); 
    }

}
