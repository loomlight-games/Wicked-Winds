using UnityEngine;
using UnityEngine.AI;

public class NPCSpawner : MonoBehaviour
{
    GameObject npcsParent, catsParent, owlsParent, flocksParent;

    public GameObject npcPrefab; // Prefab del NPC
    public GameObject catPrefab; // Prefab del gato
    public GameObject owlPrefab; // Prefab del b�ho (nuevo)

    public int npcCount = 30; // N�mero total de NPCs a generar
    public float detectionRadius = 100f; // Radio para detectar el suelo
    public LayerMask groundLayer; // Capa del suelo
    public int numOfTries = 30; // Intentos m�ximos para buscar una posici�n v�lida

    private int spawnedNPCCount = 0; // Contador de NPCs generados

    //BIRD OBSTACLES
    public GameObject birdPrefab; // Prefab del p�jaro
    public int numberOfFlocks = 20; // N�mero de bandadas de p�jaros a generar
    public int birdsPerFlock = 5; // P�jaros por bandada
    public float flockSpawnRadius = 80f; // Radio para dispersar las bandadas
    public float flockRadius = 15f; // Radio inicial para posicionar a los p�jaros en la bandada
    public float birdHeightOffset = 15f; // Altura fija de las bandadas
    public bool cat;


    public float cloudSpawnRadious = 100f;
    public float cloudHeightOffset = 40;
    public GameObject cloudPrefab;
    public LayerMask buildingLayer; // Capa de edificios
    public LayerMask waterLayer; // Capa de agua




