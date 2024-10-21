using UnityEngine;

public class GamePlayState : AState
{
    GameObject UI, buttons, gameplayButtons, hud;

    public override void Enter()
    {
        Time.timeScale = 1f; // Resumes simulation

        UI = GameObject.Find("UI");

        hud = UI.transform.Find("HUD").gameObject;
        hud.SetActive(true);

        buttons = UI.transform.Find("Buttons").gameObject;
        gameplayButtons = buttons.transform.Find("Gameplay").gameObject;
        gameplayButtons.SetActive(true);
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            GameManager.Instance.ClickButton("Pause");
    }

    public override void Exit()
    {
        hud.SetActive(false);
        gameplayButtons.SetActive(false);
    }
}
