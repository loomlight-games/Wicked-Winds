using System.Collections.Generic;
using UnityEngine;

public class MissionIconPool
{
    private MissionIcon prefab;
    private List<MissionIcon> pool;
    private Transform iconPoolParent; // Almacena la referencia al GameObject del pool
    private GameObject assignedIcons;

    // Constructor que acepta el prefab y el GameObject del pool
    public MissionIconPool(MissionIcon prefab, int initialSize, Transform parent)
    {
        this.prefab = prefab;
        this.iconPoolParent = parent; // Asigna el padre
        pool = new List<MissionIcon>();
        assignedIcons = GameObject.Find("IngredientsParent") ?? new GameObject("IngredientsParent");
        // Inicializa el pool con el tama?o dado
        for (int i = 0; i < initialSize; i++)
        {
            MissionIcon icon = Object.Instantiate(prefab);
            icon.gameObject.SetActive(false); // Desactiva el icono hasta que se necesite
            icon.transform.SetParent(iconPoolParent); // Asigna el padre del pool
            pool.Add(icon);

        }
    }

    // Metodo para obtener un icono disponible del pool
    public MissionIcon GetIcon()
    {

        // Revisa si hay un icono inactivo disponible
        foreach (var icon in pool)
        {
            if (!icon.gameObject.activeInHierarchy)
            {
                icon.gameObject.SetActive(true);
                return icon;
            }
        }

        // Si no hay iconos disponibles, crea uno nuevo y a�delo al pool
        MissionIcon newIcon = Object.Instantiate(prefab);
        newIcon.gameObject.SetActive(true); // Activa el nuevo icono
        newIcon.transform.SetParent(assignedIcons.transform); // Asigna el nuevo ?cono al padre del pool
        pool.Add(newIcon); // A?adir el nuevo ?cono al pool para su reutilizacion futura

        return newIcon;
    }

    // M?todo para devolver un ?cono al pool
    public void ReleaseIcon(MissionIcon icon)
    {

        icon.gameObject.SetActive(true); 
        icon.transform.SetParent(iconPoolParent); // Devuelve el Icono al padre del pool


    }
}
