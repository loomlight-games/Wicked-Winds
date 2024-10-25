using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class MissionObjectiveSpawner : MonoBehaviour
{
    public List<GameObject> ingredientPrefabs; // Lista de prefabs para los ingredientes
    public float minSpawnRadius = 10f; // Distancia mínima de spawn
    public float maxSpawnRadius = 1000f; // Distancia máxima de spawn
    public LayerMask buildingLayerMask; // LayerMask para los edificios
    public LayerMask groundLayerMask; // LayerMask para el suelo
    public float heightOffset = 0.5f; // Desplazamiento hacia arriba para la posición final

    private static MissionObjectiveSpawner instance;
    public static MissionObjectiveSpawner Instance { get { return instance; } }

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

    public GameObject[] SpawnIngredients(Vector3 centerPosition, int count)
    {
        List<GameObject> spawnedIngredients = new List<GameObject>(); // Lista temporal para almacenar los objetos generados

        for (int i = 0; i < count; i++)
        {
            // Seleccionar un prefab aleatorio de la lista
            GameObject randomPrefab = ingredientPrefabs[Random.Range(0, ingredientPrefabs.Count)];
            Vector3 randomPos = Vector3.zero; // Inicializar randomPos

            bool validPosition = false;

            while (!validPosition)
            {
                // Generar un vector de dirección aleatorio
                Vector3 randomDirection = Random.insideUnitSphere;

                // Elegir una distancia aleatoria entre el radio mínimo y máximo
                float spawnDistance = Random.Range(minSpawnRadius, maxSpawnRadius);

                // Calcular la posición final del spawn
                randomPos = centerPosition + randomDirection.normalized * spawnDistance;

                // Asegura que el objeto se genera en el mismo plano
                randomPos.y = centerPosition.y;

                // Verificar si hay un edificio debajo usando un Raycast
                RaycastHit buildingHit;
                bool isBuildingBelow = Physics.Raycast(randomPos, Vector3.down, out buildingHit, Mathf.Infinity, buildingLayerMask);

                // Si no hay edificio debajo, verificar si hay suelo
                if (!isBuildingBelow)
                {
                    RaycastHit groundHit;
                    bool isGroundBelow = Physics.Raycast(randomPos, Vector3.down, out groundHit, Mathf.Infinity, groundLayerMask);

                    // Si hay suelo debajo, se puede instanciar el objeto
                    if (isGroundBelow)
                    {
                        randomPos.y = groundHit.point.y + heightOffset; // Ajustar la posición para estar encima del suelo

                        // Verificar que no hay edificios en la posición final
                        RaycastHit finalBuildingHit;
                        bool isFinalBuildingBelow = Physics.Raycast(randomPos, Vector3.down, out finalBuildingHit, Mathf.Infinity, buildingLayerMask);

                        if (!isFinalBuildingBelow)
                        {
                            validPosition = true; // Se encontró una posición válida
                        }
                    }
                }
            }

            // Instanciar el prefab seleccionado y agregarlo a la lista
            GameObject spawnedIngredient = Instantiate(randomPrefab, randomPos, Quaternion.identity);
            spawnedIngredients.Add(spawnedIngredient); // Agregar a la lista
        }

        return spawnedIngredients.ToArray(); // Convertir la lista a un arreglo y devolverlo
    }


}
