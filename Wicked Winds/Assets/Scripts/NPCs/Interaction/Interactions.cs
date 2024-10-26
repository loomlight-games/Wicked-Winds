using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactions : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float interactRange = 2f;
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            Debug.Log($"Attempting to interact. Found {colliderArray.Length} colliders within range."); // Log para contar colisionadores encontrados

            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent(out Interactable interactable))
                {
                    Debug.Log($"Interacting with interactable: {collider.name}"); // Log para el objeto interactuable
                    interactable.Interact();
                }

                if (collider.TryGetComponent(out Interactable interactable2) && collider.TryGetComponent(out Pickable pickableObject))
                {
                    Debug.Log($"Attempting to collect item: {pickableObject.name}"); // Log para objeto recogible
                    interactable2.CollectItem();
                    pickableObject.CollectItem();
                }
            }
        }
    }
}
