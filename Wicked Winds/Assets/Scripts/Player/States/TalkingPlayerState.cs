using TMPro;
using UnityEngine;

public class TalkingPlayerState : AState
{
    GameObject dialoguePanel;

    public override void Enter()
    {
        GameObject UI = GameObject.Find("HUD");
        dialoguePanel = UI.transform.Find("Dialogue panel").gameObject;

        dialoguePanel.SetActive(true); // Show dialogue panel

        GameObject messageGO = dialoguePanel.transform.Find("Message").gameObject;
        GameObject nameGO = dialoguePanel.transform.Find("Name").gameObject;

        TextMeshProUGUI messageText = messageGO.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI nameText = nameGO.GetComponent<TextMeshProUGUI>();

        // Assign texts to dialogue
        GameManager.Instance.dialogue.Initialize(messageText, nameText);

        // Plays dialogue sound
        SoundManager.PlaySound(SoundType.Dialogue);
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
        SoundManager.StopSoundEffect(); // Stops dialogue sound
    }
}
