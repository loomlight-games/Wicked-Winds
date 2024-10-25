using UnityEngine;
using UnityEngine.AI;

public class RandomNPCMovement : MonoBehaviour
{
    public float moveRadius = 10f; // Radio de movimiento aleatorio
    public float detectionRadius = 100f; // Radio para detectar si la posici�n es adecuada (dentro del terreno)
    public LayerMask groundLayer; // Capa del suelo
    public LayerMask buildingLayer; // Capa de edificios para evitar que se posicione sobre ellos

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

       /* // Generar una posici�n inicial aleatoria
        Vector3 randomPosition = GetRandomPositionOnGround();
        if (randomPosition != Vector3.zero)
        {
            transform.position = randomPosition;
            Debug.Log($"Posici�n inicial establecida en: {randomPosition}");
        }
        else
        {
            Debug.LogWarning("No se pudo encontrar una posici�n inicial v�lida.");
        }
       */
        // Iniciar el movimiento aleatorio
        SetRandomDestination();
    }

    void Update()
    {
        // Si el NPC ha llegado a su destino, generar una nueva posici�n
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            Debug.Log("NPC ha llegado a su destino. Estableciendo nueva posici�n.");
            SetRandomDestination();
        }
    }


    public void SetRandomDestination()
    {
        // Definir un rango m�nimo y m�ximo desde la posici�n actual para evitar destinos demasiado cercanos
        float minDistance = 5f; // Distancia m�nima para evitar destinos cercanos
        float maxDistance = moveRadius; // Distancia m�xima para el rango de movimiento

        Vector3 randomDirection;
        NavMeshHit hit;

        for (int i = 0; i < 30; i++) // Intentos para encontrar una posici�n v�lida
        {
            // Generar una direcci�n aleatoria
            randomDirection = Random.insideUnitSphere * maxDistance;
            randomDirection += transform.position;

            // Verificar que la nueva posici�n est� en el NavMesh y a una distancia adecuada
            if (NavMesh.SamplePosition(randomDirection, out hit, maxDistance, NavMesh.AllAreas) &&
                Vector3.Distance(transform.position, hit.position) >= minDistance)
            {
                agent.SetDestination(hit.position); // Establecer el destino v�lido
                Debug.Log($"Destino aleatorio establecido en: {hit.position}");
                return; // Salir del m�todo al encontrar una posici�n v�lida
            }
            else
            {
                Debug.Log($"Intento {i + 1}: posici�n aleatoria no v�lida: {randomDirection}");
            }
        }

        Debug.LogWarning("No se pudo encontrar un destino v�lido despu�s de 30 intentos.");
    }


}
