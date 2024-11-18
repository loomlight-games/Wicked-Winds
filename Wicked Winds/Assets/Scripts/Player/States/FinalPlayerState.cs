using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPlayerState : AState
{
    public override void Enter()
    {
        // Save score
        PlayerPrefs.SetInt(GameManager.Instance.PLAYER_SCORE_FILE, PlayerManager.Instance.score);
        PlayerPrefs.SetInt(GameManager.Instance.PLAYER_MISSIONCOUNT_FILE, MissionManager.Instance.missionsCompleted);
        PlayerPrefs.SetFloat(GameManager.Instance.PLAYER_MISSIONTIME_FILE, GameManager.Instance.AverageMissionTime);
        Debug.Log("Saved Average Time/Mission: " + GameManager.Instance.AverageMissionTime);
        Debug.Log("Saved score: " + PlayerManager.Instance.score);
    }

    // Update is called once per frame
    public override void Update()
    {
        
    }
}
