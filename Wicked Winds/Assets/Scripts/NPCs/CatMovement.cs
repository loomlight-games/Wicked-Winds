using UnityEngine;
using UnityEngine.AI;

public class CatMovement : MonoBehaviour
{
    public float moveRadius = 10f;
    public float fleeSpeed = 5f;
    public float detectionRadius = 10f;
    public float playerSpeedThreshold = 2f; // Velocidad para decidir si el jugador es lento o rápido
    public Transform player;
    public LayerMask groundLayer;
    public LayerMask buildingLayer;

    private NavMeshAgent agent;
    private bool isFleeing;
    private Vector3 lastPlayerPosition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Random.Range(3f, 5f); // Velocidad inicial aleatoria
        lastPlayerPosition = player.position; // Posición inicial del jugador
        SetRandomDestination();
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            float playerSpeed = GetPlayerSpeed();

            if (distanceToPlayer < detectionRadius) // Si el jugador está cerca
            {
                if (playerSpeed >= playerSpeedThreshold) // Si el jugador se mueve rápido, el gato huye
                {
                    FleeFromPlayer();
                }
                else // Si el jugador es lento, el gato se acerca
                {
                    FollowPlayer();
                }
                return;
            }
        }

        // Movimiento aleatorio si no está huyendo
        if (!isFleeing && (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance))
        {
            SetRandomDestination();
        }
    }

    /// <summary>
    /// Calcula la velocidad del jugador basado en su posición actual y anterior.
    /// </summary>
    /// <returns>La velocidad del jugador.</returns>
    float GetPlayerSpeed()
    {
        float speed = (player.position - lastPlayerPosition).magnitude / Time.deltaTime;
        lastPlayerPosition = player.position;
        return speed;
    }

    void SetRandomDestination()
    {
        float minDistance = 5f;
        float maxDistance = moveRadius;
        Vector3 randomDirection;
        NavMeshHit hit;

        for (int i = 0; i < 30; i++)
        {
            randomDirection = Random.insideUnitSphere * maxDistance;
            randomDirection += transform.position;

            if (NavMesh.SamplePosition(randomDirection, out hit, maxDistance, NavMesh.AllAreas) &&
                Vector3.Distance(transform.position, hit.position) >= minDistance)
            {
                agent.SetDestination(hit.position);

                if (agent.pathStatus == NavMeshPathStatus.PathComplete)
                {
                    return;
                }
            }
        }
    }

    void FleeFromPlayer()
    {
        isFleeing = true;
        Vector3 fleeDirection = (transform.position - player.position).normalized * fleeSpeed;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(transform.position + fleeDirection, out hit, moveRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void FollowPlayer()
    {
        isFleeing = false;
        agent.SetDestination(player.position);
    }
}
