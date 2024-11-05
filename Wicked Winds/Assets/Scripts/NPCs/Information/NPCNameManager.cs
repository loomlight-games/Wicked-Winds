using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCNameManager : MonoBehaviour
{
    public List<string> npcNames; // List to hold NPC names
    private static NPCNameManager instance;
    public static NPCNameManager Instance { get { return instance; } } // con el patron singleton hacemos que 
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

    public string GetRandomNPCName()
    {
        int randomIndex = Random.Range(0, npcNames.Count);
        return npcNames[randomIndex];
    }
}