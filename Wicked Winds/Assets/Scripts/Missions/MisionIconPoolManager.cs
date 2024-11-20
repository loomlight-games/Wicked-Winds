using UnityEngine;

public class MissionIconPoolManager : MonoBehaviour
{


    public MissionIcon missionIconPrefab; // Asignar el prefab de MissionIcon en el inspector
    public int poolSize = 10; // Tama?o inicial del pool
    private MissionIconPool missionIconPool;
    public Transform iconPoolObject;

    private static MissionIconPoolManager instance;
    public static MissionIconPoolManager Instance { get { return instance; } } // con el patron singleton hacemos que 
    //solo tengamos una unica instancia de bulletpool y nos permite acceder m?s f?cilmente a sus metodos
    // y campos desde otros scripts

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

    void Start()
    {
        // Verifica que el prefab est? asignado antes de inicializar el pool
        if (missionIconPrefab == null)
        {
            Debug.LogError("MissionIcon prefab no asignado en el inspector");
            return;
        }

        // Inicializar el pool con el prefab y el tama?o inicial
        missionIconPool = new MissionIconPool(missionIconPrefab, poolSize, iconPoolObject);
    }

    // M?todo para obtener el pool desde otros scripts
    public MissionIconPool GetMissionIconPool()
    {
        return missionIconPool;
    }
}
