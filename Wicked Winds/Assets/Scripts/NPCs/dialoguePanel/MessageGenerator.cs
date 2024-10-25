using UnityEngine;

public class MessageGenerator : MonoBehaviour
{
    private NPCNameManager nameManager; // Reference to the NPC manager
    private static MessageGenerator instance;
    public static MessageGenerator Instance { get { return instance; } } // con el patron singleton hacemos que 
    //solo tengamos una unica instancia de bulletpool y nos permite acceder más fácilmente a sus metodos
    // y campos desde otros scripts

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    void Start()
    {
        nameManager = NPCNameManager.Instance; // Get the NPCManager instance
    }

    public string GenerateMessage(MissionData mission)
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
            // Get a random NPC name
            string randomNPCName = NPCNameManager.Instance.GetRandomNPCName(); // Ensure this method is available in your NPC class

            // Replace {NPC_NAME} with the random NPC name
            randomMessageTemplate = randomMessageTemplate.Replace("{NPC_NAME}", randomNPCName);
        }

        return randomMessageTemplate; // Return the message (with or without NPC name)
    }
}
