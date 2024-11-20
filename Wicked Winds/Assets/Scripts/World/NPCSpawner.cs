using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab; // Prefab del NPC
    public GameObject catPrefab; // Prefab del gato
    public int npcCount = 30; // Número total de NPCs a generar
    public float detectionRadius = 100f; // Radio para detectar el suelo
    public LayerMask groundLayer; // Capa del suelo
    public LayerMask buildingLayer; // Capa de edificios
    public LayerMask waterLayer; // Capa de agua
    public int numOfTries = 30;
    private int spawnedNPCCount = 0; // Variable para contar el número de NPCs generados

    void Start()
    {

        for (int i = 0; i < npcCount; i++)
        {

            SpawnNPC();
        }
        // Mostrar el total de NPCs generados en la consola después de completar la generación
        Debug.Log($"Total de NPCs generados: {spawnedNPCCount}");
    }

    void SpawnNPC()
    {
        Vector3 spawnPosition = GetRandomPositionOnGround();
        if (spawnPosition != Vector3.zero)
        {
            GameObject npc = Instantiate(npcPrefab, spawnPosition, Quaternion.identity);
            NPC npcComponent = npc.GetComponent<NPC>();
            npcComponent.hasMission = Random.value > 0.5f;

            // 20% de probabilidad de tener un gato
            if (Random.value < 0.2f)
            {
                Vector3 catPosition = spawnPosition + new Vector3(2f, 5f, 2f);
                GameObject cat = Instantiate(catPrefab, catPosition, Quaternion.identity);
                CatController catController = cat.GetComponent<CatController>();
                npcComponent.cat = catController;
                catController.owner = npcComponent;
            }
            // Incrementar el contador de NPCs generados
            spawnedNPCCount++;
        }
    }

    Vector3 GetRandomPositionOnGround()
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
                    return spawnPoint; // Punto válido
                }
            }
        }

        Debug.LogWarning("No se encontró una posición válida después de varios intentos.");
        return Vector3.zero;
    }
}
