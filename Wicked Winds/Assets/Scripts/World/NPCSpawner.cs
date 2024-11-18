using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab; // Prefab del NPC
    public GameObject catPrefab; // Prefab del gato
    public int npcCount = 10; // Numero total de NPCs a generar
    public float detectionRadius = 100f; // Radio para detectar el suelo
    public LayerMask groundLayer; // Capa del suelo
    public LayerMask buildingLayer; // Capa de edificios
    public LayerMask waterLayer; // Capa de agua
    public int numOfTries = 30;

    void Start()
    {
        Debug.Log("Iniciando el proceso de generacion de NPCs.");

        for (int i = 0; i < npcCount; i++)
        {
            Debug.Log($"Generando NPC {i + 1} de {npcCount}.");
            SpawnNPC();
        }
    }

    void SpawnNPC()
    {
        Vector3 spawnPosition = GetRandomPositionOnGround();
        if (spawnPosition != Vector3.zero)
        {
            // Generar el NPC
            GameObject npc = Instantiate(npcPrefab, spawnPosition, Quaternion.identity);
            NPC npcComponent = npc.GetComponent<NPC>();

            // Asigna si el NPC tiene una mision
            bool hasMission = Random.value > 0.5f;
            npcComponent.hasMission = hasMission;

            // Determinar si el NPC tiene un gato (20% de probabilidad)
            bool hasCat = Random.value < 0.2f; // 20% de probabilidad de tener gato

            if (hasCat)
            {
                // Generar el gato al lado del NPC
                Vector3 catPosition = spawnPosition + new Vector3(2f, 5f, 2f); // Coloca el gato un poco al lado del NPC (ajusta segun sea necesario)
                GameObject cat = Instantiate(catPrefab, catPosition, Quaternion.identity);
                CatController catController = cat.GetComponent<CatController>();

                // Asigna el gato al NPC
                npcComponent.cat = catController;
                catController.owner = npcComponent;
            }
        }
    }

    Vector3 GetRandomPositionOnGround()
    {
        Vector3 randomPosition = Vector3.zero;

        for (int i = 0; i < numOfTries; i++) // Intentar 30 veces encontrar una posicion adecuada
        {
            Vector3 randomPoint = new Vector3(
                Random.Range(-detectionRadius, detectionRadius),
                100f,
                Random.Range(-detectionRadius, detectionRadius)
            );

            // Raycast desde arriba hacia abajo para detectar el suelo
            if (Physics.Raycast(randomPoint, Vector3.down, out RaycastHit hit, Mathf.Infinity, groundLayer))
            {
                // Verificar si el punto detectado no esta sobre un edificio
                Collider[] hitCollidersBuilding = Physics.OverlapSphere(hit.point, 0.5f, buildingLayer);
                Collider[] hitCollidersWater = Physics.OverlapSphere(hit.point, 0.5f, waterLayer);
                if (hitCollidersBuilding.Length == 0 && hitCollidersWater.Length == 0) // Si no hay edificios, es un punto valido
                {
                    randomPosition = hit.point;
                    break;
                }
            }
        }

        if (randomPosition == Vector3.zero)
        {
            Debug.LogWarning("No se encontro una posicion valida despues de 30 intentos.");
        }

        return randomPosition;
    }
}
