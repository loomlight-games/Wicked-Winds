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

        // Inicializa el pool con el tama?o dado
        for (int i = 0; i < initialSize; i++)
        {
            MissionIcon icon = Object.Instantiate(prefab);
            icon.gameObject.SetActive(false); // Desactiva el ?cono hasta que se necesite
            icon.transform.SetParent(iconPoolParent); // Asigna el padre del pool
            pool.Add(icon);

        }
    }

    // M?todo para obtener un ?cono disponible del pool
    public MissionIcon GetIcon()
    {

        // Revisa si hay un ?cono inactivo disponible
        foreach (var icon in pool)
        {
            if (!icon.gameObject.activeInHierarchy)
            {
                icon.gameObject.SetActive(true);
                icon.OnObjectSpawn();
                return icon;
            }
        }

        // Si no hay ?conos disponibles, crea uno nuevo y a??delo al pool
        MissionIcon newIcon = Object.Instantiate(prefab);
        newIcon.gameObject.SetActive(true); // Activa el nuevo ?cono
        newIcon.transform.SetParent(iconPoolParent); // Asigna el nuevo ?cono al padre del pool
        pool.Add(newIcon); // A?adir el nuevo ?cono al pool para su reutilizaci?n futura

        newIcon.OnObjectSpawn();
        return newIcon;
    }

    // M?todo para devolver un ?cono al pool
    public void ReleaseIcon(MissionIcon icon)
    {

        icon.gameObject.SetActive(false); // Desactiva el ?cono
        icon.transform.SetParent(iconPoolParent); // Devuelve el ?cono al padre del pool

        // Puedes agregar c?digo adicional aqu? si necesitas restablecer propiedades
    }
}
