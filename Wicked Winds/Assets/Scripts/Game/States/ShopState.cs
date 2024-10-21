using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopState : AState
{
    GameObject UI, buttons;

    public override void Enter()
    {
        UI = GameObject.Find("UI");
        buttons = UI.transform.Find("Buttons").gameObject;
        buttons.SetActive(true);
    }

    public override void Exit()
    {
        buttons.SetActive(false);
    }
}
