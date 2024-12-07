using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableOwl : MonoBehaviour
{
    public OwlController owl;

    private void Start()
    {

        owl = GetComponent<OwlController>();
    }

    // Metodo para manejar la interaccion con el NPC
    public void Interact()
    {
        // Has mission assigned
        if (PlayerManager.Instance.hasActiveMission &&
            PlayerManager.Instance.currentTargets.Contains(gameObject))
        {
            owl.InteractWithOwl();
        }
    }
}
