using System.Collections.Generic;
using UnityEngine;

public class MissionIconPool
{
    private MissionIcon prefab;
    private List<MissionIcon> pool;
    private Transform iconPoolParent; // Almacena la referencia al GameObject del pool

    // Constructor que acepta el prefab y el GameObject del pool
    public MissionIconPool(MissionIcon prefab, int initialSize, Transform parent)
    {
        this.prefab = prefab;
        this.iconPoolParent = parent; // Asigna el padre
        pool = new List<MissionIcon>();

        // Inicializa el pool con el tamaño dado
        for (int i = 0; i < initialSize; i++)
        {
            MissionIcon icon = Object.Instantiate(prefab);
            icon.gameObject.SetActive(false); // Desactiva el ícono hasta que se necesite
            icon.transform.SetParent(iconPoolParent); // Asigna el padre del pool
            pool.Add(icon);
            Debug.Log($"Ícono inicializado en el pool. Total inicial: {pool.Count}");
        }
    }

    // Método para obtener un ícono disponible del pool
    public MissionIcon GetIcon()
    {
        Debug.Log("Intentando obtener un ícono del pool...");

        // Revisa si hay un ícono inactivo disponible
        foreach (var icon in pool)
        {
            if (!icon.gameObject.activeInHierarchy)
            {
                icon.gameObject.SetActive(true);
                icon.OnObjectSpawn();
                Debug.Log("Ícono activado y obtenido del pool.");
                return icon;
            }
        }

        // Si no hay íconos disponibles, crea uno nuevo y añádelo al pool
        MissionIcon newIcon = Object.Instantiate(prefab);
        newIcon.gameObject.SetActive(true); // Activa el nuevo ícono
        newIcon.transform.SetParent(iconPoolParent); // Asigna el nuevo ícono al padre del pool
        pool.Add(newIcon); // Añadir el nuevo ícono al pool para su reutilización futura
        Debug.Log($"No hay íconos inactivos, creado nuevo MissionIcon. Tamaño actual del pool: {pool.Count}");
        newIcon.OnObjectSpawn();
        return newIcon;
    }

    // Método para devolver un ícono al pool
    public void ReleaseIcon(MissionIcon icon)
    {
        Debug.Log("Devolviendo ícono al pool.");

        icon.gameObject.SetActive(false); // Desactiva el ícono
        icon.transform.SetParent(iconPoolParent); // Devuelve el ícono al padre del pool
        Debug.Log("Ícono devuelto y desactivado en el pool.");
        // Puedes agregar código adicional aquí si necesitas restablecer propiedades
    }
}
