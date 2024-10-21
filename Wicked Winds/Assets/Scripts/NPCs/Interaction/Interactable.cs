using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public MissionIcon missionIcon; // Referencia al �cono de misi�n del NPC

    public void Interact()
    {
        Debug.Log("Interacted with NPC");
        if (missionIcon != null)
        {
            missionIcon.CompleteMission(); // Marca la misi�n como completada
        }
    }
}
