using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TalkingState : AState
{
    GameObject dialoguePanel;

    public override void Enter()
    {
        Time.timeScale = 0f; // Stops simulation

        GameObject UI = GameObject.Find("Game UI");
        dialoguePanel = UI.transform.Find("Dialogue panel").gameObject;

        dialoguePanel.SetActive(true); // Show dialogue panel

        GameObject messageGO = dialoguePanel.transform.Find("Message").gameObject;
        GameObject nameGO = dialoguePanel.transform.Find("Name").gameObject;

        TextMeshProUGUI messageText = messageGO.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI nameText = nameGO.GetComponent<TextMeshProUGUI>();

        // Assign texts to dialogue
        GameManager.Instance.dialogue.Initialize(messageText, nameText);
    }

    public override void Update()
    {
        // Interact key is pressed
        if (PlayerManager.Instance.interactKey)
            GameManager.Instance.dialogue.PrintLine();
    }

    public override void Exit()
    {
        dialoguePanel.SetActive(false); // Hide dialogue panel

        Time.timeScale = 1f; // Reactivates simulation
    }
}
