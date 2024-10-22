using UnityEngine;

public class NPC : MonoBehaviour
{
    public MissionIcon missionIcon = null; // Referencia al �cono que ser� asignado a este NPC
    public bool hasMission; // Indica si el NPC tiene una misi�n
    public RandomNPCMovement movementScript; // Referencia al script de movimiento
    public GameObject bubble; // Referencia al objeto bubble del NPC

    void Start()
    {
        if (missionIcon == null) hasMission = false;
        else hasMission = true;
        if (hasMission && movementScript != null)
        {
            movementScript.enabled = false; // Desactiva el movimiento si tiene misi�n
            if (bubble != null)
            {
                bubble.gameObject.SetActive(true); // Mostrar el bubble si tiene misi�n
            }
        }
        else if (movementScript != null)
        {
            movementScript.enabled = true; // Activa el movimiento si no tiene misi�n

            if (bubble != null)
            {
                bubble.gameObject.SetActive(false); // Ocultar el bubble si no tiene misi�n
            }
        }
    }

    private void Update()
    {
        if (missionIcon == null) hasMission = false;
        else hasMission = true;
    }
}


