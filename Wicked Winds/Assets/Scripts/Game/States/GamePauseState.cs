using UnityEngine;

public class GamePauseState : AState
{
    GameObject UI,
        statesUI,
        pauseMenuUI;

    public override void Enter()
    {
        Time.timeScale = 0f; // Stops simulation

        UI = GameObject.Find("Game UI");

        statesUI = UI.transform.Find("States").gameObject;
        pauseMenuUI = statesUI.transform.Find("PauseMenu").gameObject;
        pauseMenuUI.SetActive(true);
    }

    public override void Update()
    {
        float deltaTime = Time.unscaledDeltaTime;
    }

    public override void Exit()
    {
        // Hide pause menu
        pauseMenuUI.SetActive(false);

        //Time.timeScale = 1f; // Reactivates simulation
    }
}
