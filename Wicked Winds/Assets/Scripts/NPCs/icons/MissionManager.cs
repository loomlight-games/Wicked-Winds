using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public MissionData[] availableMissions; // Todas las misiones disponibles
    public List<NPC> allNPCs;
    public const int numMissionsToAssign = 10; // Número de misiones por ronda
    public MissionIconPoolManager missionIconPoolManager; // Asigna el GameObject del Pool Manager
    private AObjectPool<MissionIcon> missionIconPool;

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

    // Método para asignar misiones a NPCs de manera escalonada por dificultad
    public void AssignMissions()
    {
        Debug.Log("Iniciando la asignación de misiones...");

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
                Debug.Log($"Misión fácil añadida: {mission.name}");
            }
            else if (mission.difficulty == 1)
            {
                mediumMissions.Add(mission);
                Debug.Log($"Misión media añadida: {mission.name}");
            }
            else if (mission.difficulty == 2)
            {
                hardMissions.Add(mission);
                Debug.Log($"Misión difícil añadida: {mission.name}");
            }
        }

        Debug.Log($"Misiones fáciles disponibles: {easyMissions.Count}");
        Debug.Log($"Misiones medias disponibles: {mediumMissions.Count}");
        Debug.Log($"Misiones difíciles disponibles: {hardMissions.Count}");

        // Determinar cuántas misiones de cada dificultad asignar
        int numHardMissions = Mathf.Max(0, Mathf.Min(currentRound / 2, numMissionsToAssign));
        int numMediumMissions = Mathf.Max(0, Mathf.Min(currentRound - 1, numMissionsToAssign - numHardMissions));
        int numEasyMissions = numMissionsToAssign - numMediumMissions - numHardMissions;

        Debug.Log($"Número de misiones A ASIGNAR: Fácil: {numEasyMissions}, Media: {numMediumMissions}, Difícil: {numHardMissions}");

        // Asignar las misiones a los NPCs
        List<NPC> shuffledNPCs = new List<NPC>(allNPCs);
        Debug.Log($"Shuffled NPCS COUNT: {shuffledNPCs.Count}");

        // Contador para las misiones asignadas
        int assignedCount = 0;

        while (assignedCount < numMissionsToAssign && shuffledNPCs.Count > 0)
        {
            int randomIndex = Random.Range(0, shuffledNPCs.Count);
            NPC selectedNPC = shuffledNPCs[randomIndex];

            Debug.Log($"NPC {selectedNPC.missionIcon} ya tiene una misión, saltando.");
            // Verificar si el NPC ya tiene una misión asignada
            if (selectedNPC.missionIcon != null)
            {
                Debug.Log($"NPC {selectedNPC.name} ya tiene una misión, saltando.");
                shuffledNPCs.RemoveAt(randomIndex); // Eliminar NPC que ya tiene una misión
                continue; // Pasar al siguiente NPC
            }

            // Asignar misión al NPC
            assignedNPCs.Add(selectedNPC);
            Debug.Log($"Asignando misión a NPC: {selectedNPC.name}");

            // Elegir una misión según la dificultad y el contador
            MissionData mission = null;
            if (assignedCount < numEasyMissions && easyMissions.Count > 0)
            {
                mission = GetRandomMission(easyMissions);
                Debug.Log($"Asignando misión fácil: {mission.name}");
            }
            else if (assignedCount < numEasyMissions + numMediumMissions && mediumMissions.Count > 0)
            {
                mission = GetRandomMission(mediumMissions);
                Debug.Log($"Asignando misión media: {mission.name}");
            }
            else if (hardMissions.Count > 0)
            {
                mission = GetRandomMission(hardMissions);
                Debug.Log($"Asignando misión difícil: {mission.name}");
            }

            // Verificar si se encontró una misión
            if (mission == null)
            {
                Debug.LogError("No se pudo encontrar una misión para asignar.");
                continue; // Saltar a la siguiente iteración si no se encuentra una misión
            }

            // Obtener un icono de misión del pool
            MissionIcon missionIcon = missionIconPool.GetObject();
            if (missionIcon == null)
            {
                Debug.LogError("No se pudo obtener un MissionIcon del pool.");
                return;
            }

            missionIcon.AssignMission(mission, this);

            // Asignar el ícono a la burbuja del NPC
            Transform bubbleTransform = selectedNPC.transform.Find("Bubble");
            if (bubbleTransform != null)
            {
                missionIcon.transform.SetParent(bubbleTransform, false);
                Vector3 desiredPosition = new Vector3(1.3051f, 0.9899999f, 0f);
                Vector3 desiredScale = new Vector3(0.1777365f, 0.6312358f, 0.4208236f);

                missionIcon.transform.localPosition = desiredPosition;
                missionIcon.transform.localScale = desiredScale;
                Debug.Log($"Icono de misión asignado a {selectedNPC.name} dentro de Bubble.");
                // Llamar a AssignMission para asignar el sprite y mostrarlo
                missionIcon.AssignMission(mission, this);
            }
            else
            {
                Debug.LogError($"El NPC {selectedNPC.name} no tiene un componente 'Bubble'.");
            }

            selectedNPC.missionIcon = missionIcon; // Asignar el icono al NPC
            selectedNPC.hasMission = true; // Actualiza el estado de hasMission
            assignedCount++; // Aumentar el contador de misiones asignadas

            // Finalmente, eliminar el NPC de la lista después de asignar la misión
            shuffledNPCs.RemoveAt(randomIndex);
        }

        Debug.Log("Finalizada la asignación de misiones.");
    }


    // Método para obtener una misión aleatoria (puede repetirse)
    private MissionData GetRandomMission(List<MissionData> missions)
    {
        int randomIndex = Random.Range(0, missions.Count);
        return missions[randomIndex]; // Permitir duplicados
    }

    //actualización del estado del NPC después de que complete su misión
    public void UpdateNPCStateAfterMissionComplete(MissionIcon missionIcon)
    {
        NPC assignedNPC = assignedNPCs.Find(npc => npc.missionIcon == missionIcon);
        if (assignedNPC != null)
        {
            assignedNPC.hasMission = false; // Actualiza el estado del NPC al completar la misión
            assignedNPC.bubble.SetActive(false); // Desactiva la burbuja del NPC
        }
    }

    // Verificar si todas las misiones se completaron
    public void CheckMissionCompletion()
    {
        bool allCompleted = true;
        foreach (NPC npc in assignedNPCs)
        {
            if (npc.missionIcon != null && npc.missionIcon.currentMission != null)
            {
                if (!npc.missionIcon.currentMission.isCompleted)
                {
                    allCompleted = false;
                    break;
                }
            }
        }

        if (allCompleted)
        {
            Debug.Log("Todas las misiones completadas. Avanzando a la siguiente ronda.");
            currentRound++;
            // Reasignar misiones
            AssignMissions();
        }
    }

}
