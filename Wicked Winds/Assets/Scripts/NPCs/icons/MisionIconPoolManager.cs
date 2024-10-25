using UnityEngine;

public class MissionIconPoolManager : MonoBehaviour
{
    public static MissionIconPoolManager Instance { get; private set; } // Propiedad estática para acceder a la instancia

    public MissionIcon missionIconPrefab; // Asignar el prefab de MissionIcon en el inspector
    public int poolSize = 10; // Tamaño inicial del pool
    private MissionIconPool missionIconPool;
    public Transform iconPoolObject;

   

    void Start()
    {
        // Verifica que el prefab esté asignado antes de inicializar el pool
        if (missionIconPrefab == null)
        {
            Debug.LogError("MissionIcon prefab no asignado en el inspector");
            return;
        }

        // Inicializar el pool con el prefab y el tamaño inicial
        missionIconPool = new MissionIconPool(missionIconPrefab, poolSize, iconPoolObject);
    }

    // Método para obtener el pool desde otros scripts
    public MissionIconPool GetMissionIconPool()
    {
        return missionIconPool;
    }
}
