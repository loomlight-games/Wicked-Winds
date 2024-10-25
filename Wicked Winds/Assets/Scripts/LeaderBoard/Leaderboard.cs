
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;
using System.Collections;
using UnityEngine.SocialPlatforms.Impl;
using System;


public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> names;
    [SerializeField]
    private List<TextMeshProUGUI> scores;

    [SerializeField] private TMP_InputField inputName;
    [SerializeField] private TextMeshProUGUI scoreText; // Campo para mostrar la puntuaci�n

    public int score; // Almacena la puntuaci�n

    private string publicLeaderboardKey =
        "d79d438f30ad962bdf7ba1f9ef19bc37bc77c16c165f95f90bc3dff59716a850";

    private void Start()
    {
        
        // Recuperar la puntuaci�n desde PlayerPrefs (o 0 si no hay puntuaci�n guardada)
        score = Mathf.FloorToInt(PlayerPrefs.GetFloat("PlayerScore", 0));
        // Mostrar la puntuaci�n en el UI
        scoreText.text = score.ToString();
        GetLeaderboard();
        //UpdateScore(score);

    }
    // M�todo que actualiza la puntuaci�n cuando el juego acaba
    public void UpdateScore(int elapsedTimeScore)
    {
        score = elapsedTimeScore;  // Asigna el tiempo transcurrido como la puntuaci�n
                                   // Guardar la puntuaci�n actualizada en PlayerPrefs
        PlayerPrefs.SetFloat("PlayerScore", score);
        PlayerPrefs.Save();
        // Mostrar la puntuaci�n actualizada
        scoreText.text = score.ToString();
        Debug.Log("Puntuaci�n actualizada: " + score);
        SubmitScore();
    }

    public void SubmitScore() //ARREGLAAAAAAAAAAAR RESETEA EL SCORE AL CAMBIAR AL LEADERBOARD
    {
        // Obtener el nombre del input

        if (!string.IsNullOrEmpty(inputName.text))
        {
            SetLeaderboardEntry(inputName.text, score);

            Debug.Log("Nombre y puntuaci�n enviados al evento." + inputName.text + score);

        }
        else
        {

            Debug.LogWarning("El nombre del jugador est� vac�o.");
        }
    }
    public void GetLeaderboard()
    {
        // Obtener los datos del leaderboard desde la API
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, false, ((msg) =>
        {
            

            int loopLength = (msg.Length< names.Count)? msg.Length: names.Count;

            for (int i = 0; i < loopLength; i++)
            {
                Debug.Log($"Usuario: {msg[i].Username}, Puntaje: {msg[i].Score}");
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
            }

            Canvas.ForceUpdateCanvases();
        }));
    }



    //uploading to the leaderboard
    public void SetLeaderboardEntry(string username, int scoreToUpload)
    {
        Debug.Log("Puntuaci�n antes de subir: " + scoreToUpload);

        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, scoreToUpload, (msg) =>
        {
            Debug.Log("Puntuaci�n subida correctamente");

            // Actualizar el leaderboard despu�s de subir la puntuaci�n
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

}
