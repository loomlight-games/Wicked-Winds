using UnityEngine;
using TMPro;
using System;

public class GamePlayState : AState
{
    public TextMeshProUGUI feedBackText;

    GameObject UI, statesUI, gameplayUI, hud;
    TextMeshProUGUI timerText, elapsedText, speedText, flyHighText;
    float elapsedTime, remainingTime;
    int timerMinutes, timerSeconds, elapsedMinutes, elapsedSeconds;
    bool gameOverTriggered = false; //in order to not recall the method

    public override void Enter()
    {
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
        speedText = GameObject.Find("Speed amount").GetComponent<TextMeshProUGUI>();
        flyHighText = GameObject.Find("Fly amount").GetComponent<TextMeshProUGUI>();
        feedBackText = GameObject.Find("Feedback").GetComponent<TextMeshProUGUI>();
        
        // Needs to know boost value
        PlayerManager.Instance.MissionCompleteEvent += OnMissionCompleteEvent;
        
        ///////////////////////////////////////////////////////////////////////////////////
        if (GameManager.Instance.generateTown) GameManager.Instance.townGenerator.Start();
        ///////////////////////////////////////////////////////////////////////////////////
    }

    public override void Update()
    {
        UpdateTimer();

        if (PlayerManager.Instance.playerController.speedPotionValue >= 0)
            speedText.text = Mathf.FloorToInt(PlayerManager.Instance.playerController.speedPotionValue).ToString();
        else
            speedText.text = "0";
        
        if (PlayerManager.Instance.playerController.flyPotionValue >= 0)
            flyHighText.text = Mathf.FloorToInt(PlayerManager.Instance.playerController.flyPotionValue).ToString();
        else
            flyHighText.text = "0";

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
    /// Called when boost value is modified (only when running)
    /// </summary>
    private void OnBoostChangeEvent(object sender, float currentBoost)
    {
        speedText.text = Mathf.FloorToInt(currentBoost).ToString();
    }

    /// <summary>
    /// Called when a mission is completed. Adds time.
    /// </summary>
    private void OnMissionCompleteEvent(object sender, EventArgs e)
    {
        remainingTime += 20f;
    }
}
