using UnityEngine;
using UnityEngine.AI;

public class ClimbingState : AState
{
    readonly CatController catController;

    public ClimbingState(CatController catController)
    {
        this.catController = catController;
    }

    public override void Enter()
    {
        Vector3 targetPosition = catController.hit.point;

        if (NavMesh.SamplePosition(targetPosition, out NavMeshHit navHit, 1f, NavMesh.AllAreas))
            catController.agent.SetDestination(navHit.position);
        else
        {
            catController.agent.enabled = false;
            catController.transform.position = targetPosition;
            catController.agent.enabled = true;
        }
    }

    public override void Update()
    {
        if (!catController.agent.pathPending && catController.agent.remainingDistance <= catController.agent.stoppingDistance)
            catController.SwitchState(catController.idleState);
    }
}
