using UnityEngine;

public class GamePauseState : AState
{
    GameObject UI, buttons, pauseMenu;
    public override void Enter()
    {
        Time.timeScale = 0f; // Stops simulation

        UI = GameObject.Find("UI");

        buttons = UI.transform.Find("Buttons").gameObject;
        pauseMenu = buttons.transform.Find("PauseMenu").gameObject;
        pauseMenu.SetActive(true);
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
        pauseMenu.SetActive(false);
    }
}
