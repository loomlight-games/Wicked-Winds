using System.Collections;
using TMPro;
using UnityEngine;

public class Dialogue
{
    TextMeshProUGUI messageText, nameText;
    string[] dialogueLines;
    int lineIndex = 0;
    bool isTyping = false;

    public void Initialize(TextMeshProUGUI messageText, TextMeshProUGUI nameText)
    {
        this.messageText = messageText;
        this.nameText = nameText;
    }

    public void StartDialogue(string name, string message)
    {
        PlayerManager.Instance.SwitchState(PlayerManager.Instance.talkingState);

        dialogueLines = new string[] { message };

        lineIndex = 0;

        nameText.text = name;

        // Divide message in lines separating by '\n'
        dialogueLines = message.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        if (dialogueLines.Length == 0)
        {
            Debug.LogError("A message is required!");
            return;
        }

        PrintLine();
    }

    public void PrintLine()
    {
        // Not executing coroutine
        if (!isTyping)
        {
            // Line is completely printed
            if (messageText.text == dialogueLines[lineIndex])
            {
                // There're more lines
                if (lineIndex < dialogueLines.Length - 1)
                {
                    messageText.text = string.Empty; // Clears text
                    lineIndex++; // Next line
                }
                // No more lines
                else
                {
                    //Change to previous state
                    PlayerManager.Instance.ReturnToPreviousState();
                }
            }
            // Full line isn't printed yet
            else
            {
                SoundManager.PlaySound(SoundType.Dialogue);

                // Executes coroutine
                GameManager.Instance.StartCoroutine(TypeLine());
            }
        }
        else
            Debug.LogWarning("Typing");
    }

    IEnumerator TypeLine()
    {
        isTyping = true;

        foreach (char c in dialogueLines[lineIndex].ToCharArray())
        {
            messageText.text += c;
            yield return new WaitForSeconds(GameManager.Instance.speechSpeed);
        }

        isTyping = false;
    }
}
