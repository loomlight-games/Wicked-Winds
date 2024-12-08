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
        GameManager.Instance.SwitchState(GameManager.Instance.talkingState);

        //dialogueLines = new string[] { message };

        lineIndex = 0;

        Debug.LogWarning($"Starting dialogue with {name}.");

        nameText.text = name;

        // Divide message in lines separating by '\n'
        dialogueLines = message.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        Debug.LogWarning($"Lines in message: {dialogueLines.Length}");

        if (dialogueLines.Length == 0)
        {
            Debug.LogError("A message is required!");
            return;
        }

        //GameManager.Instance.StartCoroutine(TypeLine());
        //PrintLineA();
        PrintLine();
    }

    public void PrintLine()
    {
        // Not executing coroutine
        if (!isTyping)
        {
            SoundManager.PlaySound(SoundType.Dialogue);

            // Executes coroutine
            GameManager.Instance.StartCoroutine(TypeLine());

            // Line is completely printed
            if (messageText.text == dialogueLines[lineIndex])
            {
                // There're more lines
                if (lineIndex < dialogueLines.Length - 1)
                {
                    messageText.text = string.Empty; // Clears text
                    lineIndex++; // Next line
                }
            }
            else
            {
                //GameManager.Instance.StopAllCoroutines();

                // Ensures the line is properly printed
                messageText.text = dialogueLines[lineIndex];
            }
        }
    }

    void PrintLineA()
    {
        if (lineIndex < dialogueLines.Length - 1)
        {
            messageText.text = string.Empty; // Clears text
            GameManager.Instance.StartCoroutine(TypeLine());
            lineIndex++;
        }
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
