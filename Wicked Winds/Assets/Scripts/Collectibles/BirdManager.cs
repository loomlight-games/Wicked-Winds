using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdManager : MonoBehaviour
{
    public static BirdManager Instance { get; private set; }
    private List<GameObject> birds; // Lista de p�jaros en la escena

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        birds = new List<GameObject>(GameObject.FindGameObjectsWithTag("Bird")); // Asume que los p�jaros tienen el tag "Bird"
    }

    // Desactivar todos los p�jaros
    public void DeactivateBirds()
    {
        foreach (var bird in birds)
        {
            bird.SetActive(false); // Desactiva cada p�jaro
        }
    }

    // Reactivar todos los p�jaros
    public void ActivateBirds()
    {
        foreach (var bird in birds)
        {
            bird.SetActive(true); // Reactiva cada p�jaro
        }
    }
}
