using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCNameManager : MonoBehaviour
{
    public List<string> npcNames; // List to hold NPC names
    private static NPCNameManager instance;
    public static NPCNameManager Instance { get { return instance; } } // Singleton pattern

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
        // Se pueden añadir nombres en el inspector
        npcNames = new List<string>
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
    }

    // Método para obtener un nombre aleatorio de NPC
    public string GetRandomNPCName()
    {
        if (npcNames.Count == 0)
        {
            Debug.LogWarning("No quedan más NPCs disponibles.");
            return null; // O manejar el caso si no hay nombres disponibles
        }

        int randomIndex = Random.Range(0, npcNames.Count);
        string randomName = npcNames[randomIndex];
        npcNames.RemoveAt(randomIndex); // Elimina el nombre para que no se repita
        return randomName;
    }
}
