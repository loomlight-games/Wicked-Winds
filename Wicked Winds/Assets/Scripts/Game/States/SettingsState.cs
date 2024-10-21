using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsState : AState
{
    GameObject UI, settings;

    public override void Enter()
    {
        UI = GameObject.Find("UI");
        settings = UI.transform.Find("Settings").gameObject;
        settings.SetActive(true);
    }

    public override void Exit()
    {
        settings.SetActive(false);
    }
}
