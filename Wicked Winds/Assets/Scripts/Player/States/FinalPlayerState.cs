using UnityEngine;

public class FinalPlayerState : AState
{
    public override void Enter()
    {
        // Save score
        PlayerPrefs.SetInt(GameManager.Instance.PLAYER_SCORE_FILE, PlayerManager.Instance.score);
        PlayerPrefs.SetInt(GameManager.Instance.PLAYER_MISSIONCOUNT_FILE, MissionManager.Instance.missionsCompleted);
        PlayerPrefs.SetInt(GameManager.Instance.PLAYER_MISSIONTIME_FILE, (int)GameManager.Instance.averageMissionTime);
        Debug.Log("Saved Average Time/Mission: " + GameManager.Instance.PLAYER_MISSIONTIME_FILE);
        Debug.Log("Saved score: " + PlayerManager.Instance.score);
        Debug.Log("Saved mission count: " + GameManager.Instance.PLAYER_MISSIONCOUNT_FILE);
    }

    // Update is called once per frame
    public override void Update()
    {

    }
}
