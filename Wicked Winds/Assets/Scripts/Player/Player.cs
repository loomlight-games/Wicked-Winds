using UnityEngine;

public class Player : MonoBehaviour
{
    public bool hasActiveMission; // Indica si el jugador tiene una misión activa

    private void Start()
    {
        hasActiveMission = false; // Inicializa la variable en falso
    }

    // Puedes agregar métodos para aceptar o completar misiones aquí
}
