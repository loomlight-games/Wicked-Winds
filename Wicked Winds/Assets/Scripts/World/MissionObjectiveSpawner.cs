using System.Collections.Generic;
using UnityEngine;

public class MissionObjectiveSpawner : MonoBehaviour
{
    public List<GameObject> ingredientPrefabs; // Lista que contiene prefabs de los ingredientes a generar.
    public float minSpawnRadius = 100f,// Distancia minima desde el centro para generar ingredientes.
                 spawnDistance,
                 maxSpawnRadius = 250f,// Distancia maxima desde el centro para generar ingredientes.
                 heightOffset = 0.5f;  // Desplazamiento vertical para colocar el ingrediente sobre el suelo o edificio.

    public LayerMask buildingLayerMask, // Mascara de capa para detectar edificios.
                     waterLayerMask,// Mascara de capa para detectar agua.
                     groundLayerMask; // Mascara de capa para detectar el suelo.

    GameObject ingredientsParent;
    const int maxAttempts = 30; // Numero maximo de intentos para encontrar una posicion valida.
    bool validPosition;
    int attempts;
    GameObject randomPrefab;
    Vector3 randomPos, randomDirection;

    private static MissionObjectiveSpawner instance;
    public static MissionObjectiveSpawner Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // Asigna esta instancia como la unica.
        }
        else
        {
            Destroy(gameObject); // Destruye el objeto si ya existe una instancia.
        }
    }

    private void Start()
    {
        ingredientsParent = GameObject.Find("IngredientsParent") ?? new GameObject("IngredientsParent");
    }

    public GameObject[] SpawnIngredients(Vector3 centerPosition, int count)
    {
        List<GameObject> spawnedIngredients = new List<GameObject>(); // Lista para almacenar los ingredientes generados.

        for (int i = 0; i < count; i++) // Itera el numero de ingredientes que se quieren generar.
        {
            randomPrefab = ingredientPrefabs[Random.Range(0, ingredientPrefabs.Count)];
            randomPos = Vector3.zero;

            validPosition = false; // Bandera para verificar si se encontro una posicion valida.
            attempts = 0; // Contador de intentos para encontrar una posicion.


            while (!validPosition && attempts < maxAttempts)
            {
                attempts++; // Incrementa el contador de intentos.


                // Genera una direccion aleatoria y una distancia aleatoria.
                randomDirection = Random.insideUnitSphere;
                float spawnDistance = Random.Range(minSpawnRadius, maxSpawnRadius);
                randomPos = centerPosition + randomDirection.normalized * spawnDistance; // Calcula la posicion aleatoria en funcion del centro y la direccion.
                randomPos.y = centerPosition.y; // Mantiene la misma altura que el centro.

                // Inicializa una variable para almacenar la altura mas alta encontrada.
                float highestPoint = 0f;
                RaycastHit buildingHit;

                if (Physics.Raycast(randomPos + Vector3.up * 10f, Vector3.down, out buildingHit, Mathf.Infinity, waterLayerMask))
                {
                    continue;
                }
                // Verifica si hay un edificio en la posicion generada.
                if (Physics.Raycast(randomPos + Vector3.up * 10f, Vector3.down, out buildingHit, Mathf.Infinity, buildingLayerMask)) // Haz el raycast desde mas arriba.
                {
                    highestPoint = buildingHit.point.y + heightOffset;

                }

                else
                {

                    RaycastHit groundHit;
                    if (Physics.Raycast(randomPos + Vector3.up * 10f, Vector3.down, out groundHit, Mathf.Infinity, groundLayerMask))
                    {
                        highestPoint = groundHit.point.y + heightOffset; // Ajusta la altura al suelo.
                    }
                    else
                    {
                        continue;
                    }
                }
                // Establece la posicion final del ingrediente.
                randomPos.y = highestPoint;
                // Asegurate de que el collider del ingrediente no esta dentro de un edificio.
                if (Physics.CheckBox(randomPos, randomPrefab.GetComponent<Collider>().bounds.extents, Quaternion.identity, buildingLayerMask))
                {
                    continue;
                }
                validPosition = true;
            }
            if (!validPosition)
            {
                Debug.LogWarning($"No se encontro una posicion valida para el ingrediente {i + 1} despues de {maxAttempts} intentos.");
                continue;
            }
            GameObject spawnedIngredient = Instantiate(randomPrefab, randomPos, Quaternion.identity, ingredientsParent.transform);
            spawnedIngredients.Add(spawnedIngredient);
        }
        return spawnedIngredients.ToArray();
    }
}
