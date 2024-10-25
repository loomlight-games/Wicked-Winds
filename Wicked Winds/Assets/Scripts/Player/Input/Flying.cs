using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flying
{
    readonly CharacterController controller;
    readonly float flyForce, gravity, heightLimit;

    ///////////////////////////////////////////////////////////////////////
    public Flying(CharacterController controller, float flyForce, float gravity, float heightLimit)
    {
        this.controller = controller;
        this.flyForce = flyForce;
        this.gravity = gravity;
        this.heightLimit = heightLimit;
    }
    ///////////////////////////////////////////////////////////////////////
    // Start is called before the first frame update
    public void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        if (controller.isGrounded) {
            PlayerManager.Instance.verticalVelocity = -1f; // Slight downward force to keep grounded
            
            // If fly is pressed and height limit hasn't been reached
            if (PlayerManager.Instance.flyKey && PlayerManager.Instance.transform.position.y < heightLimit)
                PlayerManager.Instance.verticalVelocity = flyForce; // Apply jump force
        }
        else {
            // If fly is pressed and height limit hasn't been reached
            if (PlayerManager.Instance.flyKey && PlayerManager.Instance.transform.position.y < heightLimit)
                PlayerManager.Instance.verticalVelocity += flyForce * Time.deltaTime; // Gradually increase altitude
            else // Apply gravity when fly is not pressed
                PlayerManager.Instance.verticalVelocity -= gravity * Time.deltaTime; // Descend naturally
        }
    }
}
