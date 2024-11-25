using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class desactivarOwlUI : MonoBehaviour
{
    // Singleton
    public static desactivarOwlUI Instance { get; private set; }


    public bool activateOwlUI; // Esta opci�n aparecer� en el Inspector
    public GameObject owlUI;     // Objeto a desactivar


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
        // Desactiva el objeto si la condici�n se cumple
        if (activateOwlUI == true)
        {
            owlUI.SetActive(true);
        }

        // Desactiva el objeto si la condici�n se cumple
        if (activateOwlUI == false)
        {
            owlUI.SetActive(false);
        }
    }
}
