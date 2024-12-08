using System.Collections.Generic;
using UnityEngine;

public class NPCNameManager : MonoBehaviour
{
    public static NPCNameManager Instance;
    List<string> npcNames = new()
    {
        "Tommy", "Mabel", "Blaire", "Bea", "Henry", "Felicity",
        "Graham", "Lopez", "Celia", "Bea", "Jitters", "Puddles",
        "Admiral", "Alfonso", "Bingo", "Bunnie", "Kiki", "Lolly",
        "Melba", "Maggie", "Daisy", "Lucky", "Piper", "Rod",
        "Broccolo", "Maggie", "Mitzi", "Sherb", "Tiffany", "Carmen",
        "Bangle", "Flo", "Pate", "Friga", "Midge", "Pango",
        "Rocco", "Sprocket", "Zucker", "Fang", "Pashmina", "Chester",
        "Bree", "Cyd", "Erik", "Lobo", "Claudia", "Gaston",
        "Carmen", "Gigi", "Lolly", "Nibbles", "Tutu", "Winnie"
    };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public string GetRandomNPCName()
    {
        if (npcNames.Count == 0)
        {
            Debug.LogWarning("No more NPC names available.");
            return null;
        }

        int randomIndex = Random.Range(0, npcNames.Count);
        string randomName = npcNames[randomIndex];
        //npcNames.RemoveAt(randomIndex); // Don't duplicate names
        return randomName;
    }
}
