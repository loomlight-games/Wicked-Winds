using UnityEngine;
using UnityEngine.AI;

public class IdleState : ICatState
{
    private CatController catController;
    private NavMeshAgent agent;
    private float idleTime;
    private float timer;

    public IdleState(CatController catController, NavMeshAgent agent)
    {
        this.catController = catController;
        this.agent = agent;
    }

    public void Enter()
    {
        idleTime = Random.Range(5f, 10f);
        timer = 0f;
        agent.isStopped = true;
    }

    public void Update()
    {
        timer += Time.deltaTime;
        if (timer >= idleTime)
        {
            catController.ChangeState(catController.randomMoveState);
        }
    }

    public void Exit()
    {
        agent.isStopped = false;
    }
}
