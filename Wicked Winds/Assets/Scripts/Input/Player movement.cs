using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playermovement : MonoBehaviour
{
    // Properties
    Vector2 movement2D; // Stick
    Vector3 movement3D; // Player
    public float speed = 8f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement3D = new Vector3(movement2D.x, 0, movement2D.y);
        movement3D.Normalize();

        transform.Translate(speed * movement3D * Time.deltaTime);
    }

    public void Input(InputAction.CallbackContext context){
        movement2D = context.ReadValue<Vector2>();
    }
}
