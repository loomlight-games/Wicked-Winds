using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudPool : MonoBehaviour
{

    [SerializeField] private GameObject cloudPrefab;
    [SerializeField] private int poolSize = 2; //numero de objetos en el objectpool
    [SerializeField] private List<GameObject> cloudList; //lista de objetos del object pull

    private static CloudPool instance;
    public static CloudPool Instance { get { return instance; } } // con el patron singleton hacemos que 
                                                                  //solo tengamos una unica instancia de bulletpool y nos permite acceder más fácilmente a sus metodos
                                                                  // y campos desde otros scripts

    [SerializeField] private float respawnDelay = 2f; // Tiempo antes de reaparecer una nube
    public bool IsUnderCloud = false;



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
        AddCloudsToPool(poolSize);
    }

    private void AddCloudsToPool(int size)
    {
        //primero vamos a instanciar todos los games objects del pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject cloud = Instantiate(cloudPrefab);
            cloud.SetActive(false);
            cloudList.Add(cloud);
            cloud.transform.parent = transform;
            //al principio todas las instancias estan inactivas
        }
    }

    public GameObject GetCloud()
    {


        for (int i = 0; i < cloudList.Count; i++)
        {
            if (!cloudList[i].activeSelf)
            {
                cloudList[i].SetActive(true);
                return cloudList[i];
            }
        }
        return null; //politica de crecimiento = no hay nubes, devuelvo null

    }

    public void ReturnCloud(GameObject cloud)
    {
        cloud.SetActive(false);
        cloud.SetActive(false);
        StartCoroutine(RespawnCloudWithDelay(cloud)); // Manejar el respawn aquí
    }

    private IEnumerator RespawnCloudWithDelay(GameObject cloud)
    {
        yield return new WaitForSeconds(respawnDelay);

        // Reposicionar la nube en un punto inicial
        Vector3 newStartPosition = NPCSpawner.Instance.GetCloudSpawnPosition();
        cloud.transform.position = newStartPosition;
        cloud.SetActive(true);
        cloud.GetComponent<CloudMovement>().SetTargetPosition(); // Reiniciar su objetivo
    }
    
    public bool ChangePlayerSpeed(bool underCloud)
    {
        IsUnderCloud = underCloud;
        return IsUnderCloud;

    }
}
