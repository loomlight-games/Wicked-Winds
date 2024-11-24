using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdManager : MonoBehaviour
{
    public static BirdManager Instance { get; private set; }
    private List<GameObject> birds; // Lista de p�jaros en la escena
    private bool birdsActive = true;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        birds = new List<GameObject>(GameObject.FindGameObjectsWithTag("Bird")); // Asume que los p�jaros tienen el tag "Bird"
    }

    public void DeactivateBirds()
    {
        if (!birdsActive) return; // Evita desactivar si ya est�n desactivados
        birdsActive = false;

        birds.RemoveAll(b => b == null);
        foreach (var bird in birds)
        {
            bird.SetActive(false);
        }
    }

    public void ActivateBirds()
    {
        if (birdsActive) return; // Evita activar si ya est�n activos
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


    // M�todo para a�adir un p�jaro a la lista
    public void RegisterBird(GameObject bird)
    {
        if (!birds.Contains(bird))
        {
            birds.Add(bird);
        }
    }

}
