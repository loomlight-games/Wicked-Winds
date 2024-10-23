using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconPool : MonoBehaviour, IObjectPool
{
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
        return null;  // Política de crecimiento: si no hay balas disponibles, devuelvo null
    }

    public void release(IPoolableObject icon)
    {
        icon.setActive(false);
        icon.reset();  // Opcionalmente, resetea el estado de la bala antes de devolverla al pool
    }

    // Método específico para desactivar la bala (manteniendo compatibilidad con el código existente)
    public void iconRelease(GameObject icon)
    {
        release(icon.GetComponent<IPoolableObject>());
    }
}