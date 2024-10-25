using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingPlayerState : AState
{
    public override void Enter()
    {
        PlayerManager.Instance.movable.Start();
    }

    public override void Update()
    {
        PlayerManager.Instance.movable.Update();
        PlayerManager.Instance.flying.Update();

        if (PlayerManager.Instance.controller.isGrounded)
            Exit();
    }

    public override void Exit()
    {
        PlayerManager.Instance.SetState(PlayerManager.Instance.controllableState);
    }
}
