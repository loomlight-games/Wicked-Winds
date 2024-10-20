using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Detects inputs (joystick and keyboard) and moves object
/// Only runs when enough boost
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class Movable : MonoBehaviour
{
// Properties ////////////////////////////////////////////////////
    // Input
    InputActions playerInput; // Class generated from input map
    bool runKey,runJoystick, jumpKey;
    CharacterController controller;
    // Movement
    public event EventHandler<EventArgs> RunningEvent; // Invoked when run trigger
    public Transform cameraTransform; // Isometric camera
    Vector2 movement2D; // Joystick
    Vector3 movement3D, lookAtPosition; // Player
    Quaternion lookAtRotation;
    float verticalVelocity, movementSpeed;
    float joystickScale = 2f;
    bool canRun;
    // Inspector
    public float walkSpeed = 10f;
    public float runSpeed = 15f;
    public float rotationSpeed = 5f;
    public float jumpForce = 2f;
    public float gravity = 9.8f;


    ///////////////////////////////////////////////////////////////////////
    void Awake()
    {
        controller = GetComponent<CharacterController>();

        playerInput = new();

        // Joystick: OnStick is susbcribed to walk inputs
        playerInput.Playermovement.Joystick.started += OnStick;
        playerInput.Playermovement.Joystick.performed += OnStick;
        playerInput.Playermovement.Joystick.canceled += OnStick;

        // Walk: OnMove is susbcribed to walk inputs
        playerInput.Playermovement.Walk.started += OnWalk;
        playerInput.Playermovement.Walk.canceled += OnWalk;

        // Run
        playerInput.Playermovement.Run.started += OnRun;
        playerInput.Playermovement.Run.canceled += OnRun;

        // Jump
        playerInput.Playermovement.Jump.started += OnJump;
        playerInput.Playermovement.Jump.canceled += OnJump;

        // Needs to know boost value
        Boostable boostable = GetComponent<Boostable>();
        boostable.BoostValueEvent += OnBoostChangeEvent;
    }

    // Update is called once per frame
    void Update()
    {
        Fly();
        Move();
        Rotate();
    }

    ///////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Gravity calculation and flying behavior
    /// </summary>
    void Fly()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = -1f; // Slight downward force to keep grounded
            
            if (jumpKey) // If jump is pressed while grounded
                verticalVelocity = jumpForce; // Apply jump force
        }
        else
        {
            if (jumpKey) // If jump is pressed while in the air, ascend
                verticalVelocity += jumpForce * Time.deltaTime; // Gradually increase altitude
            else // Apply gravity when jump is not pressed
                verticalVelocity -= gravity * Time.deltaTime; // Descend naturally
        }
    }

    /// <summary>
    /// Moves player in isometric perspective.
    /// </summary>
    void Move(){
        // Limit joystick movement and check if it's running
        CheckJoystick();

        // Get direction in 3D space based on camera orientation
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Ensure the camera's movement stays on the horizontal plane
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize(); // Magnitude of 1
        right.Normalize(); // Magnitude of 1

        // 3DMovement based on 2D input and camera's orientation, without veretical volicity applied
        movement3D = right * movement2D.x + forward * movement2D.y;

        // Check if the player is running (on the ground or in the air)
        if (runKey || runJoystick)
        {
            if (canRun)  // If able to run (boost available)
            {
                if (movement2D.sqrMagnitude != 0) // Is moving
                    RunningEvent?.Invoke(this, null); // Trigger running event
                
                movementSpeed = runSpeed;
            }
            else  // If not able to run, walk
                movementSpeed = walkSpeed;
        }
        else
            movementSpeed = walkSpeed;

        // Move character applying vertical velocity
        controller.Move((movement3D * movementSpeed + verticalVelocity * Vector3.up) * Time.deltaTime);
    }

    /// <summary>
    /// Rotates player to face movement and maintains it after stopping
    /// </summary>
    void Rotate(){
        // Only rotate if there is movement input
        // To ensure that rotation moving is maintained when stopping
        if (movement3D.x != 0 || movement3D.z != 0)
        {
            lookAtPosition = new Vector3(movement3D.x, 0, movement3D.z); // Movement direction
            lookAtRotation = Quaternion.LookRotation(lookAtPosition); // Rotation to movement direction
            // From current rotation to movement rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, lookAtRotation, rotationSpeed * Time.deltaTime);
        }
    }

        /// <summary>
    /// Check if joystick has passed keyboard limits to run
    /// </summary>
    private void CheckJoystick()
    {
        // Keyboard inputs are normalized (-1>0<1)
        if (movement2D.x > 1 || movement2D.y > 1 || 
            movement2D.x < -1 || movement2D.y < -1){
            // Limit movement passed keyboard values
            // Ensures the movemeent difference between both inputs is minimal
            if (movement2D.x > 1)
                movement2D.x = 1.1f;
            else if (movement2D.x < -1)
                movement2D.x = -1.1f;
            if (movement2D.y > 1)
                movement2D.y = 1.1f;
            else if (movement2D.y < -1)
                movement2D.y = -1.1f;

            runJoystick = true;
        }
        else{
            runJoystick = false;
        }
    }

    /// <summary>
    /// Detects joystick inputs
    /// </summary>
    public void OnStick(InputAction.CallbackContext context)
    {
        movement2D = context.ReadValue<Vector2>();
        movement2D *= joystickScale; // Must be bigger than keyboard scake (1.0)
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
    /// Detects run inputs
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        jumpKey = context.ReadValueAsButton();
    }

    /// <summary>
    /// Called when boost value is modified (only when running)
    /// </summary>
    private void OnBoostChangeEvent(object sender, float currentBoost)
    {
        // All consumed
        if (currentBoost <= 0)
            canRun = false; // Can't run
        else
            canRun = true;
    }
}