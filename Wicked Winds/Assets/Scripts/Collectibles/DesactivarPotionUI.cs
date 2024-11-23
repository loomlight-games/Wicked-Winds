using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesactivarPotionUI : MonoBehaviour
{
    // Singleton
    public static DesactivarPotionUI Instance { get; private set; }

    // Variables para controlar la UI de las pociones
    public bool activarFogUI; // Esta opción aparecerá en el Inspector
    public GameObject potionFog;     // Objeto a desactivar para la poción de niebla

    public bool activarBirdUI; // Esta opción aparecerá en el Inspector
    public GameObject potionBird;     // Objeto a desactivar para la poción de pájaros

    private void Awake()
    {
        // Configuración del Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Elimina duplicados si ya existe una instancia
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Opcional: Mantiene el objeto al cambiar de escena
    }

    void Update()
    {
        // Desactiva el objeto de la poción de niebla si la condición se cumple
        if (activarFogUI)
        {
            potionFog.SetActive(true); // Activa la UI de la poción de niebla
        }
        else
        {
            potionFog.SetActive(false); // Desactiva la UI de la poción de niebla
        }

        // Desactiva el objeto de la poción de pájaros si la condición se cumple
        if (activarBirdUI)
        {
            potionBird.SetActive(true); // Activa la UI de la poción de pájaros
        }
        else
        {
            potionBird.SetActive(false); // Desactiva la UI de la poción de pájaros
        }
    }
}
