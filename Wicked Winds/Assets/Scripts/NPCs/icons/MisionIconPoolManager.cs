using UnityEngine;

public class MissionIconPoolManager : MonoBehaviour
{
    public MissionIcon missionIconPrefab; // Asignar el prefab de MissionIcon en el inspector
    public int poolSize = 10; // Tamaño inicial del pool
    private AObjectPool<MissionIcon> missionIconPool;

    void Start()
    {
        // Inicializar el pool con el prefab y tamaño inicial
        missionIconPool = new AObjectPool<MissionIcon>(missionIconPrefab, poolSize);
    }

    // Método para obtener el pool desde otros scripts
    public AObjectPool<MissionIcon> GetMissionIconPool()
    {
        return missionIconPool;
    }
}