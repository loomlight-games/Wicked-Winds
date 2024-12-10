using System;
using UnityEngine;
using UnityEngine.AI;

public class NpcController : AAnimationController
{
    public Guid npcID;

    [Header("Mission")]
    public bool isTalking,
        hasMission;
    public string missionType,
        message,
        responseMessage;
    public MissionIcon request = null;
    public NpcController sender;
    public CatController cat;
    public OwlController owl;


    [Header("Movement")]
    public float range = 50; // Radius of sphere
    public float stuckDistance = 1.0f; // Minimum distance to consider NPCs stuck
    public float checkInterval = 0.5f; // How often to check for stuck NPCs
    public LayerMask npcLayer; // Layer to identify other NPCs

    [HideInInspector] public NavMeshAgent agent;
    RandomNPCMovement movementScript;
    GameObject bubble;


    #region STATES
    // Idle: has a mission
    // Walking: doesn't have a mission. Already RandomNpcMovement?
    // Talking: player has interacted
    #endregion

    #region ANIMATIONS
    readonly int Idle = Animator.StringToHash("Idle"),
        Moving = Animator.StringToHash("Moving"),
        Talking = Animator.StringToHash("Talking");
    #endregion

    public override void Start()
    {
        name = NPCNameManager.Instance.GetRandomNPCName();
        npcID = Guid.NewGuid(); // Unique ID
        bubble = transform.Find("Bubble").gameObject;
        agent = GetComponent<NavMeshAgent>();
        movementScript = new(this);

        // 'avoidancePriority' is assumed to be set externally
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance;
    }

    public override void UpdateFrame()
    {
        // If player isn't talking with anyone
        if (PlayerManager.Instance.GetState() != PlayerManager.Instance.talkingState)
        {
            isTalking = false; // This isn't either

            // Doesn't have a mission
            if (request == null)
            {
                hasMission = false;
                bubble.SetActive(false);

                // Moves
                movementScript.Update();
                agent.isStopped = false;
            }
            else // Has a mission
            {
                hasMission = true;
                bubble.SetActive(true);

                // Doesn't move
                agent.isStopped = true;
            }
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
                    isTalking = true;
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
                    isTalking = true;
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
        if (request != null)
        {
            Debug.Log($"Devolviendo MissionIcon de {gameObject.name} al pool.");

            if (request != null)
            {
                MissionIconPoolManager.Instance.GetMissionIconPool().ReleaseIcon(request);
                request = null;
            }

            this.hasMission = false;
            this.message = string.Empty;
        }
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
        // Quita al NPC de la lista de objetivos al completar la misi�n
        PlayerManager.Instance.RemoveTarget(gameObject);

        PlayerManager.Instance.hasActiveMission = false;
    }

    void RotateTowardsPlayer()
    {
        // Calculates rotation to player
        Quaternion lookRotation = Quaternion.LookRotation(PlayerManager.Instance.transform.position - transform.position);

        // Transitions to it
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * agent.speed);
    }

    public override void CheckAnimation()
    {
        if (isTalking)
        {
            ChangeAnimationTo(Talking);
            agent.isStopped = true;
            RotateTowardsPlayer();
        }
        else
        {
            if (hasMission)
                ChangeAnimationTo(Idle);
            else
                ChangeAnimationTo(Moving);
        }
    }
}