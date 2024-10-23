using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public MissionData[] availableMissions; // Todas las misiones disponibles
    public List<NPC> allNPCs;
    public const int numMissionsToAssign = 3; // N�mero de misiones por ronda
    public MissionIconPoolManager missionIconPoolManager; // Asigna el GameObject del Pool Manager
    private MissionIconPool missionIconPool;

    public List<NPC> assignedNPCs = new List<NPC>();
    private int currentRound = 1; // Empezamos con la primera ronda

    void Start()
    {
        allNPCs.Clear();
        NPC[] npcs = FindObjectsOfType<NPC>();
        allNPCs.AddRange(npcs);
        missionIconPool = missionIconPoolManager.GetMissionIconPool();
        AssignMissions();
    }

    // M�todo para asignar misiones a NPCs de manera escalonada por dificultad
    public void AssignMissions()
    {
        Debug.Log("Iniciando la asignaci�n de misiones...");

        if (allNPCs.Count < numMissionsToAssign)
        {
            Debug.LogError("No hay suficientes NPCs para asignar misiones.");
            return;
        }

        Debug.Log($"Total de NPCs disponibles: {allNPCs.Count}");
        assignedNPCs.Clear();

        // Filtrar las misiones por dificultad
        List<MissionData> easyMissions = new List<MissionData>();
        List<MissionData> mediumMissions = new List<MissionData>();
        List<MissionData> hardMissions = new List<MissionData>();

        foreach (MissionData mission in availableMissions)
        {
            if (mission.difficulty == 0)
            {
                easyMissions.Add(mission);
                Debug.Log($"Misi�n f�cil a�adida: {mission.name}");
            }
            else if (mission.difficulty == 1)
            {
                mediumMissions.Add(mission);
                Debug.Log($"Misi�n media a�adida: {mission.name}");
            }
            else if (mission.difficulty == 2)
            {
                hardMissions.Add(mission);
                Debug.Log($"Misi�n dif�cil a�adida: {mission.name}");
            }
        }

        Debug.Log($"Misiones f�ciles disponibles: {easyMissions.Count}");
        Debug.Log($"Misiones medias disponibles: {mediumMissions.Count}");
        Debug.Log($"Misiones dif�ciles disponibles: {hardMissions.Count}");

        // Determinar cu�ntas misiones de cada dificultad asignar
        int numHardMissions = Mathf.Max(0, Mathf.Min(currentRound, numMissionsToAssign)); // Incrementa con cada ronda
        int numMediumMissions = Mathf.Max(0, Mathf.Min(currentRound - 1, numMissionsToAssign - numHardMissions)); // Aumenta ligeramente
        int numEasyMissions = numMissionsToAssign - numMediumMissions - numHardMissions;

        // Aseg�rate de que no se asigne un n�mero negativo de misiones
        numEasyMissions = Mathf.Max(numEasyMissions, 0);

        Debug.Log($"N�mero de misiones A ASIGNAR: F�cil: {numEasyMissions}, Media: {numMediumMissions}, Dif�cil: {numHardMissions}");

        // Asignar las misiones a los NPCs
        List<NPC> shuffledNPCs = new List<NPC>(allNPCs);
        Debug.Log($"Shuffled NPCS COUNT: {shuffledNPCs.Count}");

        // Contador para las misiones asignadas
        int assignedCount = 0;

        while (assignedCount < numMissionsToAssign && shuffledNPCs.Count > 0)
        {
            int randomIndex = Random.Range(0, shuffledNPCs.Count);
            NPC selectedNPC = shuffledNPCs[randomIndex];

            // Verificar si el NPC ya tiene una misi�n asignada
            if (selectedNPC.missionIcon != null)
            {
                shuffledNPCs.RemoveAt(randomIndex); // Eliminar NPC que ya tiene una misi�n
                continue; // Pasar al siguiente NPC
            }

            // Asignar misi�n al NPC
            assignedNPCs.Add(selectedNPC);
            Debug.Log($"Asignando misi�n a NPC: {selectedNPC.name}");

            // Elegir una misi�n seg�n la dificultad y el contador
            MissionData mission = null;
            if (assignedCount < numEasyMissions && easyMissions.Count > 0)
            {
                mission = GetRandomMission(easyMissions);
                Debug.Log($"Asignando misi�n f�cil: {mission.name}");
            }
            else if (assignedCount < numEasyMissions + numMediumMissions && mediumMissions.Count > 0)
            {
                mission = GetRandomMission(mediumMissions);
                Debug.Log($"Asignando misi�n media: {mission.name}");
            }
            else if (hardMissions.Count > 0)
            {
                mission = GetRandomMission(hardMissions);
                Debug.Log($"Asignando misi�n dif�cil: {mission.name}");
            }

            // Verificar si se encontr� una misi�n
            if (mission == null)
            {
                Debug.LogError("No se pudo encontrar una misi�n para asignar.");
                continue; // Saltar a la siguiente iteraci�n si no se encuentra una misi�n
            }

            // Obtener un icono de misi�n del pool
            MissionIcon missionIcon = missionIconPool.GetIcon();
            if (missionIcon == null)
            {
                Debug.LogError("No se pudo obtener un MissionIcon del pool.");
                return;
            }

            missionIcon.AssignMission(mission, this);

            // Asignar el �cono a la burbuja del NPC
            Transform bubbleTransform = selectedNPC.transform.Find("Bubble");
            if (bubbleTransform != null)
            {
                missionIcon.transform.SetParent(bubbleTransform, false);
                Vector3 desiredPosition = new Vector3(1.3051f, 0.9899999f, 0f);
                Vector3 desiredScale = new Vector3(0.1777365f, 0.6312358f, 0.4208236f);

                missionIcon.transform.localPosition = desiredPosition;
                missionIcon.transform.localScale = desiredScale;
                Debug.Log($"Icono de misi�n asignado a {selectedNPC.name} dentro de Bubble.");
            }
            else
            {
                Debug.LogError($"El NPC {selectedNPC.name} no tiene un componente 'Bubble'.");
            }

            selectedNPC.missionIcon = missionIcon; // Asignar el icono al NPC
            selectedNPC.hasMission = true; // Actualiza el estado de hasMission
            assignedCount++; // Aumentar el contador de misiones asignadas

            // Finalmente, eliminar el NPC de la lista despu�s de asignar la misi�n
            shuffledNPCs.RemoveAt(randomIndex);
        }

        Debug.Log("Finalizada la asignaci�n de misiones.");
    }

    // M�todo para obtener una misi�n aleatoria (puede repetirse)
    private MissionData GetRandomMission(List<MissionData> missions)
    {
        int randomIndex = Random.Range(0, missions.Count);
        return missions[randomIndex]; // Permitir duplicados
    }

    // Actualizaci�n del estado del NPC despu�s de que complete su misi�n
    public void UpdateNPCStateAfterMissionComplete(MissionIcon missionIcon)
    {
        NPC assignedNPC = assignedNPCs.Find(npc => npc.missionIcon == missionIcon);
        if (assignedNPC != null)
        {
            assignedNPC.hasMission = false; // Actualiza el estado del NPC al completar la misi�n
            assignedNPC.bubble.SetActive(false); // Desactiva la burbuja del NPC
            assignedNPC.missionIcon = null; // Restablecer la referencia al icono de la misi�n
        }

        // Reasignar misiones si hay menos de 10 activas
        ReassignMissions();
    }

    // M�todo para reasignar misiones
    private void ReassignMissions()
    {
        int activeMissionsCount = assignedNPCs.Count;

        // Verificar cu�ntas misiones faltan para llegar a 10
        int missionsToReassign = numMissionsToAssign - activeMissionsCount;

        if (missionsToReassign > 0)
        {
            Debug.Log($"Reasignando {missionsToReassign} misiones...");
            AssignMissions();
        }
    }

    // Verificar si todas las misiones se completaron
    public void CheckMissionCompletion()
    {
        bool allCompleted = true;
        foreach (NPC npc in assignedNPCs)
        {
            if (npc.missionIcon != null && npc.missionIcon.isActiveAndEnabled)
            {
                allCompleted = false;
                break; // Salir si hay alguna misi�n activa
            }
        }

        if (allCompleted)
        {
            Debug.Log("Todas las misiones han sido completadas.");
            // Aqu� puedes a�adir l�gica adicional si todas las misiones se completaron
            currentRound++; // Incrementar la ronda al completar todas las misiones
            AssignMissions(); // Asignar nuevas misiones despu�s de completar
        }
    }
}
