using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdPanel : MonoBehaviour
{
    #region PROPERTIES
    public event EventHandler<int> EarnCoinsEvent;

    public const int COINS_TO_EARN = 3;
    int timerMinutes, timerSeconds;

    public const float SECONDS_FOR_REWARD = 16f;
    float secondsForReward = SECONDS_FOR_REWARD;

    public TextMeshProUGUI earnCoinsText, rewardText;
    public Button rewardButton, yesButton, noButton;
    public GameObject confirmationPanel;
    #endregion

    ///////////////////////////////////////////////////////////////

    // Start is called before the first frame update
    void Start()
    {
        earnCoinsText.text = "Earn " + COINS_TO_EARN + " coins";
    }

    // Update is called once per frame
    void Update()
    {
        // Only if watching ad
        if (gameObject.activeSelf && !confirmationPanel.activeSelf)
        {
            if (secondsForReward > 0.5)
            {
                rewardButton.interactable = false;

                secondsForReward -= Time.deltaTime;

                timerMinutes = Mathf.FloorToInt(secondsForReward / 60);
                timerSeconds = Mathf.FloorToInt(secondsForReward % 60);
                rewardText.text = string.Format("Reward in {0:00}:{1:00}", timerMinutes, timerSeconds);
            }
            else // Timer finished
            {
                rewardText.text = "Get reward!";
                rewardButton.interactable = true;

                secondsForReward = 0;
            }
        }
    }

    /// <summary>
    /// Invokes the event to earn coins
    /// </summary>
    public void GetReward()
    {
        // Invoke event
        EarnCoinsEvent?.Invoke(this, COINS_TO_EARN);

        // Close panel
        gameObject.SetActive(false);

        // Restart timer
        secondsForReward = SECONDS_FOR_REWARD;
    }

    /// <summary>
    /// Handles panel activation
    /// </summary>
    public void UpdatePanel()
    {
        // This is active -> trying to close
        if (gameObject.activeSelf)
            // Confirm before closing
            confirmationPanel.SetActive(true);
        // This is not active -> show panel
        else
        {
            gameObject.SetActive(true);
            confirmationPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Try to close panel
    /// </summary>
    public void ClosePanel()
    {
        // This is active -> trying to close
        if (gameObject.activeSelf)
            // Confirm before closing
            confirmationPanel.SetActive(true);
    }

    /// <summary>
    /// Shows confimation message before closing panel
    /// </summary>
    public void Confirm(string answer)
    {
        confirmationPanel.SetActive(false);

        if (answer == "Yes")
        {
            gameObject.SetActive(false);

            // Restart timer
            secondsForReward = SECONDS_FOR_REWARD;
        }
    }
}
