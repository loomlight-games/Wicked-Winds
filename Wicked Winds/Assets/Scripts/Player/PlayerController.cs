using UnityEngine;
using System;

/// <summary>
/// Detects inputs (joystick and keyboard) and moves object
/// Only runs when enough boost
/// </summary>
public class PlayerController
{
    public event EventHandler<EventArgs> RunningEvent; // Invoked when run trigger
    
    readonly CharacterController controller;
    readonly float walkSpeed,
        boostSpeed,
        heightLimit,
        flyForce, 
        gravityValue,
        rotationSpeed;

    float movementSpeed, verticalVelocity, verticalPosition;
    Vector3 movement3D, forward, right, lookAtPosition;
    Vector2 movement2D;
    Quaternion lookAtRotation;
    Transform cameraTransform,
            playerBodyTransform;
    
    ///////////////////////////////////////////////////////////////////////
    public PlayerController(CharacterController controller, 
                            float walkSpeed, 
                            float boostSpeed, 
                            float flyForce,
                            float gravityValue,
                            float heightLimit,
                            float rotationSpeed){
        this.controller = controller;
        this.walkSpeed = walkSpeed;
        this.boostSpeed = boostSpeed;
        this.flyForce = flyForce;
        this.gravityValue = gravityValue;
        this.heightLimit = heightLimit;
        this.rotationSpeed = rotationSpeed;
    }
    ///////////////////////////////////////////////////////////////////////

    public void Start()
    {
        // Find camera with 'main camera'tag
        cameraTransform = Camera.main.transform;
        if (cameraTransform == null) Debug.LogWarning("No camera found");

        // Body model bust be first child
        playerBodyTransform = PlayerManager.Instance.transform;//.GetChild(0).transform;
        
        // Needs to know boost value
        PlayerManager.Instance.boostable.BoostValueEvent += OnBoostChangeEvent;
    }

    public void Update()
    {
        verticalPosition = PlayerManager.Instance.transform.position.y;
        movement2D = PlayerManager.Instance.movement2D;

        HandleGravity();
        HandleMovement();
        HandleRotation();
    }
    ///////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Calculates vertical velocity
    /// </summary>
    private void HandleGravity()
    {
        // Player on ground
        if (controller.isGrounded) {
            verticalVelocity = -1f; // Slight downward force to keep grounded
            
            // If fly is pressed and height limit hasn't been reached
            if (PlayerManager.Instance.flyKey && verticalPosition < heightLimit)
                verticalVelocity = flyForce; // Apply jump force
        }
        else { // Player flying
            // If fly is pressed and height limit hasn't been reached
            if (PlayerManager.Instance.flyKey && verticalPosition < heightLimit)
                verticalVelocity += flyForce * Time.deltaTime; // Gradually increase altitude
            else // Apply gravity when fly is not pressed
                verticalVelocity -= gravityValue * Time.deltaTime; // Descend naturally
        }
    }

    /// <summary>
    /// Transforms 2D input into 3D movement, applying vertical velocity
    /// </summary>
    private void HandleMovement()
    {
        // Limit joystick movement and check if it's running
        CheckJoystick();

        // Check if the player is running
        if (PlayerManager.Instance.runKey || PlayerManager.Instance.runJoystick){
            if (PlayerManager.Instance.canRun) { // If able to run (boost available)
                if (PlayerManager.Instance.movement2D.sqrMagnitude != 0) // Is moving
                    RunningEvent?.Invoke(this, null); // Trigger running event
                
                movementSpeed = boostSpeed;
            }
            else  // If not able to run, walk
                movementSpeed = walkSpeed;
        } else
            movementSpeed = walkSpeed;

        // Get direction in 3D space based on camera orientation
        forward = cameraTransform.forward;
        right = cameraTransform.right;

        // Movement vector from 2D to 3D from camera view
        movement3D = right * movement2D.x + forward * movement2D.y;

        // Apply forces to movement vector
        movement3D = new(movement3D.x * movementSpeed, // Apply movement speed
                         verticalVelocity, // Apply vertical velocity
                         movement3D.z * movementSpeed); // Apply movement speed

        // Moves controller in the movement vector
        controller.Move(Time.deltaTime * movement3D);
    }

    /// <summary>
    /// Rotates player to face movement
    /// </summary>
    private void HandleRotation()
    {
        // If there is any movement
        // To maintain rotation when stopping
        if (movement2D != Vector2.zero){
            lookAtPosition = new Vector3(movement3D.x, 0, movement3D.z); // Movement direction
            lookAtRotation = Quaternion.LookRotation(lookAtPosition); // Rotation to movement direction
            
            // From current rotation to movement rotation
            playerBodyTransform.rotation = Quaternion.Slerp(playerBodyTransform.rotation, 
                                                            lookAtRotation, 
                                                            rotationSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Check if joystick has passed keyboard limits to run
    /// </summary>
    void CheckJoystick()
    {
        Vector2 movement2D = PlayerManager.Instance.movement2D;

        // Keyboard inputs are normalized (-1>0<1)
        if (movement2D.x > 1 || movement2D.y > 1 || 
            movement2D.x < -1 || movement2D.y < -1){
            // Limit movement passed keyboard values
            // Ensures the movement difference between both inputs is minimal
            if (movement2D.x > 1)
                movement2D.x = 1f;
            else if (movement2D.x < -1)
                movement2D.x = -1f;
            if (movement2D.y > 1)
                movement2D.y = 1f;
            else if (movement2D.y < -1)
                movement2D.y = -1f;

            PlayerManager.Instance.runJoystick = true;
        }
        else{
            PlayerManager.Instance.runJoystick = false;
        }
    }

    /// <summary>
    /// Called when boost value is modified (only when running)
    /// </summary>
    private void OnBoostChangeEvent(object sender, float currentBoost)
    {
        // All consumed
        if (currentBoost <= 0)
            PlayerManager.Instance.canRun = false; // Can't run
        else
            PlayerManager.Instance.canRun = true;
    }
}