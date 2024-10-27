
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UIElements;

public class LeaderboardGameState : AState
{

    public override void Enter()
    {
        //funcion con idea de hacer un swithc que diferencie los diferentes rankings, de momento solo hay uno
        PanelManager.Open("auth");
        Debug.Log("openleaderboards");

    }


    /*
    static int size = 7;
    TextMeshProUGUI[] entriesText = new TextMeshProUGUI[size];

    TMP_InputField inputName;
    TextMeshProUGUI scoreText; // Campo para mostrar la puntuación

    int score; 
  

    public override void Enter()
    {
        GameObject entriesParent = GameObject.Find("Entries");
        scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        inputName = GameObject.Find("InputName").GetComponent<TMP_InputField>();

        // Filling the array with parents's children
        for (int i = 0; i < size; i++)
        {
            //TextMeshProUGUI name = name.Transform.GetComponent<TextMeshProUGUI>();
            //names.Add(name);
            entriesText[i] = entriesParent.transform.GetChild(i).GetComponent<TextMeshProUGUI>(); 
        }

        // Deserialise score 
        score = PlayerPrefs.GetInt(GameManager.Instance.PLAYER_SCORE_FILE, 0);
        
        // Show score
        scoreText.text = score.ToString();

        LoadEntries();
    }

    public void LoadEntries()
    {
        Leaderboards.ElapsedTime.GetEntries(entries =>
        {
            foreach (var t in entriesText)
                t.text = "";

            var length = Mathf.Min(entriesText.Length, entries.Length);

            for (int i = 0; i < length; i++)
            {
                entriesText[i].text = $"{entries[i].Rank}. {entries[i].Username} - {entries[i].Score}";
            }
        });
    }

    /// <summary>
    /// Gets username from input field
    /// </summary>
    public void SubmitScore() //ARREGLAAAAAAAAAAAR RESETEA EL SCORE AL CAMBIAR AL LEADERBOARD
    {
        Leaderboards.ElapsedTime.UploadNewEntry(inputName.text, score, isSuccessful =>
        {
            if (isSuccessful)
                LoadEntries();
        });
    }
*/
}
