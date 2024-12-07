using UnityEngine;

public class SettingsState : AState
{

    GameObject UI, settingsMenu;

    public override void Enter()
    {
        UI = GameObject.Find("UI");
        settingsMenu = UI.transform.Find("Settings menu").gameObject;
        settingsMenu.SetActive(true);
    }

    public override void Exit()
    {
        settingsMenu.SetActive(false);
    }
}
