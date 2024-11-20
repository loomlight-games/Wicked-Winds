using UnityEngine;
using UnityEngine.AI;

public class ClimbingState : ICatState
{
    private CatController catController;
    private NavMeshAgent agent;

    public ClimbingState(CatController catController, NavMeshAgent agent)
    {
        this.catController = catController;
        this.agent = agent;
    }

    public void Enter()
    {
        // L?gica para detectar y moverse hacia un edificio
        Vector3 targetPosition;
        if (Physics.Raycast(catController.transform.position, Vector3.up, out RaycastHit hit, 5f, catController.buildingLayer))
        {
            targetPosition = hit.point;

            if (NavMesh.SamplePosition(targetPosition, out NavMeshHit navHit, 1f, NavMesh.AllAreas))
            {
                agent.SetDestination(navHit.position);
            }
            else
            {
                JumpToPosition(targetPosition);
            }
        }
        else
        {
            catController.ChangeState(catController.randomMoveState);
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

    private void JumpToPosition(Vector3 position)
    {
        agent.enabled = false;
        catController.transform.position = position;
        agent.enabled = true;
    }
}
