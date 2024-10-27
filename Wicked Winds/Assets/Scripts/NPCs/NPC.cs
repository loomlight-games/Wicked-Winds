using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public MissionIcon missionIcon = null; // Referencia al ícono que será asignado a este NPC
    public bool hasMission; // Indica si el NPC tiene una misión
    public RandomNPCMovement movementScript; // Referencia al script de movimiento
    public GameObject bubble; // Referencia al objeto bubble del NPC
    public MissionIconPool missionIconPool; // Agrega esta línea
    [SerializeField] public string npcname;
    private NPCNameManager nameManager;
    private MessageGenerator messageGenerator;
    [SerializeField] public string message;
    public string missionType;
    public string responseMessage;
    private NavMeshAgent agent;

    private void Awake()
    {
        nameManager = NPCNameManager.Instance;
        npcname = nameManager.GetRandomNPCName();

    }

    void Start()
    {

        agent = GetComponent<NavMeshAgent>();

        missionIconPool = MissionIconPoolManager.Instance.GetMissionIconPool();
        if (missionIcon == null) hasMission = false;
        else hasMission = true;
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

    public void StopMovement()
    {
        if (agent != null)
        {
            agent.isStopped = true; // Detiene el movimiento del NPC
        }
    }

    public void ThankPlayer()
    {
        Debug.Log("Gracias por completarme la misión!");
        // Aquí puedes activar un bocadillo de agradecimiento o cualquier otro feedback visual
    }

    // Este método es llamado cuando el objeto es devuelto al pool
    public void OnObjectReturn()
    {
        Debug.Log("Devolviendo MissionIcon al pool.");

        if (missionIcon != null) ////NO ENTRA PORQ NO HAY ASIGNED NPC
        {
            Debug.Log($"Devolviendo MissionIcon de {gameObject.name} al pool.");

            if (missionIcon != null)
            {
                Debug.Log($"Liberando ícono de misión de {gameObject.name}.");
                MissionIconPoolManager.Instance.GetMissionIconPool().ReleaseIcon(missionIcon);
                missionIcon = null;
            }

            this.hasMission = false;
            this.message = string.Empty;
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

    public void OnInteractAfterLetter()
    {
        if (agent.isStopped == false)
        {
            StopMovement();
        }
        string response = "Gracias por entregarme esta carta! ";
        gameObject.GetComponent<Interactable>().textBubble.StartDialogue(response); 
    }

    // Método llamado cuando el jugador interactúa con el NPC
    public void OnInteractAfterCollection()
    {
     // Mostrar el mensaje antes de completar la misión
            if (!string.IsNullOrEmpty(responseMessage))
            {
                gameObject.GetComponent<Interactable>().textBubble.StartDialogue(responseMessage);
            }
            this.message = string.Empty;
            CompleteMission();
      
        
    }



    // Método para completar la misión
    public void CompleteMission()
    {
        if (missionIcon != null)
        {
            missionIcon.CompleteMission(); // Completa la misión
            Debug.Log("Misión completada.");

            // Quita al NPC de la lista de objetivos al completar la misión
            PlayerManager.Instance.RemoveTarget(gameObject);
        }
        else
        {
            Debug.LogWarning("Este NPC no tiene una misión activa para completar.");
        }
    }
}