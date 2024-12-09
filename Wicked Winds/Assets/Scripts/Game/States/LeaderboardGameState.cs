using UnityEngine;

public class LeaderboardGameState : AState
{
    public override void Enter()
    {
        PanelManager.Open("profile");
    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }
}
