using UnityEngine;

public class GamePauseState : AState
{
    GameObject UI, statesUI, pauseMenuUI;
    public override void Enter()
    {
        Time.timeScale = 0f; // Stops simulation

        UI = GameObject.Find("UI");

        statesUI = UI.transform.Find("States").gameObject;
        pauseMenuUI = statesUI.transform.Find("PauseMenu").gameObject;
        pauseMenuUI.SetActive(true);
    }
    public override void Update()
    {
        // Press ESCAPE 
        if (Input.GetKeyDown(KeyCode.Escape))
            GameManager.Instance.ClickButton("Resume");
    }
    public override void Exit()
    {
        // Hide pause menu
        pauseMenuUI.SetActive(false);
    }
}
