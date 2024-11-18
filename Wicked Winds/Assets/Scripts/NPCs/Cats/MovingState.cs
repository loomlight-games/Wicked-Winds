using UnityEngine;
using UnityEngine.AI;

public class MovingState : ICatState
{
    private CatController catController;
    private NavMeshAgent agent;

    public MovingState(CatController catController, NavMeshAgent agent)
    {
        this.catController = catController;
        this.agent = agent;
    }

    public void Enter()
    {
        SetRandomDestination();
    }

    public void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            catController.ChangeState(catController.idleState);
        }

        // Detectar edificios cercanos
        if (Physics.Raycast(catController.transform.position, Vector3.up, out RaycastHit hit, 5f, catController.buildingLayer))
        {
            Debug.Log("Edificio detectado. Cambiando a ClimbingState.");
            catController.ChangeState(catController.climbingState);
        }
    }

    public void Exit() { }

    private void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 15f + catController.transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 15f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}
