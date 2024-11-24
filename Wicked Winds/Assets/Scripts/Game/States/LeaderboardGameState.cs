using UnityEngine;

public class LeaderboardGameState : AState
{

    public override void Enter()
    {
        PanelManager.Open("auth");
        SoundManager.Instance.PlayMainMenuMusic();
    }
}
