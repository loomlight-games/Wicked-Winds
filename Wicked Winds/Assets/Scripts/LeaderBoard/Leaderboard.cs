
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;
using System.Collections;
using UnityEngine.SocialPlatforms.Impl;


public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> names;
    [SerializeField]
    private List<TextMeshProUGUI> scores;

    [SerializeField] private TMP_InputField inputName;
    [SerializeField] private TextMeshProUGUI scoreText; // Campo para mostrar la puntuación

    public int score; // Almacena la puntuación

    private string publicLeaderboardKey =
        "d79d438f30ad962bdf7ba1f9ef19bc37bc77c16c165f95f90bc3dff59716a850";

    private void Start()
    {
        
        // Recuperar la puntuación desde PlayerPrefs (o 0 si no hay puntuación guardada)
        score = Mathf.FloorToInt(PlayerPrefs.GetFloat("PlayerScore", 0));
        // Mostrar la puntuación en el UI
        scoreText.text = score.ToString();
        GetLeaderboard();
        //UpdateScore(score);

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
        //after getting leaderboard data, runs callback function (msg contains leaderboard data)
        //in order to access names and scores
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) =>
        {
            int loopLength = (msg.Length < names.Count ? msg.Length : names.Count); 
            // GetLeaderboar -> returns a value accessed by the msg parameter
            // pass the names and scores to the leaderboard
            for (int i = 0; i < loopLength; i++)
            {
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
                Debug.Log("Asignando: " + msg[i].Username + " - " + msg[i].Score);

            }
        }) );
    }

    //uploading to the leaderboard
    public void SetLeaderboardEntry(string username, int scoreToUpload)
    {
        Debug.Log("Puntuación antes de subir: " + scoreToUpload);

        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, scoreToUpload, (msg) =>
        {
            Debug.Log("Puntuación subida correctamente");

            // Actualizar el leaderboard después de subir la puntuación
            GetLeaderboard();
        });
    }
}
