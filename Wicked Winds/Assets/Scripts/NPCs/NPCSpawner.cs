using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab; // Prefab del NPC
    public int npcCount = 10; // N�mero total de NPCs a generar
    public float detectionRadius = 100f; // Radio para detectar el suelo
    public LayerMask groundLayer; // Capa del suelo
    public LayerMask buildingLayer; // Capa de edificios

    void Start()
    {
        Debug.Log("Iniciando el proceso de generaci�n de NPCs.");

        for (int i = 0; i < npcCount; i++)
        {
            Debug.Log($"Generando NPC {i + 1} de {npcCount}.");
            SpawnNPC();
        }

        //Debug.Log("Generaci�n de NPCs completada.");
    }

    void SpawnNPC()
    {
        Vector3 spawnPosition = GetRandomPositionOnGround();
        if (spawnPosition != Vector3.zero)
        {
            //Debug.Log($"Posici�n de generaci�n v�lida encontrada en {spawnPosition}.");
            GameObject npc = Instantiate(npcPrefab, spawnPosition, Quaternion.identity);
            NPC npcComponent = npc.GetComponent<NPC>();

            // Asigna si el NPC tiene una misi�n
            bool hasMission = Random.value > 0.5f;
            npcComponent.hasMission = hasMission;
           // Debug.Log($"NPC generado en {spawnPosition} con misi�n: {hasMission}");
        }
        else
        {
            //Debug.LogWarning("No se encontr� una posici�n v�lida para generar un NPC.");
        }
    }

    Vector3 GetRandomPositionOnGround()
    {
        Vector3 randomPosition = Vector3.zero;
        //Debug.Log("Buscando posici�n aleatoria en el suelo.");

        for (int i = 0; i < 30; i++) // Intentar 30 veces encontrar una posici�n adecuada
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

                // Verificar si el punto detectado no est� sobre un edificio
                Collider[] hitColliders = Physics.OverlapSphere(hit.point, 0.5f, buildingLayer);
                if (hitColliders.Length == 0) // Si no hay edificios, es un punto v�lido
                {
                   // Debug.Log($"Posici�n v�lida encontrada en {hit.point}, sin colisiones de edificios.");
                    randomPosition = hit.point;
                    break;
                }
                else
                {
                   // Debug.Log("Posici�n inv�lida debido a la presencia de un edificio.");
                }
            }
            else
            {
              //  Debug.Log("No se detect� suelo en esta posici�n.");
            }
        }

        if (randomPosition == Vector3.zero)
        {
            Debug.LogWarning("No se encontr� una posici�n v�lida despu�s de 30 intentos.");
        }

        return randomPosition;
    }
}
