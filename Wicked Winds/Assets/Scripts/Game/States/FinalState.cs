using TMPro;
using UnityEngine;

public class FinalState : AState
{    
    GameObject finalMenu, defeat, retryButton, mainMenuButton,leaderboardButton, UI, statesUI;
    TextMeshProUGUI scoreText;

    public override void Enter()
    {
        UI = GameObject.Find("Game UI");

        statesUI = UI.transform.Find("States").gameObject;
        finalMenu = statesUI.transform.Find("FinalMenu").gameObject;
        defeat = finalMenu.transform.Find("Game over").gameObject;
        retryButton = finalMenu.transform.Find("Replay").gameObject;
        mainMenuButton = finalMenu.transform.Find("Main menu").gameObject;
        leaderboardButton = finalMenu.transform.Find("Leaderboard").gameObject;
        scoreText = finalMenu.transform.Find("Score").GetComponent<TextMeshProUGUI>();

        // Mostrar el men� de final de juego
        finalMenu.SetActive(true);
        defeat.SetActive(true);
        retryButton.SetActive(true);
        mainMenuButton.SetActive(true);
        leaderboardButton.SetActive(true);

        // Mostrar la puntuaci�n final
        float playerScore = PlayerManager.Instance.score;
        scoreText.text = "Your total time: " + playerScore.ToString();


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