    void Start()
    {
        // Instantiate all the parents
        npcsParent = GameObject.Find("NpcsParent") ?? new GameObject("NpcsParent");
        catsParent = GameObject.Find("CatsParent") ?? new GameObject("CatsParent");
        owlsParent = GameObject.Find("OwlsParent") ?? new GameObject("OwlsParent");
        flocksParent = GameObject.Find("FlocksParent") ?? new GameObject("FlocksParent");

        for (int i = 0; i < npcCount; i++)
        {
            SpawnNPC();
        }
        Debug.Log($"Total de NPCs generados: {spawnedNPCCount}");

        // Generar bandadas de p�jaros
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

        // Buscar una posici�n v�lida en el NavMesh para el NPC
        Vector3 spawnPosition = GetRandomPositionOnGround(humanoidAgentTypeID, false);
        if (spawnPosition != Vector3.zero)
        {
            GameObject npc = Instantiate(npcPrefab, spawnPosition, Quaternion.identity, npcsParent.transform);
            NPC npcComponent = npc.GetComponent<NPC>();
            npcComponent.hasMission = Random.value > 0.5f;

            // Configurar NavMeshAgent
            NavMeshAgent agentNPC = npc.GetComponent<NavMeshAgent>();
            if (agentNPC != null)
            {
                agentNPC.avoidancePriority = spawnedNPCCount; // Prioridad aleatoria para evitar colisiones
                agentNPC.obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance; // Calidad de evasion

            }

            // 30% de probabilidad de generar un gato
            if (Random.value < 0.3f)
            {
                // Obtener el ID del tipo de agente para gatos (Cat)
                int catAgentTypeID = NavMesh.GetSettingsByIndex(1).agentTypeID;

                // Buscar una posici�n v�lida en el NavMesh para el gato
                Vector3 catPosition = GetRandomPositionOnGround(catAgentTypeID, true);
                if (catPosition != Vector3.zero)
                {
                    GameObject catInstance = Instantiate(catPrefab, catPosition, Quaternion.identity, catsParent.transform);
                    CatController catController = catInstance.GetComponent<CatController>();
                    npcComponent.cat = catController;
                    catController.owner = npcComponent;

                    // Configurar NavMeshAgent
                    NavMeshAgent agentCat = catInstance.GetComponent<NavMeshAgent>();
                    if (agentCat != null)
                    {
                        agentCat.avoidancePriority = spawnedNPCCount; // Prioridad aleatoria para evitar colisiones
                        agentCat.obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance; // Calidad de evasi�n

                    }
                }


            }

            // 10% de probabilidad de generar un b�ho
            if (Random.value < 0.3f)
            {
                // Generar el b�ho en cualquier parte del mapa a una altura de 10 unidades
                Vector3 owlPosition = new Vector3(
                    Random.Range(-detectionRadius, detectionRadius),
                    15f, // Altura fija de 10 unidades
                    Random.Range(-detectionRadius, detectionRadius)
                );

                GameObject owl = Instantiate(owlPrefab, owlPosition, Quaternion.identity, owlsParent.transform);
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
        // Generar una posicion central aleatoria para la bandada
        Vector3 flockCenter = new Vector3(
            Random.Range(-flockSpawnRadius, flockSpawnRadius),
            birdHeightOffset,
            Random.Range(-flockSpawnRadius, flockSpawnRadius)
        );

        // Validar que el centro este sobre el terreno usando un raycast
        if (Physics.Raycast(flockCenter + Vector3.up * 100f, Vector3.down, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            flockCenter = hit.point + Vector3.up * birdHeightOffset;
        }

        // Crear un objeto vacio que servira como centro de la bandada y hacerlo hijo del padre
        GameObject flockCenterObject = new GameObject("FlockCenter");
        flockCenterObject.transform.position = flockCenter;
        flockCenterObject.transform.parent = flocksParent.transform;

        // Crear pajaros alrededor del centro y asignarlos como hijos del centro
        for (int i = 0; i < birdsPerFlock; i++)
        {
            Vector3 birdPosition = flockCenter + new Vector3(
                Random.Range(-flockRadius, flockRadius),
                Random.Range(-5f, 10f), // Variaciones menores en la altura inicial
                Random.Range(-flockRadius, flockRadius)
            );

            GameObject bird = Instantiate(birdPrefab, birdPosition, Quaternion.identity);

            // Configurar el pajaro como hijo del centro de su bandada
            bird.transform.parent = flockCenterObject.transform;

            // Obtener el BirdController y asignar dinamicamente el flockCenter
            BirdController birdController = bird.GetComponent<BirdController>();
            if (birdController != null)
            {
                birdController.flockCenter = flockCenterObject.transform; // Asignar el centro
            }

            // Registrar el pajaro en el BirdManager
            BirdManager.Instance.RegisterBird(bird);
        }
    }

    void SpawnCloud()
    {
        // Generar una posici�n central aleatoria para la bandada
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

                // Realizar un raycast hacia arriba desde el punto de spawn para comprobar si hay un edificio
                RaycastHit hitpoint;
                if (Physics.Raycast(spawnPoint, Vector3.up, out hitpoint, Mathf.Infinity, buildingLayer))
                {
                    // Si hay un edificio encima, continuamos con la siguiente iteraci�n
                    continue;
                }

                // Verificar que el punto est� libre de agua o NPCs cercanos
                bool isWaterNearby = Physics.CheckSphere(spawnPoint, 1f, waterLayer);
                bool isNPCNearby = Physics.CheckSphere(spawnPoint, 2f, LayerMask.GetMask("NPC"));

                if (!isWaterNearby && !isNPCNearby)
                {


                    return spawnPoint;


                }
            }
        }


        Debug.LogWarning("No se encontr� una posici�n v�lida despu�s de varios intentos.");
        return Vector3.zero;
    }


    Vector3 GetValidNavMeshPosition(int agentTypeID, Vector3 randomPoint)
    {
        int attempts = 30;  // N�mero de intentos para encontrar una posici�n v�lida
        for (int i = 0; i < attempts; i++)
        {
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
                            return navHit.position; // Retorna la posici�n v�lida si se encuentra
                        }
                    }
                }
            }

            // Si no se encuentra una posici�n v�lida, genera una nueva posici�n aleatoria
            randomPoint = new Vector3(
                Random.Range(-50f, 50f), // Ajusta el rango seg�n lo que necesites
                randomPoint.y,
                Random.Range(-50f, 50f)
            );
        }

        Debug.LogWarning($"No se encontr� una posici�n v�lida en el NavMesh para el agente con ID {agentTypeID} despu�s de {attempts} intentos.");
        return Vector3.zero; // Si no se encuentra ninguna posici�n v�lida despu�s de los intentos, devuelve Vector3.zero
    }
}
