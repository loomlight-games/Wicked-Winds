using Unity.VisualScripting;
using UnityEngine;

public class NPC : Interactable
{

    //La clase NPC debe tener una referencia a un Icon, y debe poder asignar y completar misiones utilizando este Icon.
    //También debe gestionar la visualización de burbujas y el movimiento en función de si tiene o no una misión.

    public Icon missionIcon = null; // Referencia al ícono que será asignado a este NPC
    public bool hasMission; // Indica si el NPC tiene una misión
    public RandomNPCMovement movementScript; // Referencia al script de movimiento
    public GameObject bubble; // Referencia al objeto bubble del NPC

    void Start()
    {
        if (missionIcon == null) hasMission = false;
        else hasMission = true;
        UpdateNPCState();
    }

    private void UpdateNPCState()
    {
        if (hasMission && movementScript != null)
        {
            movementScript.enabled = false; // Desactiva el movimiento si tiene misión
            if (bubble != null)
            {
                bubble.SetActive(true); // Mostrar el bubble si tiene misión
            }
        }
        else if (movementScript != null)
        {
            movementScript.enabled = true; // Activa el movimiento si no tiene misión
            if (bubble != null)
            {
                bubble.SetActive(false); // Ocultar el bubble si no tiene misión
            }
        }
    }

    private void Update()
    {
        base.Update(); // Asegúrate de llamar al método Update de la clase base si es necesario
    }

    // Puedes agregar aquí métodos específicos de NPC si es necesario
}


