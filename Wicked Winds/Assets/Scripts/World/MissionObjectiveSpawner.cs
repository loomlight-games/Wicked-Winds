using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionObjectiveSpawner : MonoBehaviour
{
    public List<GameObject> ingredientPrefabs; // Lista que contiene prefabs de los ingredientes a generar.
    public float minSpawnRadius = 100f; // Distancia mínima desde el centro para generar ingredientes.
    public float maxSpawnRadius = 250f; // Distancia máxima desde el centro para generar ingredientes.
    public LayerMask buildingLayerMask; // Máscara de capa para detectar edificios.
    public LayerMask groundLayerMask; // Máscara de capa para detectar el suelo.
    public float heightOffset = 0.5f; // Desplazamiento vertical para colocar el ingrediente sobre el suelo o edificio.

    private static MissionObjectiveSpawner instance;
    public static MissionObjectiveSpawner Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // Asigna esta instancia como la única.
        }
        else
        {
            Destroy(gameObject); // Destruye el objeto si ya existe una instancia.
        }
    }

    public GameObject[] SpawnIngredients(Vector3 centerPosition, int count)
    {
        List<GameObject> spawnedIngredients = new List<GameObject>(); // Lista para almacenar los ingredientes generados.

        for (int i = 0; i < count; i++) // Itera el número de ingredientes que se quieren generar.
        {
            GameObject randomPrefab = ingredientPrefabs[Random.Range(0, ingredientPrefabs.Count)];
            Vector3 randomPos = Vector3.zero;

            bool validPosition = false; // Bandera para verificar si se encontró una posición válida.
            int attempts = 0; // Contador de intentos para encontrar una posición.
            const int maxAttempts = 10; // Número máximo de intentos para encontrar una posición válida.

            while (!validPosition && attempts < maxAttempts)
            {
                attempts++; // Incrementa el contador de intentos.
                Debug.Log($"Intento {attempts} para el ingrediente {i + 1}/{count}");

                // Genera una dirección aleatoria y una distancia aleatoria.
                Vector3 randomDirection = Random.insideUnitSphere;
                float spawnDistance = Random.Range(minSpawnRadius, maxSpawnRadius);
                randomPos = centerPosition + randomDirection.normalized * spawnDistance; // Calcula la posición aleatoria en función del centro y la dirección.
                randomPos.y = centerPosition.y; // Mantiene la misma altura que el centro.

                // Inicializa una variable para almacenar la altura más alta encontrada.
                float highestPoint = 0f;
                RaycastHit buildingHit;

                // Verifica si hay un edificio en la posición generada.
                if (Physics.Raycast(randomPos + Vector3.up * 10f, Vector3.down, out buildingHit, Mathf.Infinity, buildingLayerMask)) // Haz el raycast desde más arriba.
                {
                    highestPoint = buildingHit.point.y + heightOffset; // Ajusta la altura al tope del edificio.
                    Debug.Log($"Edificio encontrado en {randomPos}. Ajustando altura a {highestPoint}.");
                }
                else
                {
                    // Si no hay edificio, verifica si hay suelo.
                    RaycastHit groundHit;
                    if (Physics.Raycast(randomPos + Vector3.up * 10f, Vector3.down, out groundHit, Mathf.Infinity, groundLayerMask))
                    {
                        highestPoint = groundHit.point.y + heightOffset; // Ajusta la altura al suelo.
                        Debug.Log($"Suelo encontrado en {randomPos}. Ajustando altura a {highestPoint}.");
                    }
                    else
                    {
                        Debug.Log($"No se encontró suelo en {randomPos}. Reintentando...");
                        continue; // Salta a la siguiente iteración del bucle para intentar de nuevo.
                    }
                }

                // Establece la posición final del ingrediente.
                randomPos.y = highestPoint;

                // Asegúrate de que el collider del ingrediente no esté dentro de un edificio.
                if (Physics.CheckBox(randomPos, randomPrefab.GetComponent<Collider>().bounds.extents, Quaternion.identity, buildingLayerMask))
                {
                    Debug.Log($"Posición final para ingrediente {i + 1} está dentro de un edificio. Reintentando...");
                    continue; // Salta a la siguiente iteración del bucle para intentar de nuevo.
                }

                validPosition = true; // Se ha encontrado una posición válida.
            }

            // Si no se encontró una posición válida después de los intentos, muestra una advertencia.
            if (!validPosition)
            {
                Debug.LogWarning($"No se encontró una posición válida para el ingrediente {i + 1} después de {maxAttempts} intentos.");
                continue; // Salta a la siguiente iteración del bucle para el siguiente ingrediente.
            }

            // Instanciar el ingrediente en la posición válida.
            GameObject spawnedIngredient = Instantiate(randomPrefab, randomPos, Quaternion.identity);
            spawnedIngredients.Add(spawnedIngredient); // Agrega el ingrediente a la lista.
            Debug.Log($"Ingrediente {i + 1} instanciado en {randomPos}"); // Muestra un mensaje en la consola indicando que el ingrediente fue creado.
        }

        // Muestra cuántos ingredientes fueron generados exitosamente.
        Debug.Log($"SpawnIngredients: Generados {spawnedIngredients.Count} ingredientes de {count} solicitados.");
        return spawnedIngredients.ToArray(); // Devuelve la lista de ingredientes generados como un arreglo.
    }
}
