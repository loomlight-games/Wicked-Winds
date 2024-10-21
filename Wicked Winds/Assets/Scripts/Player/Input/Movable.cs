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
    bool runKey,runJoystick, jumpKey;
    CharacterController controller;
    // Movement
    public event EventHandler<EventArgs> RunningEvent; // Invoked when run trigger
    public Transform cameraTransform; // Isometric camera
    Vector2 movement2D;
    Vector3 movement3D, lookAtPosition;
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
    public float heightLimit = 10f;

    
    ///////////////////////////////////////////////////////////////////////
    void Awake()
    {
        controller = GetComponent<CharacterController>();

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
        if (controller.isGrounded) {
            verticalVelocity = -1f; // Slight downward force to keep grounded
            
            // If jump is pressed and height limit hasn't been reached
            if (jumpKey && transform.position.y < heightLimit)
                verticalVelocity = jumpForce; // Apply jump force
        }
        else {
            // If jump is pressed and height limit hasn't been reached
            if (jumpKey && transform.position.y < heightLimit)
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

        // 3DMovement based on 2D input and camera's orientation, 
        // Without vertical velicity applied bc will be affected by running
        movement3D = right * movement2D.x + forward * movement2D.y;
        
        // Check if the player is running
        if (runKey || runJoystick){
            if (canRun) { // If able to run (boost available)
                if (movement2D.sqrMagnitude != 0) // Is moving
                    RunningEvent?.Invoke(this, null); // Trigger running event
                
                movementSpeed = runSpeed;
            }
            else  // If not able to run, walk
                movementSpeed = walkSpeed;
        } else
            movementSpeed = walkSpeed;

        // Move character applying vertical velocity
        controller.Move((movement3D * movementSpeed + verticalVelocity * Vector3.up) * Time.deltaTime);
        
        // Apply both movement and vertical (jump/gravity) logic together
        Vector3 movementWithJump = movement3D * movementSpeed + verticalVelocity * Vector3.up;
    
        // Move the character based on combined movement and jump
        controller.Move(movementWithJump * Time.deltaTime);
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


        // Ensure joystick running is still detected
        CheckJoystick();
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
    /// Detects jump inputs
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