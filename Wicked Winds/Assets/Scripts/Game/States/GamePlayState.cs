using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class GamePlayState : AState
{
    public TextMeshProUGUI feedBackText;

    GameObject UI, statesUI, gameplayUI, handledControls;
    TextMeshProUGUI timerText;
    HUDBar highSpeedBar, flyHighBar;
    float elapsedTime, remainingTime;
    int timerMinutes, timerSeconds;
    bool gameOverTriggered = false; //in order to not recall the method

    public override void Enter()
    {
        Debug.LogWarning("GamePlayState");

        Time.timeScale = 1f; // Resumes simulation

        UI = GameObject.Find("Game UI");

        statesUI = UI.transform.Find("States").gameObject;
        statesUI.SetActive(true);
        gameplayUI = statesUI.transform.Find("Gameplay").gameObject;
        gameplayUI.SetActive(true);

        timerText = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        feedBackText = GameObject.Find("Feedback").GetComponent<TextMeshProUGUI>();
        highSpeedBar = GameObject.Find("High speed bar").GetComponent<HUDBar>();
        flyHighBar = GameObject.Find("Fly high bar").GetComponent<HUDBar>();
        handledControls = UI.transform.Find("Handled controls").gameObject;
        
        // Don't show handled controls in PC
        if (GameManager.Instance.playingOnPC)
            handledControls.SetActive(false);
        else // Show them in other device type
            handledControls.SetActive(true);

        

        SoundManager.Instance.PlayGamePlayMusic();

        gameOverTriggered = false;
    }

    public override void Update()
    {
        UpdateTimer();

        if (PlayerManager.Instance.hasActiveMission)
        {
            UpdateMissionTime();
        }

        if (PlayerManager.Instance.playerController.speedPotionValue >= 0)
            highSpeedBar.SetValue(PlayerManager.Instance.playerController.speedPotionValue);
        else
            highSpeedBar.SetValue(0);

        if (PlayerManager.Instance.playerController.flyPotionValue >= 0)
            flyHighBar.SetValue(PlayerManager.Instance.playerController.flyPotionValue);
        else
            flyHighBar.SetValue(0);

    }

    public override void Exit()
    {
        gameplayUI.SetActive(false);
    }

    private void UpdateTimer()
    {
        remainingTime = GameManager.Instance.remainingTime;

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            elapsedTime += Time.deltaTime;
        }
        else if (remainingTime <= 0 && !gameOverTriggered)
        {
            remainingTime = 0;

            // GAMEOVER when remaining time is over
            TriggerGameOver(); //switching states
            timerText.color = Color.red;
        }

        timerMinutes = Mathf.FloorToInt(remainingTime / 60);
        timerSeconds = Mathf.FloorToInt(remainingTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", timerMinutes, timerSeconds);

        GameManager.Instance.remainingTime = remainingTime;
    }

    private void UpdateMissionTime()
    {

        if (remainingTime > 0)
        {
            GameManager.Instance.missionTime += Time.deltaTime;
        }
    }
    void TriggerGameOver()
    {
        gameOverTriggered = true;  // avoids double calls

        PlayerManager.Instance.score = (int)elapsedTime;
        // Suma todos los tiempos de la lista
        float totalMissionTime = GameManager.Instance.missionsTimes.Sum();

        // Calcula la media dividiendo entre las misiones completadas
        GameManager.Instance.averageMissionTime = totalMissionTime / MissionManager.Instance.missionsCompleted;
        PlayerManager.Instance.SwitchState(PlayerManager.Instance.finalState);
        GameManager.Instance.SwitchState(GameManager.Instance.endState);

        timerText.color = Color.red;
    }

    
}
