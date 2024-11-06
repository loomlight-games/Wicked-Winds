using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;

/// <summary>
/// Manages the player states and information
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerManager : AStateController
{
    public event EventHandler MissionCompleteEvent;

    public static PlayerManager Instance { get; private set;} // Only one player

    [HideInInspector] public CharacterController controller;
    [HideInInspector] public bool runKey, runJoystick, canRun, flyKey, interactKey, nextLineKey, hasActiveMission;
    [HideInInspector] public Vector2 movement2D;
    [HideInInspector] public int score;
    [HideInInspector] public List<GameObject> currentTargets = new ();
    [HideInInspector] public Transform target;
    [HideInInspector] public MissionIcon activeMission;

    #region STATES
    public readonly ControllablePlayerState controllableState = new();// On ground
    public readonly AtShopPlayerState atShopState = new();// At shop
    public readonly FinalPlayerState finalState = new();
    #endregion

    #region HABILITIES
    public PlayerController playerController;
    public Boostable boostable;
    public CustomizableCharacter customizable;
    public Interactions interactions;
    public Compass compass;
    #endregion

    #region PROPERTIES
    [Header("Movement")]
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float boostSpeed = 13f;
    [SerializeField] public float rotationSpeed = 5f;
    [SerializeField] float flyForce = 2f;
    [SerializeField] float gravity = 5f;
    [SerializeField] float heightLimit = 10f;
    [SerializeField] float joystickScale = 2f;

    [Header("Mechanics")]
    [SerializeField] float boostLossPerSecond = 5f;
    [SerializeField] public float interactRange = 2f;
    [SerializeField] public Transform compassTransform;


    [Header("Customizable")]
    [SerializeField] Transform head;
    [SerializeField] Transform upperBody; 
    [SerializeField] Transform lowerBody; 
    [SerializeField] Transform shoes; 
    [SerializeField] public float rotatorySpeedAtShop = 0.1f;
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

        playerController = new(controller, walkSpeed, boostSpeed, flyForce, gravity, heightLimit, rotationSpeed);
        boostable = new (boostLossPerSecond);
        customizable = new (head, upperBody, lowerBody, shoes);
        interactions = new ();
        compass = new();

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

    public void OnUpdateBodyPart(Garment newItem){
        customizable.UpdateBodyPart(newItem);
    }

    public void OnInteract(InputAction.CallbackContext context){
        interactKey = context.ReadValueAsButton();
    }

    public void OnNextLine(InputAction.CallbackContext context){
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

    public void OnMissionCompleted(){
        MissionCompleteEvent?.Invoke(this,null);
    }
}
