using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Leaderboards.Models;
using UnityEngine.UI;

public class LeaderboardsPlayerItem : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI rankText = null;
    [SerializeField] public TextMeshProUGUI nameText = null;
    [SerializeField] public TextMeshProUGUI scoreText = null;

    private LeaderboardEntry player = null; //guarda los datos del player para usarlos luego


    /// <summary>
    /// whenever creates an instance of player is called this function, which contains the data saved on the
    /// leaderbaords function from the unity game services (data related to the player)
    /// </summary>
    /// <param name="player"></param>
    public void Initialize(LeaderboardEntry player)
    {
        this.player = player;
        rankText.text = (player.Rank + 1).ToString();
        nameText.text = player.PlayerName;
        scoreText.text = player.Score.ToString();
    }
}
