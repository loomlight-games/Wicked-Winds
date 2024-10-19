using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Properties
    InputActions playerInput; // Class generated from input map
    bool runKey;
    public Transform cameraTransform; // Fixed isometric camera
    CharacterController controller;
    Vector2 movement2D; // Joystick
    Vector3 movement3D, lookAtPosition; // Player
    Quaternion lookAtRotation;
    public float walkSpeed = 10f;
    public float runSpeed = 15f;
    public float rotationSpeed = 5f;
    public float gravity = 9.8f;
    float verticalVelocity;

    
    void Awake()
    {
        controller = GetComponent<CharacterController>();

        playerInput = new();

        // Walk
        playerInput.Playermovement.Walk.started += OnMove;
        playerInput.Playermovement.Walk.performed += OnMove; // Valores medios
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

    public void OnMove(InputAction.CallbackContext context)
    {
        movement2D = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        runKey = context.ReadValueAsButton();
    }

    float Gravity(){
        if (controller.isGrounded){
            verticalVelocity = -1f;
        }else{
            verticalVelocity -= gravity * Time.deltaTime;
        }
        return verticalVelocity;
    }

    void Move(){
        // Get direction in 3D space based on camera orientation
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Ensure the camera's movement stays on the horizontal plane
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        // Movement based on 2D input and camera's orientation
        movement3D = right * movement2D.x + forward * movement2D.y;
        movement3D.y = Gravity(); // Add gravity

        if (runKey){
            // Run
            controller.Move(runSpeed * Time.deltaTime * movement3D);
        }else{
            // Walk
            controller.Move(walkSpeed * Time.deltaTime * movement3D);
        }
    }

    void Rotate(){
        // Only rotate if there is movement input
        // To ensure that rotation is the same when stops moving to when it was
        if (movement3D.x != 0 || movement3D.z != 0)
        {
            lookAtPosition = new Vector3(movement3D.x, 0, movement3D.z);
            lookAtRotation = Quaternion.LookRotation(lookAtPosition);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookAtRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
