using UnityEngine;

public class ShopState : AState
{
    GameObject UI, player;

    public override void Enter()
    {
        Time.timeScale = 1f; // Resumes simulation

        UI = GameObject.Find("UI");
    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }
}
