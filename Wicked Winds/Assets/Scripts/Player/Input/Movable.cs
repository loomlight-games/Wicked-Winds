using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Detects inputs (joystick and keyboard) and moves object
/// Only runs when enough boost
/// </summary>

public class Movable
{
    public event EventHandler<EventArgs> RunningEvent; // Invoked when run trigger

    Transform cameraTransform; // Isometric camera

    Vector3 movement3D, 
        lookAtPosition;

    Quaternion lookAtRotation;

    float movementSpeed;

    readonly float walkSpeed, 
        boostSpeed, 
        rotationSpeed;

    readonly CharacterController controller;

    ///////////////////////////////////////////////////////////////////////
    public Movable(CharacterController controller, float walkSpeed, float boostSpeed, float rotationSpeed)
    {
        this.controller = controller;
        this.walkSpeed = walkSpeed;
        this.boostSpeed = boostSpeed;
        this.rotationSpeed = rotationSpeed;
    }
    ///////////////////////////////////////////////////////////////////////
    public void Start()
    {
        cameraTransform = Camera.main.transform;

        // Needs to know boost value
        PlayerManager.Instance.boostable.BoostValueEvent += OnBoostChangeEvent;
    }

    // Update is called once per frame
    public void Update()
    {
        Move();
        Rotate();
    }

    ///////////////////////////////////////////////////////////////////////

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

        // 3DMovement based on 2D input and camera's orientation, 
        // Without vertical velicity applied bc will be affected by running
        movement3D = right * PlayerManager.Instance.movement2D.x + forward * PlayerManager.Instance.movement2D.y;
        
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

        // Apply both movement and vertical (jump/gravity) logic together
        Vector3 movementWithFly = movement3D * movementSpeed + PlayerManager.Instance.verticalVelocity * Vector3.up;
    
        // Move the character based on combined movement and jump
        controller.Move(movementWithFly * Time.deltaTime);
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
            PlayerManager.Instance.transform.rotation = Quaternion.Slerp(PlayerManager.Instance.transform.rotation, lookAtRotation, rotationSpeed * Time.deltaTime);
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
            // Ensures the movemeent difference between both inputs is minimal
            if (movement2D.x > 1)
                movement2D.x = 1.1f;
            else if (movement2D.x < -1)
                movement2D.x = -1.1f;
            if (movement2D.y > 1)
                movement2D.y = 1.1f;
            else if (movement2D.y < -1)
                movement2D.y = -1.1f;

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