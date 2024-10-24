using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class FinalState : AState
{
    GameObject finalMenu, defeat, retryButton, mainMenuButton, UI, statesUI;
    TextMeshProUGUI scoreText;

    public override void Enter()
    {
        UI = GameObject.Find("UI");

        statesUI = UI.transform.Find("States").gameObject;
        finalMenu = statesUI.transform.Find("FinalMenu").gameObject;
        defeat = finalMenu.transform.Find("Defeat").gameObject;
        retryButton = finalMenu.transform.Find("RetryButton").gameObject;
        mainMenuButton = finalMenu.transform.Find("MainMenuButton").gameObject;
        scoreText = finalMenu.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();

        // Mostrar el menú de final de juego
        finalMenu.SetActive(true);
        defeat.SetActive(true);
        retryButton.SetActive(true);
        mainMenuButton.SetActive(true);

        // Mostrar la puntuación final
        float playerScore = GameManager.Instance.playerScore;
        scoreText.text = "Your Score: " + Mathf.FloorToInt(playerScore).ToString();


        Time.timeScale = 0f; // Pausar el juego
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) // Por ejemplo, reiniciar el juego
        {
            GameManager.Instance.ClickButton("Replay");
        }
    }

    public override void Exit()
    {
        Time.timeScale = 1f; // Reanudar el juego si es necesario cuando se salga de este estado
        finalMenu.SetActive(false);
    }
}

