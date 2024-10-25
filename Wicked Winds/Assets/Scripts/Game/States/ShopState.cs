using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopState : AState
{
    GameObject UI, player;

    public override void Enter()
    {
        UI = GameObject.Find("UI");

        // Find player
        player = GameObject.Find("Player");
    }

    public override void Update()
    {
        // Rotates item
        player.transform.Rotate(0, 360 * 0.1f * Time.deltaTime, 0);
    }

    public override void Exit()
    {
        
    }
}
