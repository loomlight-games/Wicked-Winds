using UnityEngine;
using UnityEngine.AI;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab; // Prefab del NPC
    public GameObject catPrefab; // Prefab del gato
    public GameObject owlPrefab; // Prefab del búho (nuevo)

    public int npcCount = 30; // Número total de NPCs a generar
    public float detectionRadius = 100f; // Radio para detectar el suelo
    public LayerMask groundLayer; // Capa del suelo
    public int numOfTries = 30; // Intentos máximos para buscar una posición válida

    private int spawnedNPCCount = 0; // Contador de NPCs generados

    //BIRD OBSTACLES
    public GameObject birdPrefab; // Prefab del pájaro
    public int numberOfFlocks = 15; // Número de bandadas de pájaros a generar
    public int birdsPerFlock = 5; // Pájaros por bandada
    public float flockSpawnRadius = 100f; // Radio para dispersar las bandadas
    public float flockRadius = 10f; // Radio inicial para posicionar a los pájaros en la bandada
    public float birdHeightOffset = 15f; // Altura fija de las bandadas
    public bool cat;


    public float cloudSpawnRadious = 100f;
    public float cloudHeightOffset = 40;
    public GameObject cloudPrefab;
    public LayerMask buildingLayer; // Capa de edificios
    public LayerMask waterLayer; // Capa de agua

    


    void Start()
    {
        for (int i = 0; i < npcCount; i++)
        {
            SpawnNPC();
        }
        Debug.Log($"Total de NPCs generados: {spawnedNPCCount}");

        // Generar bandadas de pájaros
        for (int i = 0; i < numberOfFlocks; i++)
        {
            SpawnFlock();
        }

        SpawnCloud();
    }

    void SpawnNPC()
    {
        // Obtener el ID del tipo de agente para NPCs (Humanoid)
        int humanoidAgentTypeID = NavMesh.GetSettingsByIndex(0).agentTypeID;

        // Buscar una posición válida en el NavMesh para el NPC
        Vector3 spawnPosition = GetRandomPositionOnGround(humanoidAgentTypeID, false);
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

                // Buscar una posición válida en el NavMesh para el gato
                Vector3 catPosition = GetRandomPositionOnGround(catAgentTypeID, true);
                if (catPosition != Vector3.zero)
                {
                    GameObject cat = Instantiate(catPrefab, catPosition, Quaternion.identity);
                    CatController catController = cat.GetComponent<CatController>();
                    npcComponent.cat = catController;
                    catController.owner = npcComponent;
                }
            }

            // 10% de probabilidad de generar un búho
            if (Random.value < 0.3f)
            {
                // Generar el búho en cualquier parte del mapa a una altura de 10 unidades
                Vector3 owlPosition = new Vector3(
                    Random.Range(-detectionRadius, detectionRadius),
                    15f, // Altura fija de 10 unidades
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

    //BIRD SPAWNER
    void SpawnFlock()
    {
        // Generar una posición central aleatoria para la bandada
        Vector3 flockCenter = new Vector3(
            Random.Range(-flockSpawnRadius, flockSpawnRadius),
            birdHeightOffset,
            Random.Range(-flockSpawnRadius, flockSpawnRadius)
        );

        // Validar que el centro esté sobre el terreno usando un raycast
        if (Physics.Raycast(flockCenter + Vector3.up * 100f, Vector3.down, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            flockCenter = hit.point + Vector3.up * birdHeightOffset;
        }

        // Asegurar que exista un objeto raíz para todas las bandadas
        GameObject flockParent = GameObject.Find("FlocksParent");
        if (flockParent == null)
        {
            flockParent = new GameObject("FlocksParent");
        }

        // Crear un objeto vacío que servirá como centro de la bandada y hacerlo hijo del padre
        GameObject flockCenterObject = new GameObject("FlockCenter");
        flockCenterObject.transform.position = flockCenter;
        flockCenterObject.transform.parent = flockParent.transform;

        // Crear pájaros alrededor del centro y asignarlos como hijos del centro
        for (int i = 0; i < birdsPerFlock; i++)
        {
            Vector3 birdPosition = flockCenter + new Vector3(
                Random.Range(-flockRadius, flockRadius),
                Random.Range(-1f, 1f), // Variaciones menores en la altura inicial
                Random.Range(-flockRadius, flockRadius)
            );

            GameObject bird = Instantiate(birdPrefab, birdPosition, Quaternion.identity);

            // Configurar el pájaro como hijo del centro de su bandada
            bird.transform.parent = flockCenterObject.transform;

            // Obtener el BirdController y asignar dinámicamente el flockCenter
            BirdController birdController = bird.GetComponent<BirdController>();
            if (birdController != null)
            {
                birdController.flockCenter = flockCenterObject.transform; // Asignar el centro
            }

            // Registrar el pájaro en el BirdManager
            BirdManager.Instance.RegisterBird(bird);
        }
    }

    void SpawnCloud()
    {
        // Generar una posición central aleatoria para la bandada
        Vector3 position = new Vector3(
            Random.Range(-cloudSpawnRadious, cloudSpawnRadious),
            cloudHeightOffset,
            Random.Range(-cloudSpawnRadious, cloudSpawnRadious)
        );

      
                position = GetRandomPositionOnGround(0, false);
                GameObject cloud = Instantiate(cloudPrefab, position, Quaternion.identity);
                PlayerManager.Instance.cloudTransform = cloud.transform;
            
        }




    Vector3 GetRandomPositionOnGround(int agentTypeID, bool cat)
    {
        for (int i = 0; i < numOfTries; i++)
        {
            Vector3 randomPoint = new Vector3(
                Random.Range(-detectionRadius, detectionRadius),
                100f,
                Random.Range(-detectionRadius, detectionRadius)
            );

            if (Physics.Raycast(randomPoint, Vector3.down, out RaycastHit hit, Mathf.Infinity, groundLayer))
            {
                Vector3 spawnPoint = hit.point;

                // Comprobar si hay un edificio justo encima del punto
                if (Physics.Raycast(spawnPoint, Vector3.up, 50f, buildingLayer))
                {

                    continue; // Saltar a la siguiente iteración
                }

                // Verificar que el punto esté libre de agua o NPCs cercanos
                bool isWaterNearby = Physics.CheckSphere(spawnPoint, 1f, waterLayer);
                bool isNPCNearby = Physics.CheckSphere(spawnPoint, 2f, LayerMask.GetMask("NPC"));

                if (!isWaterNearby && !isNPCNearby)
                {
                    
                    if (cat)
                    {
                        return GetValidNavMeshPosition(agentTypeID, spawnPoint); ; // Punto válido
                    }
                    else
                    {
                        return spawnPoint; 
                    }
                   
                }
            }
        }
        

        Debug.LogWarning("No se encontró una posición válida después de varios intentos.");
        return Vector3.zero;
    }


    Vector3 GetValidNavMeshPosition(int agentTypeID, Vector3 randomPoint)
    {
        int attempts = 30;  // Número de intentos para encontrar una posición válida
        for (int i = 0; i < attempts; i++)
        {
            // Realizar un raycast hacia abajo para detectar el suelo
            if (Physics.Raycast(randomPoint, Vector3.down, out RaycastHit hit, Mathf.Infinity, groundLayer))
            {
                Vector3 potentialPosition = hit.point;

                // Comprobar si el punto está en el NavMesh para el agente especificado
                if (NavMesh.SamplePosition(potentialPosition, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas))
                {
                    // Validar que el tipo de agente sea compatible con el NavMesh en esa posición
                    if (navHit.mask != 0 && navHit.hit && navHit.distance <= 1.0f)
                    {
                        NavMeshQueryFilter filter = new NavMeshQueryFilter
                        {
                            agentTypeID = agentTypeID,
                            areaMask = NavMesh.AllAreas // Puedes limitar esto a áreas específicas si es necesario
                        };

                        // Verificar que realmente pertenece al NavMesh del agente
                        if (NavMesh.FindClosestEdge(navHit.position, out NavMeshHit edgeHit, filter))
                        {
                            return navHit.position; // Retorna la posición válida si se encuentra
                        }
                    }
                }
            }

            // Si no se encuentra una posición válida, genera una nueva posición aleatoria
            randomPoint = new Vector3(
                Random.Range(-50f, 50f), // Ajusta el rango según lo que necesites
                randomPoint.y,
                Random.Range(-50f, 50f)
            );
        }

        Debug.LogWarning($"No se encontró una posición válida en el NavMesh para el agente con ID {agentTypeID} después de {attempts} intentos.");
        return Vector3.zero; // Si no se encuentra ninguna posición válida después de los intentos, devuelve Vector3.zero
    }
}
