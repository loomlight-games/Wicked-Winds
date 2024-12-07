using UnityEngine;

public class MessageGenerator
{
    public (string, string) GenerateMessage(MissionData mission, NPC currentNPC, MissionIcon missionIcon)
    {
        // Check if the mission has messages
        if (mission.npcMessages == null || mission.npcMessages.Count == 0)
        {
            Debug.LogWarning("No messages available for this mission.");
            return ("No message available.", "No response available.");
        }

        // Select a random message template based on the mission type
        string[] messageTemplates;
        string[] answerTemplates;

        switch (mission.type.ToLower()) // Use .ToLower() to ensure case-insensitive comparison
        {
            case "lettermision":
                messageTemplates = mission.npcMessages["LetterMision"];
                answerTemplates = mission.npcAnswers["LetterMision"];
                break;
            case "catmission":
                messageTemplates = mission.npcMessages["CatMission"];
                answerTemplates = mission.npcAnswers["CatMission"];
                break;
            case "potionmission":
                messageTemplates = mission.npcMessages["PotionMission"];
                answerTemplates = mission.npcAnswers["PotionMission"];
                break;
            case "owlmission":
                messageTemplates = mission.npcMessages["OwlMission"];
                answerTemplates = mission.npcAnswers["OwlMission"];
                break;
            default:
                Debug.LogWarning("Unknown mission type.");
                return ("No message available.", "No response available.");
        }

        // Select a random index
        int randomIndex = Random.Range(0, messageTemplates.Length);

        // Get the random message and the corresponding answer
        string randomMessageTemplate = messageTemplates[randomIndex];
        string responseMessage = answerTemplates[randomIndex];

        return (randomMessageTemplate, responseMessage); // Return the message and the response
    }
}
