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
        // Detectar si el jugador se acerca rápidamente y cambiar al estado de huida
        float distanceToPlayer = Vector3.Distance(catController.transform.position, catController.player.position);
        Vector3 playerVelocity = (catController.player.position - catController.previousPlayerPosition) / Time.deltaTime;

        // Si el jugador se acerca rápidamente
        if (distanceToPlayer < catController.fleeDistance && playerVelocity.magnitude > catController.playerApproachSpeed)
        {
            Debug.Log("Jugador se acerca rápidamente, cambiando a FleeingState.");
            catController.ChangeState(catController.fleeingState);
            return; // Salimos de la función para evitar seguir procesando el estado de movimiento
        }

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

        // Actualizar la posición previa del jugador
        catController.previousPlayerPosition = catController.player.position;
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
