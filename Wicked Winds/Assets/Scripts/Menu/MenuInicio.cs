using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicio : MonoBehaviour
{
    // Referencias a los Canvas
    public GameObject menuInicio;   // El Canvas del menú de inicio 
    public GameObject menuCreditos;     // El Canvas que muestra los créditos

    private void Start()
    {
        // Asegurarse de que el canvas de créditos esté desactivado al iniciar el juego
        menuCreditos.SetActive(false);
        menuInicio.SetActive(true);
    }
    public void Jugar()
    {
        //pasamos a la escena del juego (id = 1)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Creditos()
    {
        // Desactivar el menú de inicio y activar los créditos
        menuInicio.SetActive(false);
        menuCreditos.SetActive(true);
    }
    public void VolverAlMenu()
    {
        // Regresar al menú de inicio desactivando los créditos y activando el menú
        menuCreditos.SetActive(false);
        menuInicio.SetActive(true);
    }
    public void Opciones()
    {
        SceneManager.LoadScene("Opciones");
    }
    public void Salir()
    {
        Debug.Log("SALIR");
        Application.Quit();
    }
}
