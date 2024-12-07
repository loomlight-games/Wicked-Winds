using UnityEngine;

public class LeaderboardGameState : AState
{
    public override void Enter()
    {
        PanelManager.Open("auth");
    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }
}
