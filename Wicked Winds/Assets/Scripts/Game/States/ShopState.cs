using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopState : AState
{
    GameObject UI;

    public override void Enter()
    {
        UI = GameObject.Find("UI");
        Time.timeScale = 0f; // Stops simulation
    }

    public override void Exit()
    {
        Time.timeScale = 1f; // Restores simulation
    }
}
