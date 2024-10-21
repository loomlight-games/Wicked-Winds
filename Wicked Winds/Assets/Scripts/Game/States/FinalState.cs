using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class FinalState : AState
{
    GameObject finalMenu;
    GameObject defeat;
    //GameObject retryButton;
    //GameObject mainMenuButton;
    public override void Enter()
    {
        // Mostrar el menú final o de derrota
        finalMenu = GameObject.Find("FinalMenu");
        defeat = GameObject.Find("defeat");
        //defeat.SetActive(true);

        //retryButton = finalMenu.transform.Find("RetryButton").gameObject;
        //mainMenuButton = finalMenu.transform.Find("MainMenuButton").gameObject;

        //retryButton.SetActive(true);
        //mainMenuButton.SetActive(true);

        Time.timeScale = 0f; // Pausar el juego
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) // Por ejemplo, reiniciar el juego
        {
            GameManager.Instance.ClickButton("Replay");
        }
    }

    public override void Exit()
    {
        Time.timeScale = 1f; // Reanudar el juego si es necesario cuando se salga de este estado
        finalMenu.SetActive(false);
    }
}

