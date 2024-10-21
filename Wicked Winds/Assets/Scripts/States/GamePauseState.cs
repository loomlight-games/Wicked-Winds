using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GamePauseState : AState
{
    GameObject pauseMenu;
    GameObject resumeButton;
    GameObject background;
    GameObject HUD;
    public override void Enter()
    {
        pauseMenu = GameObject.Find("PauseMenu");
        pauseMenu.SetActive(true);
        resumeButton = pauseMenu.transform.Find("ResumeButton").gameObject;
        resumeButton.SetActive(true);
        background = pauseMenu.transform.Find("Background").gameObject;
        background.SetActive(true);

        HUD = GameObject.Find("HUD");
        HUD.SetActive(false);
        Time.timeScale = 0f;
    }
    public override void Update()
    {//press ESCAPE (i'll implement the button too later)
        if (Input.GetKeyDown(KeyCode.Escape))
            GameManager.Instance.ClickButton("ResumeButton");
    }
    public override void Exit()
    {
        Time.timeScale = 1f; // Resumes simulation

        //hide pause menu buttons
        resumeButton.SetActive(false);
        background.SetActive(false);
        HUD.SetActive(true);
    }
}
