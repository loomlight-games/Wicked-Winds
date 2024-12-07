using UnityEngine;
using UnityEngine.AI;

public class IdleState : AState
{
    readonly CatController catController;
    float idleTime, timer;

    public IdleState(CatController catController)
    {
        this.catController = catController;
    }

    public override void Enter()
    {
        idleTime = Random.Range(catController.minIdleTime, catController.maxIdleTime);
        timer = 0f;
        catController.agent.isStopped = true;
    }

    public override void Update()
    {
        timer += Time.deltaTime;

        if (timer >= idleTime)
            catController.SwitchState(catController.randomMoveState);
    }

    public override void Exit()
    {
        catController.agent.isStopped = false;
    }
}
