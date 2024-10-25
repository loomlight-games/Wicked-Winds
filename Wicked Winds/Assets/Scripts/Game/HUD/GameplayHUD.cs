using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayHUD : MonoBehaviour
{
    public TextMeshProUGUI timerText, elapsedText, boostText;
    public float remainingTime, elapsedTime;
    int timerMinutes, timerSeconds, elapsedMinutes, elapsedSeconds;
    private bool gameOverTriggered = false; //in order to not recall the method


    void Awake()
    {
        // Needs to know boost value
        PlayerManager.Instance.boostable.BoostValueEvent += OnBoostChangeEvent;
    }

    // Update is called once per frame
    void Update()
    {
        if (remainingTime > 0){

            remainingTime -= Time.deltaTime;
            elapsedTime += Time.deltaTime;
        }else if (remainingTime <= 0 && !gameOverTriggered)
        {
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
    }

    void TriggerGameOver()
    {
        gameOverTriggered = true;  // avoids double calls

        // Guardar el tiempo transcurrido como puntuaci�n en PlayerPrefs
        PlayerPrefs.SetFloat("PlayerScore", elapsedTime);
        PlayerPrefs.Save();  // Asegurarse de que el valor se guarda correctamente
        
        // Env�a la puntuaci�n al ScoreManager (pasa elapsedTime)
        /*if (ScoreManager.Instance != null)
        {
            Debug.Log("scoremanager is not nuuuuull");
            ScoreManager.Instance.UpdateScore((int)elapsedTime);
            ScoreManager.Instance.SubmitScore();  // Submit the score
        }
        */

        GameManager.Instance.GameOver(elapsedTime);
        // switch to defeat state
        GameManager.Instance.SwitchState(GameManager.Instance.endState);
        timerText.color = Color.red;
    }
    /// <summary>
    /// Called when boost value is modified (only when running)
    /// </summary>
    private void OnBoostChangeEvent(object sender, float currentBoost)
    {
        boostText.text = Mathf.FloorToInt(currentBoost).ToString();
    }
}
