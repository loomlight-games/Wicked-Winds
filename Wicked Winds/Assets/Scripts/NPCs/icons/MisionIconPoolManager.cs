using UnityEngine;

public class MissionIconPoolManager : MonoBehaviour
{
    public static MissionIconPoolManager Instance { get; private set; } // Propiedad est�tica para acceder a la instancia

    public MissionIcon missionIconPrefab; // Asignar el prefab de MissionIcon en el inspector
    public int poolSize = 10; // Tama�o inicial del pool
    private MissionIconPool missionIconPool;
    public Transform iconPoolObject;

   

    void Start()
    {
        // Verifica que el prefab est� asignado antes de inicializar el pool
        if (missionIconPrefab == null)
        {
            Debug.LogError("MissionIcon prefab no asignado en el inspector");
            return;
        }

        // Inicializar el pool con el prefab y el tama�o inicial
        missionIconPool = new MissionIconPool(missionIconPrefab, poolSize, iconPoolObject);
    }

    // M�todo para obtener el pool desde otros scripts
    public MissionIconPool GetMissionIconPool()
    {
        return missionIconPool;
    }
}
