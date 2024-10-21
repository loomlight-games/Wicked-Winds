using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsState : AState
{
    GameObject UI, credits;

    public override void Enter()
    {
        UI = GameObject.Find("UI");
        credits = UI.transform.Find("Credits").gameObject;
        credits.SetActive(true);
    }

    public override void Exit()
    {
        credits.SetActive(false);
    }
}
