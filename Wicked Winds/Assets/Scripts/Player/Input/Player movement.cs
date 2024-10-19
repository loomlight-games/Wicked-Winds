using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playermovement : MonoBehaviour
{
    // Properties
    public Transform cameraTransform; // Fixed isometric camera
    CharacterController controller;
    Vector2 movement2D; // Joystick
    Vector3 movement3D, lookAtPosition; // Player
    Quaternion lookAtRotation;
    public float moveSpeed = 10f;
    public float rotationSpeed = 5f;
    public float gravity = 9.8f;
    float verticalVelocity;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();  
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();
    }

    public void Input(InputAction.CallbackContext context){
        movement2D = context.ReadValue<Vector2>();
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
        movement3D = (right * movement2D.x + forward * movement2D.y);
        movement3D.y = Gravity(); // Add gravity

        // Move the player with the CharacterController
        controller.Move(moveSpeed * Time.deltaTime * movement3D);
    }

    void Rotate(){
        // Only rotate if there is movement input
        if (movement3D.x != 0 || movement3D.z != 0)
        {
            lookAtPosition = new Vector3(movement3D.x, 0, movement3D.z);
            lookAtRotation = Quaternion.LookRotation(lookAtPosition);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookAtRotation, rotationSpeed * Time.deltaTime);
        }
    }


}
