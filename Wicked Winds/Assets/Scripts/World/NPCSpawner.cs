using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    //hacer en vez de arrastrar aqui addresable, en la ropa de alvaro esta
    public GameObject npcPrefab; // Prefab del NPC
    public int npcCount = 10; // N�mero total de NPCs a generar
    public float detectionRadius = 100f; // Radio para detectar el suelo
    public LayerMask groundLayer; // Capa del suelo
    public LayerMask buildingLayer; // Capa de edificios
    public LayerMask waterLayer;
    public int numOfTries = 30;
    //falta transform padre para generar los npcs dentro de un objeti vacio, 4 componente de instanciate

    void Start()
    {
        Debug.Log("Iniciando el proceso de generaci�n de NPCs.");

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
            
            GameObject npc = Instantiate(npcPrefab, spawnPosition, Quaternion.identity);
            NPC npcComponent = npc.GetComponent<NPC>();

            // Asigna si el NPC tiene una misi�n
            bool hasMission = Random.value > 0.5f;
            npcComponent.hasMission = hasMission;
           
        }
      
    }


    /// <summary>
    /// Busca una posici�n aleatoria en el suelo que sea v�lida (que no tenga edificios)
    /// </summary>
    /// <returns></returns>
    Vector3 GetRandomPositionOnGround()
    {
        Vector3 randomPosition = Vector3.zero;

        for (int i = 0; i < numOfTries; i++) // Intentar 30 veces encontrar una posici�n adecuada
        {
            Vector3 randomPoint = new Vector3(
                Random.Range(-detectionRadius, detectionRadius),
                100f,
                Random.Range(-detectionRadius, detectionRadius)
            );
          

            // Raycast desde arriba hacia abajo para detectar el suelo
            if (Physics.Raycast(randomPoint, Vector3.down, out RaycastHit hit, Mathf.Infinity, groundLayer))
            {
               

                // Verificar si el punto detectado no est� sobre un edificio
                Collider[] hitCollidersBuilding = Physics.OverlapSphere(hit.point, 0.5f, buildingLayer);
                Collider[] hitCollidersWater = Physics.OverlapSphere(hit.point, 0.5f, waterLayer);
                if (hitCollidersBuilding.Length == 0 && hitCollidersWater.Length == 0) // Si no hay edificios, es un punto v�lido
                {
             
                    randomPosition = hit.point;
                    break;
                }


              
            }
         
        }

        if (randomPosition == Vector3.zero)
        {
            Debug.LogWarning("No se encontr� una posici�n v�lida despu�s de 30 intentos.");
        }

        return randomPosition;
    }
}
