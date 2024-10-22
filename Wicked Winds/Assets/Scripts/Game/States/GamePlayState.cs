using UnityEngine;

public class GamePlayState : AState
{
    GameObject UI, statesUI, gameplayUI, hud;

    public override void Enter()
    {
        Time.timeScale = 1f; // Resumes simulation

        UI = GameObject.Find("UI");

        hud = UI.transform.Find("HUD").gameObject;
        hud.SetActive(true);

        statesUI = UI.transform.Find("States").gameObject;
        statesUI.SetActive(true);
        gameplayUI = statesUI.transform.Find("Gameplay").gameObject;
        gameplayUI.SetActive(true);
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            GameManager.Instance.ClickButton("Pause");
    }

    public override void Exit()
    {
        hud.SetActive(false);
        gameplayUI.SetActive(false);
    }
}
