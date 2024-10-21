using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public MissionIcon missionIcon; // Referencia al ícono de misión del NPC

    public void Interact()
    {
        Debug.Log("Interacted with NPC");
        if (missionIcon != null)
        {
            missionIcon.CompleteMission(); // Marca la misión como completada
        }
    }
}
