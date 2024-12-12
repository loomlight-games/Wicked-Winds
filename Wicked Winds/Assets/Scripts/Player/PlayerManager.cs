using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the player states and data
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerManager : AStateController
{
    public static PlayerManager Instance { get; private set; } // Only one player

    [HideInInspector] public CharacterController controller;
    [HideInInspector] public bool runKey, runJoystick, flyKey, interactKey, nextLineKey, hasActiveMission;
    [HideInInspector] public Vector2 movement2D;
    public NpcController npcMissionActive;
    public NpcController npcObjective;
    public bool playerIsInsideFog;
    public bool potionFog;
    public bool potionBird;
    public Transform cloudTransform;
    [HideInInspector] public int score, MAX_VALUE = 100;
    public List<GameObject> currentTargets = new();
    public Transform target;
    [HideInInspector] public MissionIcon activeMission;

    #region STATES
    public readonly ControllablePlayerState controllableState = new();
    public readonly AtShopPlayerState atShopState = new();
    public readonly FinalPlayerState finalState = new();
    public readonly TalkingPlayerState talkingState = new();
    #endregion

    #region HABILITIES
    public PlayerController playerController = new();
    public CustomizableCharacter customizable;
    public Compass compass = new();
    public PlayerAnimator playerAnimator = new();
    #endregion

    #region PROPERTIES
    [Header("Movement")]
    public Transform orientation;
    public Animator animator;
    public float walkSpeed = 6f;
    public float boostSpeed = 12f;
    public float rainySpeed = 3f;
    public float rotationSpeed = 2f;
    public float flyForce = 2f;
    public float gravityForce = 3f;
    public float lowerHeightLimit = 7f;
    public float maxHeightLimit = 20f;
    public float joystickScale = 1.1f;
    public float pushBackForce = 5f;

    [Header("Mechanics")]
    public float speedPotionLossPerSecond = 2f;
    public float flyPotionLossPerSecond = 4f;
    public float interactionThreshold = 0.9f;
    public Transform compassTransform;


    [Header("Customizable")]
    public Transform model;
    public float rotatorySpeedAtShop = 0.1f;
    #endregion

    /////////////////////////////////////////////////////////////////////////////////////////////
    public override void Awake()
    {
        // Creates one instance if there isn't any (Singleton)
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        controller = GetComponent<CharacterController>();

        customizable = new();

        hasActiveMission = false;

        customizable.Awake();
    }

    // Start is called before the first frame update
    public override void Start()
    {
        if (SceneManager.GetActiveScene().name == "Shop")
            SetState(atShopState);
        else
            SetState(controllableState);
    }

    // Update is called once per frame
    public override void UpdateFrame()
    {

    }
    /////////////////////////////////////////////////////////////////////////////////////////////

    public void OnStick(InputAction.CallbackContext context)
    {
        movement2D = context.ReadValue<Vector2>();
        movement2D *= joystickScale; // Must be bigger than keyboard scale (1.0)
    }

    public void OnWalk(InputAction.CallbackContext context)
    {
        movement2D = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        runKey = context.ReadValueAsButton();
    }

    public void OnFly(InputAction.CallbackContext context)
    {
        flyKey = context.ReadValueAsButton();
    }

    public void OnUpdateBodyPart(Garment newItem)
    {
        customizable.UpdateBodyPart(newItem);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        interactKey = context.ReadValueAsButton();
    }

    public void OnNextLine(InputAction.CallbackContext context)
    {
        nextLineKey = context.ReadValueAsButton();
    }


    public void AddTarget(GameObject target)
    {
        if (!currentTargets.Contains(target))
        {
            currentTargets.Add(target);
            Debug.Log($"Added {target.name} to current targets.");
        }
    }

    public void RemoveTarget(GameObject target)
    {
        if (currentTargets.Contains(target))
        {
            currentTargets.Remove(target);
            Debug.Log($"Removed {target.name} from current targets.");
        }
    }
}
