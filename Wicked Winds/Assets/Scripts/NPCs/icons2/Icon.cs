using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icon : MonoBehaviour, IPoolableObject
{
    //LA CLASE ICON IMPLEMENTA IPOOLABLE OBJECT Y MANEJA LA ASIGNACIÓN Y EL ESTADO DE LAS MISIONES

    public GameObject bubble; // Referencia al GameObject del Bubble
    public Sprite spriteMission;
    public Sprite spriteMissionCompleted;

    public MissionData currentMission; // La misión asignada a este ícono
    private MissionManager missionManager; // Referencia al MissionManager
    private NPC assignedNPC; // Referencia al NPC asignado para este icono
    private IconPool missionIconPool; // Referencia al pool de íconos

    public void AssignMission(MissionData mission, MissionManager manager)
    {
        currentMission = mission;
        missionManager = manager;

        // Actualiza el sprite de la misión
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && spriteMission != null)
        {
            spriteRenderer.sprite = spriteMission;
        }
        else
        {
            Debug.LogError("SpriteRenderer o spriteMission es nulo.");
        }
    }

    public void CompleteMission()
    {
        if (currentMission != null && !currentMission.isCompleted)
        {
            currentMission.isCompleted = true;

            // Cambiar el sprite al de misión completada
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && spriteMissionCompleted != null)
            {
                spriteRenderer.sprite = spriteMissionCompleted;
            }
            else
            {
                Debug.LogError("SpriteRenderer o spriteMissionCompleted es nulo.");
            }

            if (assignedNPC != null)
            {
                assignedNPC.bubble.SetActive(false); // Desactiva la burbuja del NPC
            }

            missionManager.CheckMissionCompletion();
        }
    }

    public void OnObjectReturn()
    {
        // Limpia la referencia del NPC y misión
        if (assignedNPC != null)
        {
            if (assignedNPC.missionIcon != null)
            {
                missionIconPool.ReleaseIcon(assignedNPC.missionIcon.gameObject);
                assignedNPC.missionIcon = null;
            }

            assignedNPC.hasMission = false;
        }

        assignedNPC = null;
        currentMission = null;
    }

    public void setActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public bool isActive()
    {
        return gameObject.activeSelf;
    }

    public void Reset()
    {
        OnObjectReturn();
    }
}
