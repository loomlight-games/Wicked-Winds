using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class GamePlayState : AState
{
    GameObject UI, statesUI, gameplayUI, handledControls;
    public TextMeshProUGUI timerText, coinsNumText;
    HUDBar highSpeedBar, flyHighBar;
    float elapsedTime, remainingTime;
    int timerMinutes, timerSeconds, coinsNum;
    bool gameOverTriggered = false, //in order to not recall the method
        isScalingRed = false,
        isScalingYellow = false;

    public override void Enter()
    {
        Time.timeScale = 1f; // Resumes simulation

        UI = GameObject.Find("Game UI");

        statesUI = UI.transform.Find("States").gameObject;
        statesUI.SetActive(true);
        gameplayUI = statesUI.transform.Find("Gameplay").gameObject;
        gameplayUI.SetActive(true);

        timerText = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        coinsNumText = GameObject.Find("Coin number").GetComponent<TextMeshProUGUI>();

        highSpeedBar = GameObject.Find("High speed bar").GetComponent<HUDBar>();
        flyHighBar = GameObject.Find("Fly high bar").GetComponent<HUDBar>();
        handledControls = UI.transform.Find("Handled controls").gameObject;

        // Don't show handled controls in PC
        if (GameManager.Instance.playingOnPC)
            handledControls.SetActive(false);
        else // Show them in other device type
            handledControls.SetActive(true);

        gameOverTriggered = false;
    }

    public override void Update()
    {
        coinsNumText.text = PlayerManager.Instance.customizable.coins.ToString();

        // Not if player is talking
        if (PlayerManager.Instance.GetState() == PlayerManager.Instance.talkingState)
            return;

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

        SoundManager.StopSoundEffect(); // Stops dialogue sound
    }

    private void UpdateTimer()
    {
        remainingTime = GameManager.Instance.remainingTime;

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            elapsedTime += Time.deltaTime;

            if (remainingTime <= 31 && !isScalingRed)
            {
                timerText.color = Color.red;
                isScalingRed = true; // Set the flag to true to prevent further calls
                GameManager.Instance.StartCoroutine(GameManager.Instance.ScaleTimerText(3, timerText.color));
            }
            else if (remainingTime <= 61 && !isScalingYellow && remainingTime > 31)
            {
                timerText.color = Color.yellow;
                isScalingYellow = true; // Set the flag to true to prevent further calls
                GameManager.Instance.StartCoroutine(GameManager.Instance.ScaleTimerText(3, timerText.color));
            }
        }
        else if (remainingTime <= 0 && !gameOverTriggered)
        {
            remainingTime = 0;
            TriggerGameOver(); //switching states
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

        if (MissionManager.Instance.missionsCompleted >= 10)
        {
            // Suma todos los tiempos de la lista
            float totalMissionTime = GameManager.Instance.missionsTimes.Sum();

            // Calcula la media dividiendo entre las misiones completadas
            GameManager.Instance.averageMissionTime = totalMissionTime / MissionManager.Instance.missionsCompleted;
        }
        else
        {
            GameManager.Instance.averageMissionTime = 0;
            Debug.Log(" No has hecho mas de 10 misiones");
        }

        PlayerManager.Instance.SwitchState(PlayerManager.Instance.finalState);
        GameManager.Instance.SwitchState(GameManager.Instance.endState);
    }
}
