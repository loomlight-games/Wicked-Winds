using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    
    public MissionIcon missionIcon = null; // Referencia al �cono que ser� asignado a este NPC
    public bool hasMission; // Indica si el NPC tiene una misi�n
    public RandomNPCMovement movementScript; // Referencia al script de movimiento
    public GameObject bubble; // Referencia al objeto bubble del NPC
    public MissionIconPool missionIconPool; // Agrega esta l�nea
    [SerializeField] public string npcname;
    private NPCNameManager nameManager;
    private MessageGenerator messageGenerator;
    [SerializeField] public string message;
    public string missionType;
    public string responseMessage;
    private NavMeshAgent agent;
    public NPC sender; 

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

    public void StopMovement()
    {
        if (agent != null)
        {
            agent.isStopped = true; // Detiene el movimiento del NPC
        }
    }

    public void ThankPlayer()
    {
        GameManager.Instance.playState.feedBackText.text = "Thanks for bringing my letter!";
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

    public void OnInteractAfterLetter()
    {   
        StopMovement();        
        ThankPlayer();
        string response = "Gracias por entregarme esta carta! ";
        gameObject.GetComponent<Interactable>().dialoguePanel.StartDialogue(response); 
        CompleteMission(sender);
    }

    // M�todo llamado cuando el jugador interact�a con el NPC
    public void OnMissionCompleted()
    {
        // Mostrar el mensaje antes de completar la misión
        if (agent.isStopped == false)
        {
            OnInteractAfterLetter();
            return;
        }

        if (!string.IsNullOrEmpty(responseMessage))
        {
            gameObject.GetComponent<Interactable>().dialoguePanel.StartDialogue(responseMessage);
        }
        
        this.message = string.Empty;

        CompleteMission(this);
    }

    /// <summary>
    /// Removes target and mission from player
    /// </summary>
    public void CompleteMission(NPC npc)
    {
        if (this.missionIcon != null)
            missionIcon.CompleteMission();
        
        if (sender!=null){
            if(sender.missionIcon != null)
            {
                sender.missionIcon.CompleteMission();
                sender = null;
            }
        }
        // Quita al NPC de la lista de objetivos al completar la misi�n
        PlayerManager.Instance.RemoveTarget(gameObject);

        PlayerManager.Instance.hasActiveMission = false;

        PlayerManager.Instance.OnMissionCompleted();
    }
}



