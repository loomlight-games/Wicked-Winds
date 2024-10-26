using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllablePlayerState : AState
{
    public override void Enter()
    {
        PlayerManager.Instance.movable.Start();
        PlayerManager.Instance.boostable.Start();
    }

    public override void Update()
    {
        PlayerManager.Instance.movable.Update();
        PlayerManager.Instance.flying.Update();
        PlayerManager.Instance.boostable.Update();
    }

    public override void Exit()
    {
        
    }

    public override void OnTriggerEnter(Collider other)
    {
        PlayerManager.Instance.boostable.OnTriggerEnter(other);
    }
}
