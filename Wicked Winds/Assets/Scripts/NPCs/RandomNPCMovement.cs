using UnityEngine;
using UnityEngine.AI;

public class RandomNPCMovement : MonoBehaviour
{
    public float moveRadius = 10f; // Radio de movimiento aleatorio
    public float detectionRadius = 100f; // Radio para detectar si la posición es adecuada (dentro del terreno)
    public LayerMask groundLayer; // Capa del suelo
    public LayerMask buildingLayer; // Capa de edificios para evitar que se posicione sobre ellos

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Generar una posición inicial aleatoria
        Vector3 randomPosition = GetRandomPositionOnGround();
        if (randomPosition != Vector3.zero)
        {
            transform.position = randomPosition;
        }

        // Iniciar el movimiento aleatorio
        MoveToRandomPosition();
    }

    void Update()
    {
        // Si el NPC ha llegado a su destino, generar una nueva posición
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            MoveToRandomPosition();
        }
    }

    // Método para mover al NPC a una posición aleatoria dentro de un radio
    void MoveToRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * moveRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, moveRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    // Método para generar una posición aleatoria en el suelo (evitando edificios)
    Vector3 GetRandomPositionOnGround()
    {
        Vector3 randomPosition = Vector3.zero;

        for (int i = 0; i < 30; i++) // Intentar 30 veces encontrar una posición adecuada
        {
            Vector3 randomPoint = new Vector3(
                Random.Range(-detectionRadius, detectionRadius),
                100f,
                Random.Range(-detectionRadius, detectionRadius)
            ); // Generar un punto alto para hacer un raycast hacia abajo

            // Raycast desde arriba hacia abajo para detectar el suelo
            if (Physics.Raycast(randomPoint, Vector3.down, out RaycastHit hit, Mathf.Infinity, groundLayer))
            {
                // Verificar si el punto detectado no está sobre un edificio
                Collider[] hitColliders = Physics.OverlapSphere(hit.point, 0.5f, buildingLayer);
                if (hitColliders.Length == 0) // Si no hay edificios, es un punto válido
                {
                    randomPosition = hit.point;
                    break; // Salir del loop si encontramos una posición adecuada
                }
            }
        }

        return randomPosition;
    }
}
