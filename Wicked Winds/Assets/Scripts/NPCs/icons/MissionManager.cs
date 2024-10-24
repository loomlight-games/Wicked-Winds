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
        Debug.Log("Inicio del Manager de Misiones"); //

        allNPCs.Clear();
        NPC[] npcs = FindObjectsOfType<NPC>();
        allNPCs.AddRange(npcs);

        Debug.Log($"Total NPCs encontrados: {allNPCs.Count}"); //
        missionIconPool = missionIconPoolManager.GetMissionIconPool();
        AssignMissions();
    }

    //SI SE USA 
    public void AssignMissions()
    {
        Debug.Log("Iniciando la asignaci�n de misiones..."); //

        if (!CanAssignMissions())
            return;

        assignedNPCs.Clear();
        var missionLists = FilterMissionsByDifficulty();

        int numEasyMissions, numMediumMissions, numHardMissions;
        GetMissionCounts(out numEasyMissions, out numMediumMissions, out numHardMissions);

        Debug.Log($"N�mero de misiones A ASIGNAR: F�cil: {numEasyMissions}, Media: {numMediumMissions}, Dif�cil: {numHardMissions}");//

        AssignMissionsToNPCs(missionLists, numEasyMissions, numMediumMissions, numHardMissions);
        Debug.Log("Finalizada la asignaci�n de misiones.");
    }

    private bool CanAssignMissions()
    {
        if (allNPCs.Count < numMissionsToAssign)
        {
            Debug.LogError("No hay suficientes NPCs para asignar misiones.");
            return false;
        }

        Debug.Log($"Total de NPCs disponibles: {allNPCs.Count}");//
        return true;
    }

    private Dictionary<string, List<MissionData>> FilterMissionsByDifficulty()
    {
        Debug.Log("Filtrando misiones por dificultad...");//

        var missionLists = new Dictionary<string, List<MissionData>>()
        {
            { "easy", new List<MissionData>() },
            { "medium", new List<MissionData>() },
            { "hard", new List<MissionData>() }
        };

        foreach (MissionData mission in availableMissions)
        {
            Debug.Log($"Analizando misi�n: {mission.name} con dificultad: {mission.difficulty}");//
            if (mission.difficulty == 0)
            {
                missionLists["easy"].Add(mission);
                Debug.Log($"Misi�n f�cil a�adida: {mission.name}");//
            }
            else if (mission.difficulty == 1)
            {
                missionLists["medium"].Add(mission);
                Debug.Log($"Misi�n media a�adida: {mission.name}");
            }
            else if (mission.difficulty == 2)
            {
                missionLists["hard"].Add(mission);
                Debug.Log($"Misi�n dif�cil a�adida: {mission.name}");
            }
        }

        Debug.Log($"Misiones f�ciles disponibles: {missionLists["easy"].Count}");
        Debug.Log($"Misiones medias disponibles: {missionLists["medium"].Count}");
        Debug.Log($"Misiones dif�ciles disponibles: {missionLists["hard"].Count}");

        return missionLists;
    }

    private void GetMissionCounts(out int numEasyMissions, out int numMediumMissions, out int numHardMissions)
    {
        Debug.Log("Calculando el n�mero de misiones por dificultad...");//
        numHardMissions = Mathf.Max(0, Mathf.Min(currentRound, numMissionsToAssign));
        numMediumMissions = Mathf.Max(0, Mathf.Min(currentRound - 1, numMissionsToAssign - numHardMissions));
        numEasyMissions = numMissionsToAssign - numMediumMissions - numHardMissions;

        numEasyMissions = Mathf.Max(numEasyMissions, 0);

        Debug.Log($"N�mero calculado de misiones: F�cil: {numEasyMissions}, Media: {numMediumMissions}, Dif�cil: {numHardMissions}");//
    }


    ///
    private void AssignMissionsToNPCs(Dictionary<string, List<MissionData>> missionLists, int numEasyMissions, int numMediumMissions, int numHardMissions)
    {
        Debug.Log("Asignando misiones a NPCs...");//

        List<NPC> shuffledNPCs = new List<NPC>(allNPCs);
        Debug.Log($"Cantidad de NPCs mezclados: {shuffledNPCs.Count}");//

        int assignedCount = 0;

        while (assignedCount < numMissionsToAssign && shuffledNPCs.Count > 0)
        {
            NPC selectedNPC = GetRandomNPC(shuffledNPCs);
            if (selectedNPC == null)
            {
                Debug.Log("No se pudo seleccionar un NPC adecuado, continuando con la siguiente iteraci�n...");
                continue;
            }

            Debug.Log($"NPC seleccionado para asignaci�n: {selectedNPC.name}");
            MissionData mission = SelectMission(missionLists, ref assignedCount, numEasyMissions, numMediumMissions);
            if (mission == null)
            {
                Debug.Log("No se encontr� una misi�n adecuada para asignar al NPC.");
                continue;
            }

            AssignMissionToNPC(selectedNPC, mission);
            assignedCount++;
            Debug.Log($"Misi�n asignada. Total de misiones asignadas: {assignedCount}/{numMissionsToAssign}");///
            shuffledNPCs.Remove(selectedNPC);
        }

        Debug.Log("Todas las misiones han sido asignadas a los NPCs.");
    }

    //SI SE USA
    private NPC GetRandomNPC(List<NPC> shuffledNPCs)//
    {
        int randomIndex = Random.Range(0, shuffledNPCs.Count);
        NPC selectedNPC = shuffledNPCs[randomIndex];

        if (selectedNPC.missionIcon != null)
        {
            Debug.Log($"El NPC {selectedNPC.name} ya tiene una misi�n. Eliminando de la lista.");
            shuffledNPCs.RemoveAt(randomIndex);
            return null; // El NPC ya tiene una misi�n, retorna null
        }

        Debug.Log($"NPC aleatorio seleccionado: {selectedNPC.name}");
        return selectedNPC;
    }

    private MissionData SelectMission(Dictionary<string, List<MissionData>> missionLists, ref int assignedCount, int numEasyMissions, int numMediumMissions)
    {//////
        Debug.Log("Seleccionando misi�n...");
        MissionData mission = null;

        if (assignedCount < numEasyMissions && missionLists["easy"].Count > 0)
        {
            mission = GetRandomMission(missionLists["easy"]);
            Debug.Log($"Misi�n f�cil seleccionada: {mission.name}");
        }
        else if (assignedCount < numEasyMissions + numMediumMissions && missionLists["medium"].Count > 0)
        {
            mission = GetRandomMission(missionLists["medium"]);
            Debug.Log($"Misi�n media seleccionada: {mission.name}");
        }
        else if (missionLists["hard"].Count > 0)
        {
            mission = GetRandomMission(missionLists["hard"]);
            Debug.Log($"Misi�n dif�cil seleccionada: {mission.name}");
        }
        else
        {
            Debug.LogWarning("No hay misiones disponibles para asignar.");
        }

        return mission;
    }

    private void AssignMissionToNPC(NPC selectedNPC, MissionData mission)//////
    {
        Debug.Log($"Asignando misi�n {mission.name} a NPC: {selectedNPC.name}");
        MissionIcon missionIcon = missionIconPool.GetIcon();
        if (missionIcon == null)
        {
            Debug.LogError("No se pudo obtener un MissionIcon del pool.");
            return;
        }

        missionIcon.AssignMission(mission, this);
        SetMissionIconPosition(selectedNPC, missionIcon);
        selectedNPC.missionIcon = missionIcon;
        selectedNPC.hasMission = true; // Actualiza el estado de hasMission
        Debug.Log($"Misi�n {mission.name} asignada correctamente a {selectedNPC.name}");
    }

    private void SetMissionIconPosition(NPC selectedNPC, MissionIcon missionIcon)/////
    {
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
    }

    private MissionData GetRandomMission(List<MissionData> missions)//
    {
        int randomIndex = Random.Range(0, missions.Count);
        Debug.Log($"Misi�n aleatoria seleccionada: {missions[randomIndex].name}");
        return missions[randomIndex]; // Permitir duplicados
    }

    public void UpdateNPCStateAfterMissionComplete(MissionIcon missionIcon)
    {
        Debug.Log($"Actualizando estado del NPC despu�s de completar la misi�n: {missionIcon.currentMission.name}");
        NPC assignedNPC = assignedNPCs.Find(npc => npc.missionIcon == missionIcon);
        if (assignedNPC != null)
        {
            assignedNPC.hasMission = false; // Actualiza el estado del NPC al completar la misi�n
            assignedNPC.bubble.SetActive(false); // Desactiva la burbuja del NPC
            assignedNPC.missionIcon = null; // Restablecer la referencia al icono de la misi�n
            Debug.Log($"El NPC {assignedNPC.name} ha completado su misi�n y su estado ha sido actualizado.");
        }
        else
        {
            Debug.LogWarning("No se encontr� un NPC asignado para la misi�n completada.");
        }

        ReassignMissions();
    }

    private void ReassignMissions()
    {
        Debug.Log("Reasignando misiones a NPCs...");
        int activeMissionsCount = assignedNPCs.Count;

        // Verificar cu�ntas misiones faltan para llegar a 10
        int missionsToReassign = numMissionsToAssign - activeMissionsCount;

        Debug.Log($"Misiones activas: {activeMissionsCount}. Misiones a reasignar: {missionsToReassign}");

        if (missionsToReassign > 0)
        {
            Debug.Log($"Reasignando {missionsToReassign} misiones...");
            AssignMissions();
        }
        else
        {
            Debug.Log("No se requieren m�s misiones para asignar.");
        }
    }
}
