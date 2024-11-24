using UnityEngine;

public class GamePauseState : AState
{
    GameObject UI, 
        statesUI, 
        pauseMenuUI, 
        //PCtutorial, 
        //tutorial, 
        PCyes, 
        PCno,
        handledControls;

    public override void Enter()
    {
        Time.timeScale = 0f; // Stops simulation

        UI = GameObject.Find("Game UI");

        statesUI = UI.transform.Find("States").gameObject;
        pauseMenuUI = statesUI.transform.Find("PauseMenu").gameObject;
        pauseMenuUI.SetActive(true);

        GameObject playingOnPCbutton = pauseMenuUI.transform.Find("Playing on PC").gameObject;
        PCyes = playingOnPCbutton.transform.Find("Yes").gameObject;
        PCno = playingOnPCbutton.transform.Find("No").gameObject;

        //PCtutorial = UI.transform.Find("PC tutorial").gameObject;
        //tutorial = UI.transform.Find("Not PC tutorial").gameObject;

        if (GameManager.Instance.playingOnPC){
            PCyes.SetActive(true);
            // Show PC tutorial
            //PCtutorial.SetActive(true);
        }else{
            PCno.SetActive(true);
            // Show handled device tutorial
            //tutorial.SetActive(true);
        }

        handledControls = UI.transform.Find("Handled controls").gameObject;
    }
    public override void Update()
    {
        if (GameManager.Instance.playingOnPC){
            PCyes.SetActive(true);
            PCno.SetActive(false);
            //PCtutorial.SetActive(true);
            //tutorial.SetActive(false);
            handledControls.SetActive(false);
        }else{
            PCyes.SetActive(false);
            PCno.SetActive(true);
            //PCtutorial.SetActive(false);
            //tutorial.SetActive(true);
            handledControls.SetActive(true);
        }
    }
    public override void Exit()
    {
        // Hide pause menu
        pauseMenuUI.SetActive(false);
        //PCtutorial.SetActive(false);
        //tutorial.SetActive(false);
        Time.timeScale = 1f; // Reactivates simulation
    }
}
