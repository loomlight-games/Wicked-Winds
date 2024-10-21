using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public MissionData[] availableMissions; // Todas las misiones disponibles
    public List<NPC> allNPCs;
    public int numMissionsToAssign = 10; // Número de misiones por ronda
    public MissionIconPoolManager missionIconPoolManager; // Asigna el GameObject del Pool Manager
    private AObjectPool<MissionIcon> missionIconPool;

    private List<NPC> assignedNPCs = new List<NPC>();
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
        int numHardMissions = Mathf.Max(0, Mathf.Min(currentRound / 2, numMissionsToAssign)); // Incrementa la dificultad cada dos rondas
        int numMediumMissions = Mathf.Max(0, Mathf.Min(currentRound - 1, numMissionsToAssign - numHardMissions)); // Aumentar en función de la ronda
        int numEasyMissions = numMissionsToAssign - numMediumMissions - numHardMissions;

   

        Debug.Log($"Número de misiones asignadas: Fácil: {numEasyMissions}, Media: {numMediumMissions}, Difícil: {numHardMissions}");

        // Asignar las misiones a los NPCs
        List<NPC> shuffledNPCs = new List<NPC>(allNPCs);
        for (int i = 0; i < numMissionsToAssign; i++)
        {
            if (shuffledNPCs.Count == 0)
            {
                Debug.LogError("No hay más NPCs disponibles para asignar misiones.");
                break;
            }

            int randomIndex = Random.Range(0, shuffledNPCs.Count);
            NPC selectedNPC = shuffledNPCs[randomIndex];
            shuffledNPCs.RemoveAt(randomIndex);
            assignedNPCs.Add(selectedNPC);
            Debug.Log($"Asignando misión a NPC: {selectedNPC.name}");

            // Elegir la misión de la dificultad adecuada
            MissionData mission = null;
            if (i < numEasyMissions && easyMissions.Count > 0)
            {
                mission = GetRandomMission(easyMissions);
                Debug.Log($"Asignando misión fácil: {mission.name}");
            }
            else if (i < numEasyMissions + numMediumMissions && mediumMissions.Count > 0)
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

            // Acceder al componente `bubble` del NPC
            Transform bubbleTransform = selectedNPC.transform.Find("Bubble"); // Asegúrate de que el objeto hijo se llama "Bubble"
            if (bubbleTransform != null)
            {
                missionIcon.transform.SetParent(bubbleTransform, false); // Asigna el icono como hijo del bubble
                                                                         // Establecer la posición y escala deseada
                Vector3 desiredPosition = new Vector3(1.3051f, 0.9899999f, 0f); // Cambia estos valores según necesites
                Vector3 desiredScale = new Vector3(0.1777365f, 0.6312358f, 0.4208236f); // Cambia estos valores según necesites

                missionIcon.transform.localPosition = desiredPosition; // Ajustar la posición relativa
                missionIcon.transform.localScale = desiredScale; // Ajustar la escala
                Debug.Log($"Icono de misión asignado a {selectedNPC.name} dentro de Bubble.");
            }
            else
            {
                Debug.LogError($"El NPC {selectedNPC.name} no tiene un componente 'Bubble'.");
            }

            selectedNPC.missionIcon = missionIcon;
        }

        Debug.Log("Finalizada la asignación de misiones.");
    }

    // Método para obtener una misión aleatoria (puede repetirse)
    private MissionData GetRandomMission(List<MissionData> missions)
    {
        int randomIndex = Random.Range(0, missions.Count);
        return missions[randomIndex]; // Permitir duplicados
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
            else
            {
                allCompleted = false;
                break;
            }
        }

        if (allCompleted)
        {
            Debug.Log("Todas las misiones completadas. Avanzando a la siguiente ronda.");
            currentRound++;
            AssignMissions();
        }
    }
}
