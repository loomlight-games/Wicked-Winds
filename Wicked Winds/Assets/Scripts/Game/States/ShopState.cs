using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopState : AState
{
    GameObject UI;

    public override void Enter()
    {
        UI = GameObject.Find("UI");
    }

    public override void Exit()
    {
        
    }
}
