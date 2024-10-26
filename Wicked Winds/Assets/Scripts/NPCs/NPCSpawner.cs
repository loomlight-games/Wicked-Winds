using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab; // Prefab del NPC
    public int npcCount = 10; // Número total de NPCs a generar
    public float detectionRadius = 100f; // Radio para detectar el suelo
    public LayerMask groundLayer; // Capa del suelo
    public LayerMask buildingLayer; // Capa de edificios

    void Start()
    {
        Debug.Log("Iniciando el proceso de generación de NPCs.");

        for (int i = 0; i < npcCount; i++)
        {
            Debug.Log($"Generando NPC {i + 1} de {npcCount}.");
            SpawnNPC();
        }

        //Debug.Log("Generación de NPCs completada.");
    }

    void SpawnNPC()
    {
        Vector3 spawnPosition = GetRandomPositionOnGround();
        if (spawnPosition != Vector3.zero)
        {
            //Debug.Log($"Posición de generación válida encontrada en {spawnPosition}.");
            GameObject npc = Instantiate(npcPrefab, spawnPosition, Quaternion.identity);
            NPC npcComponent = npc.GetComponent<NPC>();

            // Asigna si el NPC tiene una misión
            bool hasMission = Random.value > 0.5f;
            npcComponent.hasMission = hasMission;
           // Debug.Log($"NPC generado en {spawnPosition} con misión: {hasMission}");
        }
        else
        {
            //Debug.LogWarning("No se encontró una posición válida para generar un NPC.");
        }
    }

    Vector3 GetRandomPositionOnGround()
    {
        Vector3 randomPosition = Vector3.zero;
        //Debug.Log("Buscando posición aleatoria en el suelo.");

        for (int i = 0; i < 30; i++) // Intentar 30 veces encontrar una posición adecuada
        {
            Vector3 randomPoint = new Vector3(
                Random.Range(-detectionRadius, detectionRadius),
                100f,
                Random.Range(-detectionRadius, detectionRadius)
            );
            //Debug.Log($"Intento {i + 1}: Punto aleatorio generado en {randomPoint}.");

            // Raycast desde arriba hacia abajo para detectar el suelo
            if (Physics.Raycast(randomPoint, Vector3.down, out RaycastHit hit, Mathf.Infinity, groundLayer))
            {
               // Debug.Log($"Suelo detectado en {hit.point}.");

                // Verificar si el punto detectado no está sobre un edificio
                Collider[] hitColliders = Physics.OverlapSphere(hit.point, 0.5f, buildingLayer);
                if (hitColliders.Length == 0) // Si no hay edificios, es un punto válido
                {
                   // Debug.Log($"Posición válida encontrada en {hit.point}, sin colisiones de edificios.");
                    randomPosition = hit.point;
                    break;
                }
                else
                {
                   // Debug.Log("Posición inválida debido a la presencia de un edificio.");
                }
            }
            else
            {
              //  Debug.Log("No se detectó suelo en esta posición.");
            }
        }

        if (randomPosition == Vector3.zero)
        {
            Debug.LogWarning("No se encontró una posición válida después de 30 intentos.");
        }

        return randomPosition;
    }
}
