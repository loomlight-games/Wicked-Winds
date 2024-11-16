using UnityEngine;
using TMPro;
using System;

public class GamePlayState : AState
{
    public TextMeshProUGUI feedBackText;

    GameObject UI, statesUI, gameplayUI, hud;
    TextMeshProUGUI timerText, elapsedText;
    HUDBar highSpeedBar, flyHighBar;
    float elapsedTime, remainingTime;
    int timerMinutes, timerSeconds, elapsedMinutes, elapsedSeconds;
    bool gameOverTriggered = false; //in order to not recall the method

    public override void Enter()
    {
        Debug.LogWarning("GamePlayState");

        Time.timeScale = 1f; // Resumes simulation

        UI = GameObject.Find("Game UI");

        hud = UI.transform.Find("HUD").gameObject;
        hud.SetActive(true);

        statesUI = UI.transform.Find("States").gameObject;
        statesUI.SetActive(true);
        gameplayUI = statesUI.transform.Find("Gameplay").gameObject;
        gameplayUI.SetActive(true);

        timerText = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        elapsedText = GameObject.Find("Elapsed time").GetComponent<TextMeshProUGUI>();
        feedBackText = GameObject.Find("Feedback").GetComponent<TextMeshProUGUI>();
        highSpeedBar =  GameObject.Find("High speed bar").GetComponent<HUDBar>();
        flyHighBar =  GameObject.Find("Fly high bar").GetComponent<HUDBar>();
        
        // Needs to know boost value
        PlayerManager.Instance.MissionCompleteEvent += OnMissionCompleteEvent;
        
        GameManager.Instance.townGenerator.Start();
    }

    public override void Update()
    {
        UpdateTimer();

        if (PlayerManager.Instance.playerController.speedPotionValue >= 0)
            highSpeedBar.SetValue(PlayerManager.Instance.playerController.speedPotionValue);
        else
            highSpeedBar.SetValue(0);
        
        if (PlayerManager.Instance.playerController.flyPotionValue >= 0)
            flyHighBar.SetValue(PlayerManager.Instance.playerController.flyPotionValue);
        else
            flyHighBar.SetValue(0);

        if (Input.GetKeyDown(KeyCode.Escape))
            GameManager.Instance.ClickButton("Pause");
    }

    public override void Exit()
    {
        hud.SetActive(false);
        gameplayUI.SetActive(false);
    }

    private void UpdateTimer()
    {
        remainingTime = GameManager.Instance.remainingTime;

        if (remainingTime > 0){
            remainingTime -= Time.deltaTime;
            elapsedTime += Time.deltaTime;
        }else if (remainingTime <= 0 && !gameOverTriggered){
            remainingTime = 0;

            // GAMEOVER when remaining time is over
            TriggerGameOver(); //switching states
            timerText.color = Color.red;
        }

        //update timer texts
        timerMinutes = Mathf.FloorToInt(remainingTime / 60);
        timerSeconds = Mathf.FloorToInt(remainingTime % 60);
        elapsedMinutes = Mathf.FloorToInt(elapsedTime / 60);
        elapsedSeconds = Mathf.FloorToInt(elapsedTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", timerMinutes, timerSeconds);
        elapsedText.text = string.Format("{0:00}:{1:00}", elapsedMinutes, elapsedSeconds);

        GameManager.Instance.remainingTime = remainingTime;
    }

    void TriggerGameOver()
    {
        gameOverTriggered = true;  // avoids double calls

        PlayerManager.Instance.score = (int) elapsedTime;

        PlayerManager.Instance.SwitchState(PlayerManager.Instance.finalState);
        GameManager.Instance.SwitchState(GameManager.Instance.endState);
        
        timerText.color = Color.red;
    }

    /// <summary>
    /// Called when a mission is completed. Adds time.
    /// </summary>
    private void OnMissionCompleteEvent(object sender, EventArgs e)
    {
        remainingTime += 20f;
    }
}
