using System;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public MissionIcon request = null; // Referencia al �cono que ser� asignado a este NPC
    public bool hasMission; // Indica si el NPC tiene una misi�n
    public RandomNPCMovement movementScript; // Referencia al script de movimiento
    public GameObject bubble; // Referencia al objeto bubble del NPC
    public MissionIconPool missionIconPool; // Agrega esta l�nea
    [SerializeField] public string npcName;
    private NPCNameManager nameManager;
    private MessageGenerator messageGenerator;
    [SerializeField] public string message;
    public string missionType;
    [SerializeField] public string responseMessage;
    private NavMeshAgent agent;
    public NPC sender;
    public CatController cat;
    public OwlController owl;
    public Guid npcID; // Identificador único para el NPC
    public Animator animator;

    private void Awake()
    {
        nameManager = NPCNameManager.Instance;
        npcName = nameManager.GetRandomNPCName();
        npcID = Guid.NewGuid(); // Generar un ID único al inicializar el NPC

    }

    void Start()
    {
        name = npcName;

        agent = GetComponent<NavMeshAgent>();

        missionIconPool = MissionIconPoolManager.Instance.GetMissionIconPool();
        if (request == null) hasMission = false;
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

    private void Update()
    {
        if (request == null) hasMission = false;
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

    // Este m�todo es llamado cuando el objeto es devuelto al pool
    public void OnObjectReturn()
    {
        Debug.Log("Devolviendo MissionIcon al pool.");

        if (request != null) ////NO ENTRA PORQ NO HAY ASIGNED NPC
        {
            Debug.Log($"Devolviendo MissionIcon de {gameObject.name} al pool.");

            if (request != null)
            {
                Debug.Log($"Liberando �cono de misi�n de {gameObject.name}.");
                MissionIconPoolManager.Instance.GetMissionIconPool().ReleaseIcon(request);
                request = null;
            }

            this.hasMission = false;
            this.message = string.Empty;
            Debug.Log($"Estado del NPC {gameObject.name} actualizado: hasMission = false.");
        }


        Debug.Log("Referencias limpiadas en OnObjectReturn.");
    }

    public void OnInteractAfterLetter()
    {
        StopMovement();
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

        this.message = string.Empty;

        CompleteMission(this);
    }

    /// <summary>
    /// Removes target and mission from player
    /// </summary>
    public void CompleteMission(NPC npc)
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
        // Quita al NPC de la lista de objetivos al completar la misi�n
        PlayerManager.Instance.RemoveTarget(gameObject);

        PlayerManager.Instance.hasActiveMission = false;
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
                    GameManager.Instance.dialogue.StartDialogue(npcName, responseMessage);
                }
                else
                    Debug.LogError("npc.responseMessage is null.");

                if (desactivarOwlUI.Instance.activateOwlUI == true)
                {
                    desactivarOwlUI.Instance.activateOwlUI = false;
                }

                PlayerManager.Instance.npcMissionActive.cat?.SwitchState(PlayerManager.Instance.npcMissionActive.cat.followingOwnerState);
                PlayerManager.Instance.npcMissionActive.OnMissionCompleted();
                PlayerManager.Instance.currentTargets.Remove(gameObject);
                PlayerManager.Instance.npcMissionActive = null;
                PlayerManager.Instance.npcObjective = null;

            }
            // This NPC is not the target
            else
                GameManager.Instance.playState.feedBackText.text = "Finish the current task before speaking to another villager.";
        }
        // Player has no mission assigned
        else
        {
            // This NPC has mission to give
            if (hasMission)
            {
                // Player knows this NPC as the giver of current mision
                PlayerManager.Instance.npcMissionActive = this;

                // Changes mission icon
                if (request != null && request.data != null)
                {
                    SpriteRenderer spriteRenderer = request.GetComponent<SpriteRenderer>();

                    if (spriteRenderer != null)
                        spriteRenderer.sprite = request.data.missionIconSprite;
                }

                // Start conversation
                if (message != null)
                {
                    GameManager.Instance.dialogue.StartDialogue(npcName, message);
                }
                else
                    Debug.LogError("npc.message is null.");

                // Assigns the mission of this NPC to the player
                PlayerManager.Instance.activeMission = request;

                if (request.data.type == "LetterMision")
                {
                    Guid targetID = PlayerManager.Instance.npcMissionActive.request.addressee.npcID;

                    // Encuentra todos los NPCs en la escena
                    NPC[] allNPCs = FindObjectsOfType<NPC>();

                    foreach (NPC npc in allNPCs)
                    {
                        // Compara el ID del NPC con el objetivo
                        if (npc.npcID == targetID)
                        {
                            Debug.Log("a�adiendo como target al destinatario");
                            PlayerManager.Instance.AddTarget(npc.gameObject);
                        }
                    }
                }

                if (PlayerManager.Instance.activeMission.data.type == "CatMission")
                {
                    GameObject objetivo = PlayerManager.Instance.npcMissionActive.cat.gameObject;
                    PlayerManager.Instance.AddTarget(objetivo.gameObject);
                }

                if (PlayerManager.Instance.activeMission.data.type == "OwlMission")
                {
                    GameObject objetivo = PlayerManager.Instance.npcMissionActive.owl.gameObject;
                    PlayerManager.Instance.AddTarget(objetivo.gameObject);
                }

                PlayerManager.Instance.hasActiveMission = true; // Marca que el jugador tiene una mision activa
            }
        }
    }
}



