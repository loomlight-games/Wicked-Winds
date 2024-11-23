using UnityEngine;
using UnityEngine.AI;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab; // Prefab del NPC
    public GameObject catPrefab; // Prefab del gato
    public GameObject owlPrefab; // Prefab del b�ho (nuevo)
    public int npcCount = 30; // N�mero total de NPCs a generar
    public float detectionRadius = 100f; // Radio para detectar el suelo
    public LayerMask groundLayer; // Capa del suelo
    public int numOfTries = 30; // Intentos m�ximos para buscar una posici�n v�lida

    private int spawnedNPCCount = 0; // Contador de NPCs generados

    void Start()
    {
        for (int i = 0; i < npcCount; i++)
        {
            SpawnNPC();
        }
        Debug.Log($"Total de NPCs generados: {spawnedNPCCount}");
    }

    void SpawnNPC()
    {
        // Obtener el ID del tipo de agente para NPCs (Humanoid)
        int humanoidAgentTypeID = NavMesh.GetSettingsByIndex(0).agentTypeID;

        // Buscar una posici�n v�lida en el NavMesh para el NPC
        Vector3 spawnPosition = GetValidNavMeshPosition(humanoidAgentTypeID);
        if (spawnPosition != Vector3.zero)
        {
            GameObject npc = Instantiate(npcPrefab, spawnPosition, Quaternion.identity);
            NPC npcComponent = npc.GetComponent<NPC>();
            npcComponent.hasMission = Random.value > 0.5f;

            // 30% de probabilidad de generar un gato
            if (Random.value < 0.3f)
            {
                // Obtener el ID del tipo de agente para gatos (Cat)
                int catAgentTypeID = NavMesh.GetSettingsByIndex(1).agentTypeID;

                // Buscar una posici�n v�lida en el NavMesh para el gato
                Vector3 catPosition = GetValidNavMeshPosition(catAgentTypeID);
                if (catPosition != Vector3.zero)
                {
                    GameObject cat = Instantiate(catPrefab, catPosition, Quaternion.identity);
                    CatController catController = cat.GetComponent<CatController>();
                    npcComponent.cat = catController;
                    catController.owner = npcComponent;
                }
            }

            // 10% de probabilidad de generar un b�ho
            if (Random.value < 0.1f)
            {
                // Generar el b�ho en cualquier parte del mapa a una altura de 10 unidades
                Vector3 owlPosition = new Vector3(
                    Random.Range(-detectionRadius, detectionRadius),
                    10f, // Altura fija de 10 unidades
                    Random.Range(-detectionRadius, detectionRadius)
                );

                GameObject owl = Instantiate(owlPrefab, owlPosition, Quaternion.identity);
                OwlController owlController = owl.GetComponent<OwlController>();
                npcComponent.owl = owlController;
                owlController.owner = npcComponent;
            }

            spawnedNPCCount++;
        }
    }

    Vector3 GetValidNavMeshPosition(int agentTypeID)
    {
        for (int i = 0; i < numOfTries; i++)
        {
            // Generar un punto aleatorio dentro del rango
            Vector3 randomPoint = new Vector3(
                Random.Range(-detectionRadius, detectionRadius),
                100f,
                Random.Range(-detectionRadius, detectionRadius)
            );

            // Realizar un raycast hacia abajo para detectar el suelo
            if (Physics.Raycast(randomPoint, Vector3.down, out RaycastHit hit, Mathf.Infinity, groundLayer))
            {
                Vector3 potentialPosition = hit.point;

                // Comprobar si el punto est� en el NavMesh para el agente especificado
                if (NavMesh.SamplePosition(potentialPosition, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas))
                {
                    // Validar que el tipo de agente sea compatible con el NavMesh en esa posici�n
                    if (navHit.mask != 0 && navHit.hit && navHit.distance <= 1.0f)
                    {
                        NavMeshQueryFilter filter = new NavMeshQueryFilter
                        {
                            agentTypeID = agentTypeID,
                            areaMask = NavMesh.AllAreas // Puedes limitar esto a �reas espec�ficas si es necesario
                        };

                        // Verificar que realmente pertenece al NavMesh del agente
                        if (NavMesh.FindClosestEdge(navHit.position, out NavMeshHit edgeHit, filter))
                        {
                            return navHit.position;
                        }
                    }
                }
            }
        }

        Debug.LogWarning($"No se encontr� una posici�n v�lida en el NavMesh para el agente con ID {agentTypeID}.");
        return Vector3.zero; // Si no encuentra una posici�n v�lida, devuelve un vector vac�o
    }
}
