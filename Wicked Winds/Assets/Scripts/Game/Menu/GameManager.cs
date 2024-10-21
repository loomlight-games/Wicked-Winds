using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

// Manages the gameplay stages
public class GameManager : AStateController
{
    public static GameManager Instance; //only one GameManager in the game (singleton)
    public event EventHandler<string> ButtonClicked;

    #region STATES
    readonly GamePauseState pauseState = new();
    readonly mainMenuState mainMenuState = new();
    readonly GamePlayState playState = new();
    #endregion

    public override void Awake()
    { //if there's not an instance, it creates one
        // Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public override void Start()
    {
        //ifnot in main menu, it's in the playing state
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            SetState(mainMenuState);
        }
        else
        {
            SetState(playState);
        }
    }
    public void ClickButton(string buttonName)
    {
        ButtonClicked?.Invoke(this, buttonName);

        switch (buttonName)
        {
            case "PauseButton":
                SwitchState(pauseState);
                break;
            case "ResumeButton":
                Debug.Log("Resuming the game"); // <-- Confirma que el juego intenta reanudarse
                SwitchState(playState);
                break;
            case "Replay":
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case "Main menu":
                SceneManager.LoadScene("Main Menu");
                break;
            case "Quit":
                Debug.Log("Quit");
                Application.Quit();
                break;
            default:
                break;
        }
    }
}
