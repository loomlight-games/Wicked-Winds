using UnityEngine;

public class FinalPlayerState : AState
{
    public override void Enter()
    {
        // Save score
        PlayerPrefs.SetInt(GameManager.Instance.PLAYER_SCORE_FILE, PlayerManager.Instance.score);
        Debug.Log("Saved score: " + PlayerManager.Instance.score);
    }

    // Update is called once per frame
    public override void Update()
    {

    }
}
