using UnityEngine;
using UnityEngine.AI;

public class IdleState : AState
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

    public override void Enter()
    {
        idleTime = Random.Range(5f, 10f);
        timer = 0f;
        agent.isStopped = true;
    }

    public override void Update()
    {
        // Detectar si el jugador se acerca r�pidamente y cambiar al estado de huida
        float distanceToPlayer = Vector3.Distance(catController.transform.position, PlayerManager.Instance.transform.position);
        Vector3 playerVelocity = (PlayerManager.Instance.transform.position - catController.previousPlayerPosition) / Time.deltaTime;

        // Si el jugador se acerca r�pidamente
        if (distanceToPlayer < catController.fleeDistance && playerVelocity.magnitude > catController.playerApproachSpeed)
        {
            Debug.Log("Jugador se acerca r�pidamente, cambiando a FleeingState.");
            catController.ChangeState(catController.fleeingState);
            return; // Salimos de la funci�n para evitar seguir procesando el estado de movimiento
        }
        timer += Time.deltaTime;
        if (timer >= idleTime)
        {
            catController.ChangeState(catController.randomMoveState);
        }

        // Actualizar la posici�n previa del jugador
        catController.previousPlayerPosition = PlayerManager.Instance.transform.position;
    }

    public override void Exit()
    {
        agent.isStopped = false;
    }
}
