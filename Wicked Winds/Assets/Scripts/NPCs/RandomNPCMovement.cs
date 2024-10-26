using UnityEngine;
using UnityEngine.AI;

public class RandomNPCMovement : MonoBehaviour
{
    public float moveRadius = 10f;
    public float detectionRadius = 100f;
    public LayerMask groundLayer;
    public LayerMask buildingLayer;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetRandomDestination();
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            SetRandomDestination();
        }
    }

    public void SetRandomDestination()
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
                // Raycast hacia arriba para asegurarse de que no haya edificios encima
                if (!Physics.Raycast(hit.position, Vector3.up, 10f, buildingLayer))
                {
                    agent.SetDestination(hit.position);

                    // Comprobar que el destino es alcanzable
                    if (agent.pathStatus == NavMeshPathStatus.PathComplete)
                    {
                        return; // Si encuentra un camino válido, establece el destino y sale del método
                    }
                }
            }
        }

       // Debug.LogWarning("No se pudo encontrar un destino válido después de 30 intentos.");
    }
}
