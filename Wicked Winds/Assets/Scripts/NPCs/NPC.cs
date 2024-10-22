using UnityEngine;

public class NPC : MonoBehaviour
{
    public MissionIcon missionIcon = null; // Referencia al ícono que será asignado a este NPC
    public bool hasMission; // Indica si el NPC tiene una misión
    public RandomNPCMovement movementScript; // Referencia al script de movimiento
    public GameObject bubble; // Referencia al objeto bubble del NPC

    void Start()
    {
        if (missionIcon == null) hasMission = false;
        else hasMission = true;
        if (hasMission && movementScript != null)
        {
            movementScript.enabled = false; // Desactiva el movimiento si tiene misión
            if (bubble != null)
            {
                bubble.gameObject.SetActive(true); // Mostrar el bubble si tiene misión
            }
        }
        else if (movementScript != null)
        {
            movementScript.enabled = true; // Activa el movimiento si no tiene misión

            if (bubble != null)
            {
                bubble.gameObject.SetActive(false); // Ocultar el bubble si no tiene misión
            }
        }
    }

    private void Update()
    {
        if (missionIcon == null) hasMission = false;
        else hasMission = true;
    }
}


