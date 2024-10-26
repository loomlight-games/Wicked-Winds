using UnityEngine;
using System.Collections.Generic;

public class MessageGenerator
{
    public string GenerateMessage(MissionData mission, NPC currentNPC, MissionIcon missionIcon)
    {
        // Check if the mission has messages
        if (mission.npcMessages == null || mission.npcMessages.Count == 0)
        {
            Debug.LogWarning("No messages available for this mission.");
            return "No message available.";
        }

        // Select a random message template based on the mission type
        string[] messageTemplates;

        switch (mission.missionName.ToLower()) // Use .ToLower() to ensure case-insensitive comparison
        {
            case "lettermision":
                messageTemplates = mission.npcMessages["letter"];
                break;
            case "catmission":
                messageTemplates = mission.npcMessages["cat"];
                break;
            case "potionmission":
                messageTemplates = mission.npcMessages["potion"];
                break;
            default:
                Debug.LogWarning("Unknown mission type.");
                return "No message available.";
        }

        // Select a random message template
        string randomMessageTemplate = messageTemplates[Random.Range(0, messageTemplates.Length)];

     
        return randomMessageTemplate; // Return the message (with or without NPC name)
    }
}


        
