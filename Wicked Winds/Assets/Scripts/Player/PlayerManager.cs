using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections.Generic;

/// <summary>
/// Manages the player states and information
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerManager : AStateController
{
    public static PlayerManager Instance { get; private set;} // Only one player

    [HideInInspector] public float verticalVelocity;
    [HideInInspector] public CharacterController controller;
    [HideInInspector] public bool runKey, runJoystick, canRun, flyKey;
    [HideInInspector] public Vector2 movement2D;
    [HideInInspector] public int score;

    [HideInInspector] public readonly string PLAYER_CUSTOMIZATION_FILE = "PlayerCustomization";
    [HideInInspector] public readonly string PLAYER_PURCHASED_ITEMS_FILE = "PlayerPurchasedItems";
    [HideInInspector] public readonly string PLAYER_COINS_FILE = "PlayerCoins";

    // List of purchased items
    public List<CustomizableItem> purchasedItems = new();

    [HideInInspector] public int coins;

    #region STATES
    public readonly ControllablePlayerState controllableState = new();// On ground
    //public readonly FlyingPlayerState flyingState = new();// Flying
    public readonly AtShopPlayerState atShopState = new();// At shop
    public readonly FinalPlayerState finalState = new();
    #endregion

    #region HABILITIES
    public Movable movable; // Movable
    public Boostable boostable; // Boostable
    public Flying flying; // Flying
    public CustomizableCharacter customizable; // Customizable
    #endregion

    #region PROPERTIES
    [Header("Movement")]
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float boostSpeed = 13f;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float flyForce = 2f;
    [SerializeField] float gravity = 5f;
    [SerializeField] float heightLimit = 10f;
    [SerializeField] float joystickScale = 2f;

    [Header("Mechanics")]
    [SerializeField] float boostLossPerSecond = 5f;

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

        movable = new (controller, walkSpeed, boostSpeed, rotationSpeed);
        boostable = new (boostLossPerSecond);
        flying = new (controller, flyForce, gravity, heightLimit);
        customizable = new (head, upperBody, lowerBody, shoes);

        customizable.Awake();
    }

    // Start is called before the first frame update
    public override void Start()
    {
        if (SceneManager.GetActiveScene().name == "Gameplay")
            SetState(controllableState);
        else if (SceneManager.GetActiveScene().name == "Shop")
            SetState(atShopState);
    }

    // Update is called once per frame
    public override void UpdateFrame()
    {
        
    }
    /////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Detects joystick inputs
    /// </summary>
    public void OnStick(InputAction.CallbackContext context)
    {
        movement2D = context.ReadValue<Vector2>();
        movement2D *= joystickScale; // Must be bigger than keyboard scale (1.0)
    }

    /// <summary>
    /// Detects walk inputs
    /// </summary>
    public void OnWalk(InputAction.CallbackContext context)
    {
        movement2D = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Detects run inputs
    /// </summary>
    public void OnRun(InputAction.CallbackContext context)
    {
        runKey = context.ReadValueAsButton();
    }

    /// <summary>
    /// Detects fly inputs
    /// </summary>
    public void OnFly(InputAction.CallbackContext context)
    {
        flyKey = context.ReadValueAsButton();
    }

    public void OnUpdateBodyPart(CustomizableItem newItem){
        customizable.UpdateBodyPart(newItem);
    }
}
