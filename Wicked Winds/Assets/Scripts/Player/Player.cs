using UnityEngine;

public class Player : MonoBehaviour
{
    public bool hasActiveMission; // Indica si el jugador tiene una misi�n activa

    private void Start()
    {
        hasActiveMission = false; // Inicializa la variable en falso
    }

    // Puedes agregar m�todos para aceptar o completar misiones aqu�
}
