using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesactivarPotionUI : MonoBehaviour
{
    // Singleton
    public static DesactivarPotionUI Instance { get; private set; }

    // Variables para controlar la UI de las pociones
    public bool activarFogUI; // Esta opci�n aparecer� en el Inspector
    public GameObject potionFog;     // Objeto a desactivar para la poci�n de niebla

    public bool activarBirdUI; // Esta opci�n aparecer� en el Inspector
    public GameObject potionBird;     // Objeto a desactivar para la poci�n de p�jaros

    private void Awake()
    {
        // Configuraci�n del Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Elimina duplicados si ya existe una instancia
            return;
        }

        Instance = this;
    }

    void Update()
    {
        // Desactiva el objeto de la poci�n de niebla si la condici�n se cumple
        if (activarFogUI)
        {
            potionFog.SetActive(true); // Activa la UI de la poci�n de niebla
        }
        else
        {
            potionFog.SetActive(false); // Desactiva la UI de la poci�n de niebla
        }

        // Desactiva el objeto de la poci�n de p�jaros si la condici�n se cumple
        if (activarBirdUI)
        {
            potionBird.SetActive(true); // Activa la UI de la poci�n de p�jaros
        }
        else
        {
            potionBird.SetActive(false); // Desactiva la UI de la poci�n de p�jaros
        }
    }
}
