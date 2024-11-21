using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesactivarPotionUI : MonoBehaviour
{

    // Singleton
    public static DesactivarPotionUI Instance { get; private set; }
    

    public bool activarFogUI; // Esta opci�n aparecer� en el Inspector
    public GameObject potionFog;     // Objeto a desactivar


    private void Awake()
    {
        // Configuraci�n del Singleton
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
        // Desactiva el objeto si la condici�n se cumple
        if (activarFogUI && potionFog != null)
        {
            potionFog.SetActive(true);
        }

        // Desactiva el objeto si la condici�n se cumple
        if (!activarFogUI && potionFog != null)
        {
            potionFog.SetActive(false);
        }
    }
}
