using UnityEngine;
using UnityEngine.AI;

public class FleeingState : ICatState
{
    private CatController catController;
    private NavMeshAgent agent;
    private Transform player;

    public FleeingState(CatController catController, NavMeshAgent agent, Transform player)
    {
        this.catController = catController;
        this.agent = agent;
        this.player = player;
    }

    public void Enter()
    {
        Vector3 fleeDirection = (catController.transform.position - player.position).normalized * 20f;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(catController.transform.position + fleeDirection, out hit, 20f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    public void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            catController.ChangeState(catController.idleState);
        }
    }

    public void Exit() { }
}
