using System;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public string missionType,
        message,
        responseMessage;
    public bool hasMission;
    public MissionIcon request = null;
    public NPC sender;
    public CatController cat;
    public OwlController owl;
    public Animator animator;
    public Guid npcID;

    NavMeshAgent agent;
    RandomNPCMovement movementScript;
    GameObject bubble;


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

    void Update()
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

        // if (missionIcon == null)
        // {
        //     hasMission = false;
        //     movementScript.enabled = false;
        //     bubble.SetActive(false);
        // }
        // else
        // {
        //     hasMission = true;
        //     movementScript.enabled = true;
        //     bubble.SetActive(true);
        // }
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

                //PlayerManager.Instance.npcMissionActive.cat?.SwitchState(PlayerManager.Instance.npcMissionActive.cat.followingOwnerState);
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

}