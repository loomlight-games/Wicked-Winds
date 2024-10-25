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

       /* // Generar una posición inicial aleatoria
        Vector3 randomPosition = GetRandomPositionOnGround();
        if (randomPosition != Vector3.zero)
        {
            transform.position = randomPosition;
            Debug.Log($"Posición inicial establecida en: {randomPosition}");
        }
        else
        {
            Debug.LogWarning("No se pudo encontrar una posición inicial válida.");
        }
       */
        // Iniciar el movimiento aleatorio
        SetRandomDestination();
    }

    void Update()
    {
        // Si el NPC ha llegado a su destino, generar una nueva posición
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            Debug.Log("NPC ha llegado a su destino. Estableciendo nueva posición.");
            SetRandomDestination();
        }
    }


    public void SetRandomDestination()
    {
        // Definir un rango mínimo y máximo desde la posición actual para evitar destinos demasiado cercanos
        float minDistance = 5f; // Distancia mínima para evitar destinos cercanos
        float maxDistance = moveRadius; // Distancia máxima para el rango de movimiento

        Vector3 randomDirection;
        NavMeshHit hit;

        for (int i = 0; i < 30; i++) // Intentos para encontrar una posición válida
        {
            // Generar una dirección aleatoria
            randomDirection = Random.insideUnitSphere * maxDistance;
            randomDirection += transform.position;

            // Verificar que la nueva posición esté en el NavMesh y a una distancia adecuada
            if (NavMesh.SamplePosition(randomDirection, out hit, maxDistance, NavMesh.AllAreas) &&
                Vector3.Distance(transform.position, hit.position) >= minDistance)
            {
                agent.SetDestination(hit.position); // Establecer el destino válido
                Debug.Log($"Destino aleatorio establecido en: {hit.position}");
                return; // Salir del método al encontrar una posición válida
            }
            else
            {
                Debug.Log($"Intento {i + 1}: posición aleatoria no válida: {randomDirection}");
            }
        }

        Debug.LogWarning("No se pudo encontrar un destino válido después de 30 intentos.");
    }


}
