using UnityEngine;
using UnityEngine.AI;

public class MovingState : AState
{
    readonly CatController catController;

    public MovingState(CatController catController)
    {
        this.catController = catController;
    }

    public override void Enter()
    {
        SetRandomDestination();
    }

    public override void Update()
    {
        // Destination is reached
        if (!catController.agent.pathPending && catController.agent.remainingDistance <= catController.agent.stoppingDistance)
            catController.SwitchState(catController.idleState);
        // Detect building
        else if (Physics.Raycast(catController.transform.position, Vector3.up, out RaycastHit hit, 5f, catController.buildingLayer))
        {
            hit = catController.hit;
            catController.SwitchState(catController.climbingState);
        }
    }

    private void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 15f + catController.transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 15f, NavMesh.AllAreas))
            catController.agent.SetDestination(hit.position);
    }
}
