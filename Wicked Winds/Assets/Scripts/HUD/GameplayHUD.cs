using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayHUD : MonoBehaviour
{
    public TextMeshProUGUI timerText, elapsedText, boostText;
    public float remainingTime, elapsedTime;
    int timerMinutes, timerSeconds, elapsedMinutes, elapsedSeconds;
    public GameObject player;
    void Awake()
    {
        // Needs to know boost value
        Boostable boostable = player.GetComponent<Boostable>();
        boostable.BoostValueEvent += OnBoostChangeEvent;
    }

    // Update is called once per frame
    void Update()
    {
        if (remainingTime > 0){
            remainingTime -= Time.deltaTime;
            elapsedTime += Time.deltaTime;
        }else if (remainingTime < 0){
            remainingTime = 0;

            // GAMEOVER

            timerText.color = Color.red;
        }

        timerMinutes = Mathf.FloorToInt(remainingTime / 60);
        timerSeconds = Mathf.FloorToInt(remainingTime % 60);
        elapsedMinutes = Mathf.FloorToInt(elapsedTime / 60);
        elapsedSeconds = Mathf.FloorToInt(elapsedTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", timerMinutes, timerSeconds);
        elapsedText.text = string.Format("{0:00}:{1:00}", elapsedMinutes, elapsedSeconds);
    }

    /// <summary>
    /// Called when boost value is modified (only when running)
    /// </summary>
    private void OnBoostChangeEvent(object sender, float currentBoost)
    {
        boostText.text = Mathf.FloorToInt(currentBoost).ToString();
    }
}
