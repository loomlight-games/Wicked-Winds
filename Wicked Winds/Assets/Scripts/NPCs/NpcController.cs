using System;
using UnityEngine;
using UnityEngine.AI;

public class NpcController : MonoBehaviour
{
    public string missionType,
        message,
        responseMessage;
    public bool hasMission;
    public MissionIcon request = null;
    public NpcController sender;
    public CatController cat;
    public OwlController owl;
    public Animator animator;
    public Guid npcID;
    public bool isMissionStateDirty; // Flag para detectar cambios en el estado


    NavMeshAgent agent;
    RandomNPCMovement movementScript;
    public GameObject bubble;


    void Awake()
    {
        name = NPCNameManager.Instance.GetRandomNPCName();
        npcID = Guid.NewGuid(); // Unique ID
    }

    void Start()
    {
        bubble = transform.Find("Bubble").gameObject;

        movementScript = GetComponent<RandomNPCMovement>();
        agent = GetComponent<NavMeshAgent>();

        if (request == null) hasMission = false;
        else hasMission = true;
        isMissionStateDirty = true; // Fuerza una actualización inicial
    }

    void Update()
    {
        hasMission = request != null;

        if (movementScript != null)
        {
            movementScript.enabled = !hasMission;
        }

        // Actualizar burbuja solo si hay cambios en el estado
        if (isMissionStateDirty)
        {
            UpdateBubbleVisibilityBasedOnMission();
            isMissionStateDirty = false; // Resetear el flag
        }
    }


    private void UpdateBubbleState(string missionName)
    {
        if (bubble == null) return;

        for (int i = 0; i < bubble.transform.childCount; i++)
        {
            GameObject child = bubble.transform.GetChild(i).gameObject;

            // Mostrar solo el elemento correspondiente al tipo de misión
            child.SetActive(child.name == missionName);
        }
    }

    private void UpdateBubbleVisibilityBasedOnMission()
    {
        if (!hasMission)
        {
            HideAllBubbleChildren(); // Oculta todo
        }
        else if (PlayerManager.Instance.npcMissionActive == this)
        {
            UpdateBubbleState(request?.data?.missionName); // Muestra el icono relacionado con la misión
        }
        else
        {
            UpdateBubbleState("ExclamationIcon"); // Muestra el icono de exclamación
        }
    }


    private void HideAllBubbleChildren()
    {
        for (int i = 0; i < bubble.transform.childCount; i++)
        {
            bubble.transform.GetChild(i).gameObject.SetActive(false); // Ocultar todos los hijos
        }
    }

    public void Interact()
    {

        // Player has a mission
        if (PlayerManager.Instance.hasActiveMission)
        {
            // This NPC is target
            if (PlayerManager.Instance.currentTargets.Contains(gameObject))
            {
                // Start conversation
                if (responseMessage != null)
                {
                    GameManager.Instance.dialogue.StartDialogue(name, responseMessage);
                }
                else
                    Debug.LogError("npc.responseMessage is null.");

                if (desactivarOwlUI.Instance.activateOwlUI == true)
                {
                    desactivarOwlUI.Instance.activateOwlUI = false;
                }

                

                PlayerManager.Instance.npcMissionActive.OnMissionCompleted();
                PlayerManager.Instance.currentTargets.Remove(gameObject);
                PlayerManager.Instance.npcMissionActive = null;
                PlayerManager.Instance.npcObjective = null;

            }
        }
        // Player has no mission assigned
        else
        {
            // This NPC has mission to give
            if (hasMission)
            {
                isMissionStateDirty = true;
                //UPDATE MISSION ICON
                
                // Player knows this NPC as the giver of current mision
                PlayerManager.Instance.npcMissionActive = this;

                // Start conversation
                if (message != null)
                {
                    GameManager.Instance.dialogue.StartDialogue(name, message);
                }
                else
                    Debug.LogError("npc.message is null.");

                // Assigns the mission of this NPC to the player
                PlayerManager.Instance.activeMission = request;



                if (request.data.name == "LetterMision")
                {
                    Guid targetID = PlayerManager.Instance.npcMissionActive.request.addressee.npcID;

                    // Encuentra todos los NPCs en la escena
                    NpcController[] allNPCs = FindObjectsOfType<NpcController>();

                    foreach (NpcController npc in allNPCs)
                    {
                        // Compara el ID del NPC con el objetivo
                        if (npc.npcID == targetID)
                        {
                            Debug.Log("añadiendo como target al destinatario");
                            PlayerManager.Instance.AddTarget(npc.gameObject);
                        }
                    }
                }

                if (PlayerManager.Instance.activeMission.data.name == "CatMission")
                {
                    GameObject objetivo = PlayerManager.Instance.npcMissionActive.cat.gameObject;
                    PlayerManager.Instance.AddTarget(objetivo.gameObject);
                }

                if (PlayerManager.Instance.activeMission.data.name == "OwlMission")
                {
                    GameObject objetivo = PlayerManager.Instance.npcMissionActive.owl.gameObject;
                    PlayerManager.Instance.AddTarget(objetivo.gameObject);
                }

                PlayerManager.Instance.hasActiveMission = true; // Marca que el jugador tiene una mision activa
            }
        }
    }

    // Este metodo es llamado cuando el objeto es devuelto al pool
    public void OnObjectReturn()
    {
        Debug.Log("Devolviendo MissionIcon al pool.");

        if (request != null) ////NO ENTRA PORQ NO HAY ASIGNED NPC
        {
            Debug.Log($"Devolviendo MissionIcon de {gameObject.name} al pool.");

            if (request != null)
            {
                Debug.Log($"Liberando icono de mision de {gameObject.name}.");
                MissionIconPoolManager.Instance.GetMissionIconPool().ReleaseIcon(request);
                request = null;
            }

            this.hasMission = false;
            isMissionStateDirty = true; // Asegura que el estado de la burbuja se actualice
            this.message = string.Empty;
            Debug.Log($"Estado del NPC {gameObject.name} actualizado: hasMission = false.");
        }

        Debug.Log("Referencias limpiadas en OnObjectReturn.");
    }

    // Metodo llamado cuando el jugador interactua con el NPC
    public void OnMissionCompleted()
    {
        // Mostrar el mensaje antes de completar la misión
        if (agent.isStopped == false)
        {
            OnInteractAfterLetter();
            return;
        }

        this.message = string.Empty;

        CompleteMission(this);
    }

    public void OnInteractAfterLetter()
    {
        StopMovement();
        CompleteMission(sender);
    }
    public void StopMovement()
    {
        if (agent != null)
        {
            agent.isStopped = true; // Detiene el movimiento del NPC
        }
    }

    /// <summary>
    /// Removes target and mission from player
    /// </summary>
    public void CompleteMission(NpcController npc)
    {
        if (this.request != null)
            request.CompleteMission();

        if (sender != null)
        {
            if (sender.request != null)
            {
                sender.request.CompleteMission();
                sender = null;
            }
        }
        isMissionStateDirty = true;
        // Quita al NPC de la lista de objetivos al completar la misi�n
        PlayerManager.Instance.RemoveTarget(gameObject);

        PlayerManager.Instance.hasActiveMission = false;
    }

}