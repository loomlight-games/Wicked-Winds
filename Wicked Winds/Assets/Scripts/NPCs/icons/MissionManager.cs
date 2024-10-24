using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    //El MissionManager es responsable de gestionar las misiones, asignando
    //instancias de Icon a los NPC. En este caso, el MissionManager usa el IconPool para obtener instancias de Icon.
    [SerializeField] private List<NPC> assignedNPCs;
    [SerializeField] private List<MissionData> availableMissions;
    private int assignedCount;

    void Start()
    {
        AssignMissions();
    }

    // Método para asignar misiones a los NPCs
    public void AssignMissions()
    {
        foreach (NPC npc in assignedNPCs)
        {
            if (!npc.hasMission && availableMissions.Count > 0)
            {
                // Obtener la primera misión disponible
                MissionData mission = availableMissions[0];
                availableMissions.RemoveAt(0);

                // Obtener un ícono del pool
                Icon missionIcon = IconPool.Instance.get() as Icon;
                if (missionIcon == null)
                {
                    Debug.LogError("No se pudo obtener un MissionIcon del pool.");
                    return;
                }

                // Asignar la misión al ícono obtenido
                missionIcon.AssignMission(mission, this);

                // Asignar el ícono al NPC
                Transform bubbleTransform = npc.transform.Find("Bubble");
                if (bubbleTransform != null)
                {
                    missionIcon.transform.SetParent(bubbleTransform, false);
                    missionIcon.transform.localPosition = Vector3.zero;
                    missionIcon.transform.localScale = Vector3.one;
                }
                else
                {
                    Debug.LogError($"El NPC {npc.name} no tiene un componente 'Bubble'.");
                }

                npc.missionIcon = missionIcon;
                npc.hasMission = true;
                assignedCount++;
            }
        }
    }

    // Verificar si todas las misiones se completaron
    public void CheckMissionCompletion()
    {
        bool allCompleted = true;
        foreach (NPC npc in assignedNPCs)
        {
            if (npc.hasMission && !npc.missionIcon.currentMission.isCompleted)
            {
                allCompleted = false;
                break;
            }
        }

        if (allCompleted)
        {
            Debug.Log("Todas las misiones han sido completadas.");
        }
    }

    // Método que actualiza el estado del NPC después de completar la misión
    public void UpdateNPCStateAfterMissionComplete(MissionIcon missionIcon)
    {
        NPC assignedNPC = assignedNPCs.Find(npc => npc.missionIcon == missionIcon);
        if (assignedNPC != null)
        {
            assignedNPC.hasMission = false; // Actualiza el estado del NPC al completar la misión
            assignedNPC.bubble.SetActive(false); // Desactiva la burbuja del NPC
            assignedNPC.missionIcon = null; // Restablecer la referencia al icono de la misión

            // Devolver el ícono al pool
            IconPool.Instance.ReleaseIcon(missionIcon.gameObject);
        }

        ReassignMissions();
    }

    // Método opcional para reasignar misiones si es necesario
    private void ReassignMissions()
    {
        // Lógica opcional para reasignar misiones
    }
}
