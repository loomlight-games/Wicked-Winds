using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCSpawner : MonoBehaviour
{
    GameObject npcsParent, catsParent, owlsParent, flocksParent;

    public GameObject npcPrefab,
    catPrefab,
    birdPrefab,
    owlPrefab,
    cloudPrefab;

    public int npcCount = 50,
        numOfTries = 30, // To find valid position
        numberOfFlocks = 15,
        birdsPerFlock = 5,
        numOfClouds = 2;

    public float detectionRadius = 50f, // Radious to detect ground
        flockSpawnRadius = 80f,// Radio para dispersar las bandadas
        flockRadius = 15f, // Radio inicial para posicionar a los p�jaros en la bandada
        birdHeightOffset = 15f, // Altura fija de las bandadas
        cloudSpawnRadious = 100f,
        cloudHeightOffset = 40;
    private static NPCSpawner instance;
    public static NPCSpawner Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public LayerMask buildingLayer, waterLayer, groundLayer;
    int spawnedNPCCount = 0;

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

        for (int i = 0; i < numberOfFlocks; i++)
        {
            SpawnFlock();
        }

        // Iniciar corutina para spawnear nubes
        StartCoroutine(SpawnCloudsWithDelay());
    }

    IEnumerator SpawnCloudsWithDelay()
    {
        for (int i = 0; i < numOfClouds; i++)
        {
            SpawnCloud();
            yield return new WaitForSeconds(50f); // Esperar 50 segundos antes de spawnear la siguiente nube
        }
    }

    void SpawnNPC()
    {
        int humanoidAgentTypeID = NavMesh.GetSettingsByIndex(0).agentTypeID;

        Vector3 spawnPosition = GetRandomPositionOnGround(humanoidAgentTypeID, false);
        if (spawnPosition != Vector3.zero)
        {
            GameObject npc = Instantiate(npcPrefab, spawnPosition, Quaternion.identity, npcsParent.transform);
            NPC npcComponent = npc.GetComponent<NPC>();

            if (npcComponent == null)
                return;

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
                SpawnCat(npcComponent);
            }

            // 10% de probabilidad de generar un b�ho
            if (Random.value < 0.1f)
            {
                SpawnOwl(npcComponent);
               
            }
            spawnedNPCCount++;
        }
    }

    public void SpawnOwl(NPC npc)
    {
        Vector3 owlPosition = new Vector3(
                           Random.Range(-detectionRadius, detectionRadius),
                           15f, // Altura fija de 10 unidades
                           Random.Range(-detectionRadius, detectionRadius)
                       );

        GameObject owl = Instantiate(owlPrefab, owlPosition, Quaternion.identity, owlsParent.transform);
        OwlController owlController = owl.GetComponent<OwlController>();
        npc.owl = owlController;
        owlController.owner = npc;
    }

    public void SpawnCat(NPC npc)
    {
        // Obtener el ID del tipo de agente para gatos (Cat)
        int catAgentTypeID = NavMesh.GetSettingsByIndex(1).agentTypeID;


        Vector3 catPosition = GetRandomPositionOnGround(catAgentTypeID, true);
        if (catPosition != Vector3.zero)
        {
            GameObject catInstance = Instantiate(catPrefab, catPosition, Quaternion.identity, catsParent.transform);
            CatController catController = catInstance.GetComponent<CatController>();
            npc.cat = catController;
            catController.owner = npc;

            // Configurar NavMeshAgent
            NavMeshAgent agentCat = catInstance.GetComponent<NavMeshAgent>();
            if (agentCat != null)
            {
                agentCat.avoidancePriority = spawnedNPCCount; // Prioridad aleatoria para evitar colisiones
                agentCat.obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance;

            }
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
        flockCenterObject.tag = "Flock";
        BirdManager.Instance.flocks.Add(flockCenterObject);

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


        }
    }


    public Vector3 GetCloudSpawnPosition()
    {
        Vector3 position = new Vector3(
            -100, // Fijo en X
            cloudHeightOffset, // Fijo en Y
            Random.Range(-cloudSpawnRadious, cloudSpawnRadious) // Aleatorio en Z
        );

        return position;
    }

    void SpawnCloud()
    {
        Vector3 position = GetCloudSpawnPosition();
        GameObject cloud = CloudPool.Instance.GetCloud();
        if (cloud != null)
        {
            cloud.transform.position = position;
            cloud.SetActive(true);
        }
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


        Debug.LogWarning("No se encontro una posicion valida despues de varios intentos.");
        return Vector3.zero;
    }


}
