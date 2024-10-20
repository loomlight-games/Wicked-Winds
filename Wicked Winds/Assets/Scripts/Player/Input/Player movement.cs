using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Properties ////////////////////////////////////////////////////
    // Input
    InputActions playerInput; // Class generated from input map
    bool runKey,runJoystick;
    CharacterController controller;
    // Movement
    public Transform cameraTransform; // Fixed isometric camera
    Vector2 movement2D; // Joystick
    Vector3 movement3D, lookAtPosition; // Player
    Quaternion lookAtRotation;
    float verticalVelocity;
    // Inspector
    public float walkSpeed = 10f;
    public float runSpeed = 15f;
    public float rotationSpeed = 5f;
    float gravity = 9.8f;
    float joystickWalkLimit = 1.1f;

    ///////////////////////////////////////////////////////////////////////
    void Awake()
    {
        controller = GetComponent<CharacterController>();

        playerInput = new();

        // Walk: OnMove is susbcribed to walk inputs
        playerInput.Playermovement.Walk.started += OnMove;
        playerInput.Playermovement.Walk.performed += OnMove;
        playerInput.Playermovement.Walk.canceled += OnMove;

        // Run
        playerInput.Playermovement.Run.started += OnRun;
        playerInput.Playermovement.Run.canceled += OnRun;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();
    }

    /// <summary>
    /// Detects walk inputs: Joystick and WASD
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        movement2D = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Detects run inputs: Left Shift
    /// </summary>
    public void OnRun(InputAction.CallbackContext context)
    {
        runKey = context.ReadValueAsButton();
    }

    /// <summary>
    /// Calculates gravity
    /// </summary>
    float Gravity(){
        if (controller.isGrounded){
            verticalVelocity = -1f;
        }else{
            verticalVelocity -= gravity * Time.deltaTime;
        }
        return verticalVelocity;
    }

    /// <summary>
    /// Moves player in isometric perspective.
    /// </summary>
    void Move(){
        // Get direction in 3D space based on camera orientation
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Ensure the camera's movement stays on the horizontal plane
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize(); // Magnitude of 1
        right.Normalize(); // Magnitude of 1

        // Movement based on 2D input and camera's orientation
        movement3D = right * movement2D.x + forward * movement2D.y;
        movement3D.y = Gravity(); // Add gravity

        //movement2D.Normalize();
        // Check if joystick has passed walk limit
        if (movement2D.x >= joystickWalkLimit || movement2D.y >= joystickWalkLimit || 
            movement2D.x <= -joystickWalkLimit || movement2D.y <= -joystickWalkLimit)
            runJoystick = true;
        else
            runJoystick = false;

        // Run key is pressed
        if (runKey || runJoystick){
            // Run
            controller.Move(runSpeed * Time.deltaTime * movement3D);
        }else{
            // Walk
            controller.Move(walkSpeed * Time.deltaTime * movement3D);
        }
    }

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
}
