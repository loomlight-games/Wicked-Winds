using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconPool : MonoBehaviour, IObjectPool
{
    //La clase IconPool es responsable de crear, activar y desactivar los objetos Icon.
    //Debe ser capaz de devolver un objeto Icon del pool y devolverlo cuando ya no se necesite.

    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private int poolSize = 5;
    [SerializeField] private List<GameObject> iconList;

    private static IconPool instance;
    public static IconPool Instance { get { return instance; } }

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
        // Inicializar el pool
        AddIconsToPool(poolSize);
    }

    private void AddIconsToPool(int size)
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject icon = Instantiate(iconPrefab);
            icon.SetActive(false);
            iconList.Add(icon);
            icon.transform.parent = transform;
        }
    }

    public IPoolableObject get()
    {
        foreach (var icon in iconList)
        {
            if (!icon.activeSelf)
            {
                icon.SetActive(true);
                return icon.GetComponent<IPoolableObject>();
            }
        }
        return null;  // Si no hay íconos disponibles, devuelve null
    }

    public void release(IPoolableObject obj)
    {
        if (obj is Icon icon) // Asegúrate de que el objeto es un Icon
        {
            icon.gameObject.SetActive(false); // Desactiva el GameObject
            icon.Reset(); // Reinicia el estado del ícono
                          // Aquí podrías agregar cualquier otra lógica que necesites
        }
    }

    public void ReleaseIcon(GameObject icon)
    {
        release(icon.GetComponent<IPoolableObject>());
    }
}
