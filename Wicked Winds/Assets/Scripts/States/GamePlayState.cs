using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class GamePlayState : AState
{
    public override void Enter()
    {
        Time.timeScale = 1f; // Resumes simulation
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            GameManager.Instance.ClickButton("Pause");
    }

    public override void Exit()
    {
    
    }
}
