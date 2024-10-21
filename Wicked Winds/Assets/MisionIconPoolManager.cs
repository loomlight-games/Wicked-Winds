using UnityEngine;

public class MissionIconPoolManager : MonoBehaviour
{
    public MissionIcon missionIconPrefab; // Asignar el prefab de MissionIcon en el inspector
    public int poolSize = 10; // Tama�o inicial del pool
    private AObjectPool<MissionIcon> missionIconPool;

    void Start()
    {
        // Inicializar el pool con el prefab y tama�o inicial
        missionIconPool = new AObjectPool<MissionIcon>(missionIconPrefab, poolSize);
    }

    // M�todo para obtener el pool desde otros scripts
    public AObjectPool<MissionIcon> GetMissionIconPool()
    {
        return missionIconPool;
    }
}