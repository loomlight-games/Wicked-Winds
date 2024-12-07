using UnityEngine;
using UnityEngine.AI;

public class MovingState : AState
{
    private CatController catController;
    private NavMeshAgent agent;

    public MovingState(CatController catController, NavMeshAgent agent)
    {
        this.catController = catController;
        this.agent = agent;
    }

    public override void Enter()
    {
        SetRandomDestination();
        agent.speed = 3.0f;
    }

    public override void Update()
    {

        // Si el gato ha llegado a su destino, cambiar a IdleState
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

        // Actualizar la posici�n previa del jugador
        catController.previousPlayerPosition = PlayerManager.Instance.transform.position;
    }


    public override void Exit() { }

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
