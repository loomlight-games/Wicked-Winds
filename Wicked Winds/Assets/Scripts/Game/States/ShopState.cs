using UnityEngine;

public class ShopState : AState
{
    public override void Enter()
    {
        Time.timeScale = 1f; // Resumes simulation
    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }
}
