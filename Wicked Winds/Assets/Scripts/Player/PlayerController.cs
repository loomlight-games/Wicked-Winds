using System;
using UnityEngine;

/// <summary>
/// Gives movement, high velocity and high flight (when possible) to player
/// </summary>
public class PlayerController
{
    #region PROPERTIES
    CharacterController controller;

    float walkSpeed,
        boostSpeed,
        lowerHeightLimit,
        maxHeightLimit,
        flyForce,
        gravityForce,
        rotationSpeed,
        movementSpeed,
        verticalVelocity,
        verticalPosition,
        flyPotionLoss,
        speedPotionLoss;

    public float MAX_VALUE = 100;
    public float flyPotionValue, speedPotionValue;

    Vector3 movement3D,
        forward,
        right;

    Vector2 movement2D;

    Transform cameraTransform,
            player,
            orientation;
    #endregion

    ///////////////////////////////////////////////////////////////////////////////////
    public void Start()
    {
        flyPotionValue = MAX_VALUE;
        speedPotionValue = MAX_VALUE;

        controller = PlayerManager.Instance.controller;
        player = PlayerManager.Instance.transform;
        orientation = PlayerManager.Instance.orientation;

        cameraTransform = Camera.main.transform; // Find camera with 'main camera'tag
        if (cameraTransform == null) Debug.LogWarning("No camera found");

        PlayerManager.Instance.controllableState.SpeedPotionCollected += SpeedPotionGain;
        PlayerManager.Instance.controllableState.FlyPotionCollected += FlyPotionGain;
        
    }

    public void Update()
    {
        verticalPosition = PlayerManager.Instance.transform.position.y;
        movement2D = PlayerManager.Instance.movement2D;
        walkSpeed = PlayerManager.Instance.walkSpeed;
        boostSpeed = PlayerManager.Instance.boostSpeed;
        flyForce = PlayerManager.Instance.flyForce;
        gravityForce = PlayerManager.Instance.gravityForce;
        lowerHeightLimit = PlayerManager.Instance.lowerHeightLimit;
        maxHeightLimit = PlayerManager.Instance.maxHeightLimit;
        rotationSpeed = PlayerManager.Instance.rotationSpeed;
        speedPotionLoss = PlayerManager.Instance.speedPotionLossPerSecond;
        flyPotionLoss = PlayerManager.Instance.flyPotionLossPerSecond;

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
        if (controller.isGrounded)
        {
            // Fly key pressed -> fly
            if (PlayerManager.Instance.flyKey)
                verticalVelocity = flyForce;
            // Not pressed -> small gravity to keep grounded
            else
                verticalVelocity = -1f;
        } // Not on ground
        else
        {
            // Fly key pressed
            if (PlayerManager.Instance.flyKey)
            {
                // Reached max height limit
                if (verticalPosition >= maxHeightLimit)
                {
                    // Enough fly potion -> reduce its value
                    if (flyPotionValue >= 0f)
                    {
                        flyPotionValue -= flyPotionLoss * Time.deltaTime;
                        // Maintain in limit
                        verticalVelocity = flyForce * Time.deltaTime;
                    } // No fly potion -> gravity
                    else
                        verticalVelocity -= gravityForce * Time.deltaTime;
                } // Not reached
                else
                {
                    // Overcomed lower height limit
                    if (verticalPosition > lowerHeightLimit)
                    {
                        // Enough fly potion -> fly and reduce its value
                        if (flyPotionValue >= 0f)
                        {
                            verticalVelocity += flyForce * Time.deltaTime;
                            // Reduce fly potion value
                            flyPotionValue -= flyPotionLoss * Time.deltaTime;
                        } // No fly potion -> gravity
                        else
                            verticalVelocity -= gravityForce * Time.deltaTime;
                    } // Reached and no fly potion -> maintain in limit
                    else if (lowerHeightLimit - verticalPosition < 0.1f && flyPotionValue <= 0f)
                        verticalVelocity = flyForce * Time.deltaTime;
                    // Not reached -> fly
                    else if (verticalPosition < lowerHeightLimit)
                        verticalVelocity += flyForce * Time.deltaTime;
                }
                // Not pressed -> gravity
            }
            else
                verticalVelocity -= gravityForce * Time.deltaTime;
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
        if (PlayerManager.Instance.runKey || PlayerManager.Instance.runJoystick)
        {
            if (speedPotionValue >= 0f)
            { // If able to run (speed potion available)
                if (movement2D.sqrMagnitude != 0) // Is moving
                    // Reduce speed potion value
                    speedPotionValue -= speedPotionLoss * Time.deltaTime;

                movementSpeed = boostSpeed;
            }
            else  // If not able to run, walk
                movementSpeed = walkSpeed;
        }
        else
            movementSpeed = walkSpeed;

        // Get direction in 3D space based on camera orientation
        forward = cameraTransform.forward;
        right = cameraTransform.right;

        forward.Normalize();
        right.Normalize();

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
        if (movement2D != Vector2.zero)
        {
            // Rotate orientation
            Vector3 viewDir = player.position - new Vector3(cameraTransform.position.x, player.position.y, cameraTransform.position.z);
            orientation.forward = viewDir.normalized;

            Vector3 inputDir = orientation.forward * movement2D.y + orientation.right * movement2D.x;

            player.forward = Vector3.Slerp(player.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
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
            movement2D.x < -1 || movement2D.y < -1)
        {
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
        else
        {
            PlayerManager.Instance.runJoystick = false;
        }
    }

    /// <summary>
    /// Recovers speed potion value
    /// </summary>
    public void SpeedPotionGain(object sender, EventArgs e)
    {
        speedPotionValue = MAX_VALUE;
    }



    /// <summary>
    /// Recovers fly potion value
    /// </summary>
    private void FlyPotionGain(object sender, EventArgs e)
    {
        flyPotionValue = MAX_VALUE;
    }
}