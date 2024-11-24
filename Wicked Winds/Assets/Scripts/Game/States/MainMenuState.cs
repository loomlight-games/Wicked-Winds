using UnityEngine;

public class MainMenuState : AState
{
    GameObject UI, mainMenu;

    public override void Enter()
    {
        UI = GameObject.Find("UI");
        mainMenu = UI.transform.Find("Main menu").gameObject;
        mainMenu.SetActive(true);

        SoundManager.Instance.PlayMainMenuMusic();
    }

    public override void Exit()
    {
        mainMenu.SetActive(false);
    }
}
