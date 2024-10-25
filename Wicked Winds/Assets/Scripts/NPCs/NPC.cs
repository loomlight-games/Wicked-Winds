using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public MissionIcon missionIcon = null; // Referencia al �cono que ser� asignado a este NPC
    public bool hasMission; // Indica si el NPC tiene una misi�n
    public RandomNPCMovement movementScript; // Referencia al script de movimiento
    public GameObject bubble; // Referencia al objeto bubble del NPC
    public MissionIconPool missionIconPool; // Agrega esta l�nea
    [SerializeField] private string npcname;
    private NPCNameManager nameManager;
    private MessageGenerator messageGenerator;
    [SerializeField] private string message;
    [SerializeField] public bool acceptMission = false;


    void Start()
    {
        nameManager = NPCNameManager.Instance;
        npcname = nameManager.GetRandomNPCName();
        
        missionIconPool= MissionIconPoolManager.Instance.GetMissionIconPool();
        if (missionIcon == null) hasMission = false;
        else hasMission = true;
        if (hasMission && movementScript != null)
        {
            movementScript.enabled = false; // Desactiva el movimiento si tiene misi�n
            if (bubble != null)
            {
                bubble.SetActive(true); // Mostrar el bubble si tiene misi�n
            }
        }
        else if (movementScript != null)
        {
            movementScript.enabled = true; // Activa el movimiento si no tiene misi�n

            if (bubble != null)
            {
                bubble.SetActive(false); // Ocultar el bubble si no tiene misi�n
            }
        }


    }

    // Este m�todo es llamado cuando el objeto es devuelto al pool
    public void OnObjectReturn()
    {
        Debug.Log("Devolviendo MissionIcon al pool.");

        if (missionIcon != null) ////NO ENTRA PORQ NO HAY ASIGNED NPC
        {
            Debug.Log($"Devolviendo MissionIcon de {gameObject.name} al pool.");

            if (missionIcon != null)
            {
                Debug.Log($"Liberando �cono de misi�n de {gameObject.name}.");
                MissionIconPoolManager.Instance.GetMissionIconPool().ReleaseIcon(missionIcon);
                missionIcon = null;
            }

            this.hasMission = false;
            Debug.Log($"Estado del NPC {gameObject.name} actualizado: hasMission = false.");
        }

       
        Debug.Log("Referencias limpiadas en OnObjectReturn.");
    }

    private void Update()
    {
       if (missionIcon == null) hasMission = false;
        else hasMission = true;

        if (hasMission && movementScript != null)
        {
            movementScript.enabled = false; // Desactiva el movimiento si tiene misi�n
            if (bubble != null)
            {
                bubble.SetActive(true); // Mostrar el bubble si tiene misi�n
            }
        }
        else if (movementScript != null)
        {
            movementScript.enabled = true; // Activa el movimiento si no tiene misi�n

            if (bubble != null)
            {
                bubble.SetActive(false); // Ocultar el bubble si no tiene misi�n
            }
        }
    }
}


