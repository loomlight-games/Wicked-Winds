using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdManager : MonoBehaviour
{
    public static BirdManager Instance { get; private set; }
    private List<GameObject> birds; // Lista de pájaros en la escena
    private bool birdsActive = true;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        birds = new List<GameObject>(GameObject.FindGameObjectsWithTag("Bird")); // Asume que los pájaros tienen el tag "Bird"
    }

    public void DeactivateBirds()
    {
        if (!birdsActive) return; // Evita desactivar si ya están desactivados
        birdsActive = false;

        birds.RemoveAll(b => b == null);
        foreach (var bird in birds)
        {
            bird.SetActive(false);
        }
    }

    public void ActivateBirds()
    {
        if (birdsActive) return; // Evita activar si ya están activos
        birdsActive = true;

        birds.RemoveAll(b => b == null);
        foreach (var bird in birds)
        {
            bird.SetActive(true);
        }
    }

    public bool AreBirdsActive()
    {
        return birdsActive;
    }


    // Método para añadir un pájaro a la lista
    public void RegisterBird(GameObject bird)
    {
        if (!birds.Contains(bird))
        {
            birds.Add(bird);
        }
    }

}
