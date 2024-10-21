using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuInicio : MonoBehaviour
{
    // Referencias a los Canvas
    public GameObject menuInicioC;   // El Canvas del men� de inicio 
    public GameObject menuCreditos;     // El Canvas que muestra los cr�ditos

    private void Start()
    {
        // Asegurarse de que el canvas de cr�ditos est� desactivado al iniciar el juego
        menuCreditos.SetActive(false);
        menuInicioC.SetActive(true);
    }
    public void Jugar()
    {
        //pasamos a la escena del juego (id = 1)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Creditos()
    {
        // Desactivar el men� de inicio y activar los cr�ditos
        menuInicioC.SetActive(false);
        menuCreditos.SetActive(true);
    }
    public void VolverAlMenu()
    {
        // Regresar al men� de inicio desactivando los cr�ditos y activando el men�
        menuCreditos.SetActive(false);
        menuInicioC.SetActive(true);
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
