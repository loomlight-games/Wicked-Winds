using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AObjectPool<T> where T : MonoBehaviour, IPoolable
{
    public T prefab; // El prefab del objeto que se reutilizará en el pool
    public int poolSize = 10; // Tamaño del pool
    private List<T> pool; // Lista de objetos en el pool

    // Constructor para inicializar el pool con objetos
    public AObjectPool(T prefab, int poolSize)
    {
        this.prefab = prefab;
        this.poolSize = poolSize;
        pool = new List<T>();

        // Crear los objetos iniciales y desactivarlos
        for (int i = 0; i < poolSize; i++)
        {
            T obj = GameObject.Instantiate(prefab);
            obj.gameObject.SetActive(false);
            pool.Add(obj);
        }
    }

    // Obtener un objeto del pool
    public T GetObject()
    {
        foreach (T obj in pool)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                obj.gameObject.SetActive(true);
                obj.OnObjectSpawn(); // Llama a OnObjectSpawn cuando se toma del pool
                return obj;
            }
        }

        // Si no hay objetos disponibles, instanciar uno nuevo (puedes ajustar este comportamiento)
        T newObj = GameObject.Instantiate(prefab);
        newObj.OnObjectSpawn();
        pool.Add(newObj);
        return newObj;
    }

    // Devolver un objeto al pool
    public void ReturnObject(T obj)
    {
        obj.OnObjectReturn(); // Llama a OnObjectReturn cuando se devuelve al pool
        obj.gameObject.SetActive(false);
    }
}
