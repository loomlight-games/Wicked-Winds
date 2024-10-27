using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    Vector3 playerVelocity;
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
        playerBodyTransform = PlayerManager.Instance.transform.GetChild(0).transform;

        // Needs to know boost value
        PlayerManager.Instance.boostable.BoostValueEvent += OnBoostChangeEvent;
    }

    public void Update()
    {
        // Checks if it's grounded
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f; // No need to apply any velocty
        }

        // Input movement
        Vector2 movement2D = PlayerManager.Instance.movement2D;

        // Get direction in 3D space based on camera orientation
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Movement vector from 2D to 3D from camera view - no vertical apply
        Vector3 movement3D = right * movement2D.x + forward * movement2D.y;
        movement3D.y = 0f;
        controller.Move(walkSpeed * Time.deltaTime * movement3D);

        // Changes the height position of the player
        if (PlayerManager.Instance.flyKey && controller.isGrounded)
        {
            playerVelocity.y += Mathf.Sqrt(flyForce * -3.0f * gravityValue);
        }

        // Applies vertical velocity
        //playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // Handle rotation
        if (movement2D != Vector2.zero){
            // Rotation in y from camera
            Quaternion newRotation = Quaternion.Euler(new Vector3(playerBodyTransform.localEulerAngles.x,
                                                            cameraTransform.localEulerAngles.y, 
                                                            playerBodyTransform.localEulerAngles.z));
            
            // Updates body rotation smoothly
            playerBodyTransform.rotation = Quaternion.Lerp(playerBodyTransform.rotation, 
                                                            newRotation, 
                                                            Time.deltaTime * rotationSpeed);
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