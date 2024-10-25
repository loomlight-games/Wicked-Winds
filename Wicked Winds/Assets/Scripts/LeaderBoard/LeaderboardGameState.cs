
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;
using UnityEngine.UIElements;

public class LeaderboardGameState : AState
{
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

    /*
    /// <summary>
    /// Get online data from leaderboard and update scene texts
    /// </summary>
    public void GetLeaderboard()
    {
        // Obtener los datos del leaderboard desde la API
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) =>
        {
            // Choose the lesser one
            int loopLength = (msg.Length < names.Count) ? msg.Length : names.Count;

            for (int i = 0; i < loopLength; ++i)
            {
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();

                Debug.Log($"Usuario: {names[i].text}, Puntaje: {scores[i].text}");
            }
        }));
    }

    /// <summary>
    /// Uploading to the online leaderboard
    /// </summary>
    public void SetLeaderboardEntry(string username, int scoreToUpload)
    {
        Debug.Log("Puntuación antes de subir: " + scoreToUpload);

        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, scoreToUpload, (msg) =>
        {
            Debug.Log("Puntuación subida correctamente"+ username + scoreToUpload);

            // Actualizar el leaderboard después de subir la puntuación
            //GetLeaderboard();
        });

        // Actualizar el leaderboard después de subir la puntuación
        //GetLeaderboard(); // Supuestamente asegrua q aparezca el nombre con el score
    }*/
}
