using UnityEngine;

public class MissionIconPoolManager : MonoBehaviour
{
   

    public MissionIcon missionIconPrefab; // Asignar el prefab de MissionIcon en el inspector
    public int poolSize = 10; // Tamaño inicial del pool
    private MissionIconPool missionIconPool;
    public Transform iconPoolObject;

    private static MissionIconPoolManager instance;
    public static MissionIconPoolManager Instance { get { return instance; } } // con el patron singleton hacemos que 
    //solo tengamos una unica instancia de bulletpool y nos permite acceder más fácilmente a sus metodos
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
        // Verifica que el prefab esté asignado antes de inicializar el pool
        if (missionIconPrefab == null)
        {
            Debug.LogError("MissionIcon prefab no asignado en el inspector");
            return;
        }

        // Inicializar el pool con el prefab y el tamaño inicial
        missionIconPool = new MissionIconPool(missionIconPrefab, poolSize, iconPoolObject);
        Debug.Log("MissionIconPool inicializado con éxito.");
    }

    // Método para obtener el pool desde otros scripts
    public MissionIconPool GetMissionIconPool()
    {
        return missionIconPool;
    }
}
