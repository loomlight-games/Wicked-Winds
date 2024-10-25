
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;
using System.Collections;
using UnityEngine.SocialPlatforms.Impl;
using System;
using Unity.VisualScripting;


public class LeaderboardGameState : AState
{
    List<TextMeshProUGUI> names = new(), scores = new();

    TMP_InputField inputName;
    TextMeshProUGUI scoreText; // Campo para mostrar la puntuación

    int score; // Almacena la puntuación

    string publicLeaderboardKey;

    public override void Enter()
    {
        publicLeaderboardKey = GameManager.Instance.publicLeaderboardKey;
        GameObject namesParent = GameObject.Find("Names");
        GameObject scoresParent = GameObject.Find("Scores");
        scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        inputName = GameObject.Find("InputName").GetComponent<TMP_InputField>();

        foreach (Transform nameTransform in namesParent.transform)
        {
            TextMeshProUGUI name = nameTransform.GetComponent<TextMeshProUGUI>();

            names.Add(name);

            Debug.Log(name.text);
        }


        foreach (Transform scoreTransform in scoresParent.transform)
        {
            TextMeshProUGUI score = scoreTransform.GetComponent<TextMeshProUGUI>();

            scores.Add(score);

            Debug.Log(score.text);
        }

        // Recuperar la puntuación desde PlayerPrefs (o 0 si no hay puntuación guardada)
        score = PlayerPrefs.GetInt(GameManager.Instance.PLAYER_SCORE_FILE, 0);
        
        // Mostrar la puntuación en el UI
        scoreText.text = score.ToString();

        GetLeaderboard();
    }

    public void SubmitScore() //ARREGLAAAAAAAAAAAR RESETEA EL SCORE AL CAMBIAR AL LEADERBOARD
    {
        // Obtener el nombre del input
        if (!string.IsNullOrEmpty(inputName.text))
        {
            SetLeaderboardEntry(inputName.text, score);

            Debug.Log("Nombre y puntuación enviados al evento." + inputName.text + score);

        }
        else
        {

            Debug.LogWarning("El nombre del jugador está vacío.");
        }
    }

    public void GetLeaderboard()
    {
        // Obtener los datos del leaderboard desde la API
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) =>
        {
            int loopLength = (msg.Length < names.Count)? msg.Length: names.Count;

            for (int i = 0; i < loopLength; i++)
            {
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();

                Debug.Log($"Usuario: {names[i].text}, Puntaje: {scores[i].text}");
            }
        }));
    }



    //uploading to the leaderboard
    public void SetLeaderboardEntry(string username, int scoreToUpload)
    {
        Debug.Log("Puntuación antes de subir: " + scoreToUpload);

        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, scoreToUpload, (msg) =>
        {
            Debug.Log("Puntuación subida correctamente"+ username + scoreToUpload);

            // Actualizar el leaderboard después de subir la puntuación
            GetLeaderboard();
        });
        
    }

    public void ResetPlayerScore()
    {
        PlayerPrefs.DeleteKey("PlayerScore");
        PlayerPrefs.Save();
        score = 0; // Reinicia el puntaje en memoria
        scoreText.text = score.ToString(); // Actualiza la UI si es necesario
    }

    // Método que actualiza la puntuación cuando el juego acaba
    public void UpdateScore(int elapsedTimeScore)
    {
        score = elapsedTimeScore;  // Asigna el tiempo transcurrido como la puntuación
                                   // Guardar la puntuación actualizada en PlayerPrefs
        PlayerPrefs.SetFloat("PlayerScore", score);
        PlayerPrefs.Save();
        // Mostrar la puntuación actualizada
        scoreText.text = score.ToString();
        Debug.Log("Puntuación actualizada: " + score);
        SubmitScore();
    }
}
