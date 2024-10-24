
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;
using System.Collections;


public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> names;
    [SerializeField]
    private List<TextMeshProUGUI> scores;

    private string publicLeaderboardKey =
        "d79d438f30ad962bdf7ba1f9ef19bc37bc77c16c165f95f90bc3dff59716a850";

    private void Start()
    {
        GetLeaderboard();
        // Iniciar la coroutine para verificar la inicialización del ScoreManager
        StartCoroutine(WaitForScoreManagerInitialization());
    }

    // Coroutine que espera a que ScoreManager.Instance esté disponible
    private IEnumerator WaitForScoreManagerInitialization()
    {
        while (ScoreManager.Instance == null)
        {
            yield return null; // Esperar al siguiente frame
        }

        // Agregar listener una vez que ScoreManager esté disponible
        ScoreManager.Instance.submitScoreEvent.AddListener(SetLeaderboardEntry);
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

            }
        }) );
    }

    //uploading to the leaderboard
    public void SetLeaderboardEntry (string username, float score){
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username,
            (int)GameManager.Instance.playerScore, ((msg) =>
            {   //limit the number of characters in username to 20
                username = username.Substring(0, Mathf.Min(20, username.Length)); // Limita el nombre a 20 caracteres
                GetLeaderboard();
                Debug.Log("puntuacion subida correctamente");
            }) );
    }
}
