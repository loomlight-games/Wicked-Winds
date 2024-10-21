using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GamePauseState : AState
{
    GameObject pauseMenu;
    GameObject resumeButton;

    GameObject HUD;
    public override void Enter()
    {
        pauseMenu = GameObject.Find("PauseMenu");
        pauseMenu.SetActive(true);
        resumeButton = pauseMenu.transform.Find("ResumeButton").gameObject;
        resumeButton.SetActive(true);


        HUD = GameObject.Find("HUD");
        HUD.SetActive(false);
        Time.timeScale = 0f;
    }
    public override void Update()
    {//press ESCAPE 
        if (Input.GetKeyDown(KeyCode.Escape))
            GameManager.Instance.ClickButton("ResumeButton");
    }
    public override void Exit()
    {
        //hide pause menu buttons
        resumeButton.SetActive(false);

        HUD.SetActive(true);
    }
}
