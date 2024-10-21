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
        for (int i = 0; i < npcCount; i++)
        {
            SpawnNPC();
        }
    }

    void SpawnNPC()
    {
        Vector3 spawnPosition = GetRandomPositionOnGround();
        if (spawnPosition != Vector3.zero)
        {
            GameObject npc = Instantiate(npcPrefab, spawnPosition, Quaternion.identity);
            NPC npcComponent = npc.GetComponent<NPC>();

            // Asigna si el NPC tiene una misi�n (puedes personalizar esta l�gica)
            if (Random.value > 0.5f) // 50% de probabilidad de tener misi�n
            {
                npcComponent.hasMission = true; // Asigna misi�n
                // Aqu� puedes asignar la misi�n espec�fica si tienes un sistema de misiones
            }
        }
    }

    Vector3 GetRandomPositionOnGround()
    {
        Vector3 randomPosition = Vector3.zero;

        for (int i = 0; i < 30; i++) // Intentar 30 veces encontrar una posici�n adecuada
        {
            Vector3 randomPoint = new Vector3(
                Random.Range(-detectionRadius, detectionRadius),
                100f,
                Random.Range(-detectionRadius, detectionRadius)
            ); // Generar un punto alto para hacer un raycast hacia abajo

            // Raycast desde arriba hacia abajo para detectar el suelo
            if (Physics.Raycast(randomPoint, Vector3.down, out RaycastHit hit, Mathf.Infinity, groundLayer))
            {
                // Verificar si el punto detectado no est� sobre un edificio
                Collider[] hitColliders = Physics.OverlapSphere(hit.point, 0.5f, buildingLayer);
                if (hitColliders.Length == 0) // Si no hay edificios, es un punto v�lido
                {
                    randomPosition = hit.point;
                    break; // Salir del loop si encontramos una posici�n adecuada
                }
            }
        }

        return randomPosition;
    }
}
