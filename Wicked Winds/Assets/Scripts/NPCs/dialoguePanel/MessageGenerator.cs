using UnityEngine;
using System.Collections.Generic;

public class MessageGenerator
{
    public string GenerateMessage(MissionData mission, NPC currentNPC)
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

        // Check if the selected message template contains {NPC_NAME}
        if (randomMessageTemplate.Contains("{NPC_NAME}"))
        {
            // Get a list of all NPCs in the scene
            NPC[] allNPCs = GetAllNPCs();

            // Create a list to hold NPC names, excluding the current NPC
            List<string> npcNames = new List<string>();

            foreach (var npc in allNPCs)
            {
                if (npc != currentNPC) // Exclude the current NPC
                {
                    npcNames.Add(npc.name); // Add the name to the list
                }
            }

            // Select a random NPC name from the list, if available
            if (npcNames.Count > 0)
            {
                string randomNPCName = npcNames[Random.Range(0, npcNames.Count)];
                randomMessageTemplate = randomMessageTemplate.Replace("{NPC_NAME}", randomNPCName);
            }
            else
            {
                Debug.LogWarning("No other NPCs available to replace {NPC_NAME}.");
            }
        }

        return randomMessageTemplate; // Return the message (with or without NPC name)
    }


    public static NPC[] GetAllNPCs()
    {
        return GameObject.FindObjectsOfType<NPC>(); // Asegúrate de usar el tipo correcto aquí
    }
}
