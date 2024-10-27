
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MissionManager : MonoBehaviour
{
    public MissionData[] availableMissions; // Todas las misiones disponibles
    public List<NPC> allNPCs;
    public int numMissionsToAssign = 3; // Número de misiones por ronda

    private MissionIconPool missionIconPool;
    public List<NPC> assignedNPCs = new List<NPC>();
    private int currentRound = 1; // Empezamos con la primera ronda
    private int missionsCompleted = 0;

    

    void Start()
    {
        Debug.Log("Inicio del Manager de Misiones");

        allNPCs.Clear();
        NPC[] npcs = FindObjectsOfType<NPC>();
        allNPCs.AddRange(npcs);

        Debug.Log($"Total NPCs encontrados: {allNPCs.Count}");
        missionIconPool = MissionIconPoolManager.Instance.GetMissionIconPool();
        assignedNPCs.Clear();
        AssignMissions(numMissionsToAssign);
    }

    public void AssignMissions(int numMissionsToAssign)
    {
        Debug.Log("Iniciando la asignación de misiones...");

        if (!CanAssignMissions())
            return;

        var missionLists = FilterMissionsByDifficulty();
        int numEasyMissions, numMediumMissions, numHardMissions;
        GetMissionCounts(out numEasyMissions, out numMediumMissions, out numHardMissions);

        Debug.Log($"Número de misiones a asignar: Fácil: {numEasyMissions}, Media: {numMediumMissions}, Difícil: {numHardMissions}");
        AssignMissionsToNPCs(missionLists, numEasyMissions, numMediumMissions, numHardMissions);
        Debug.Log("Finalizada la asignación de misiones.");
    }

    private bool CanAssignMissions()
    {
        if (allNPCs.Count < numMissionsToAssign)
        {
            Debug.LogError("No hay suficientes NPCs para asignar misiones.");
            return false;
        }

        Debug.Log($"Total de NPCs disponibles: {allNPCs.Count}");
        return true;
    }

    private Dictionary<string, List<MissionData>> FilterMissionsByDifficulty()
    {
        Debug.Log("Filtrando misiones por dificultad...");

        var missionLists = new Dictionary<string, List<MissionData>>()
        {
            { "easy", new List<MissionData>() },
            { "medium", new List<MissionData>() },
            { "hard", new List<MissionData>() }
        };

        foreach (MissionData mission in availableMissions)
        {
            Debug.Log($"Analizando misión: {mission.name} con dificultad: {mission.difficulty}");
            if (mission.difficulty == 0)
            {
                missionLists["easy"].Add(mission);
                Debug.Log($"Misión fácil añadida: {mission.name}");
            }
            else if (mission.difficulty == 1)
            {
                missionLists["medium"].Add(mission);
                Debug.Log($"Misión media añadida: {mission.name}");
            }
            else if (mission.difficulty == 2)
            {
                missionLists["hard"].Add(mission);
                Debug.Log($"Misión difícil añadida: {mission.name}");
            }
        }

        Debug.Log($"Misiones fáciles disponibles: {missionLists["easy"].Count}");
        Debug.Log($"Misiones medias disponibles: {missionLists["medium"].Count}");
        Debug.Log($"Misiones difíciles disponibles: {missionLists["hard"].Count}");

        return missionLists;
    }

    private void GetMissionCounts(out int numEasyMissions, out int numMediumMissions, out int numHardMissions)
    {
        Debug.Log("Calculando el número de misiones por dificultad...");
        numHardMissions = Mathf.Max(0, Mathf.Min(currentRound, numMissionsToAssign));
        numMediumMissions = Mathf.Max(0, Mathf.Min(currentRound - 1, numMissionsToAssign - numHardMissions));
        numEasyMissions = numMissionsToAssign - numMediumMissions - numHardMissions;

        numEasyMissions = Mathf.Max(numEasyMissions, 0);
        Debug.Log($"Número calculado de misiones: Fácil: {numEasyMissions}, Media: {numMediumMissions}, Difícil: {numHardMissions}");
    }

    private void AssignMissionsToNPCs(Dictionary<string, List<MissionData>> missionLists, int numEasyMissions, int numMediumMissions, int numHardMissions)
    {
        Debug.Log("Asignando misiones a NPCs...");
        List<NPC> shuffledNPCs = new List<NPC>(allNPCs);
        Debug.Log($"Cantidad de NPCs mezclados: {shuffledNPCs.Count}");

        int assignedCount = 0;

        while (assignedCount < numMissionsToAssign && shuffledNPCs.Count > 0)
        {
            NPC selectedNPC = GetRandomNPC(shuffledNPCs);
            if (selectedNPC == null)
            {
                Debug.Log("No se pudo seleccionar un NPC adecuado, continuando con la siguiente iteración...");
                continue;
            }

            Debug.Log($"NPC seleccionado para asignación: {selectedNPC.name}");
            MissionData mission = SelectMission(missionLists, ref assignedCount, numEasyMissions, numMediumMissions);
            if (mission == null)
            {
                Debug.Log("No se encontró una misión adecuada para asignar al NPC.");
                continue;
            }

            AssignMissionToNPC(selectedNPC, mission);
            assignedCount++;
            Debug.Log($"Misión asignada. Total de misiones asignadas: {assignedCount}/{numMissionsToAssign}");
            shuffledNPCs.Remove(selectedNPC);
        }

        Debug.Log("Todas las misiones han sido asignadas a los NPCs.");
        specialMissionGeneration(assignedNPCs);
    }

    private NPC GetRandomNPC(List<NPC> shuffledNPCs)
    {
        Debug.Log("Seleccionando NPC aleatorio...");
        int randomIndex = UnityEngine.Random.Range(0, shuffledNPCs.Count);
        NPC selectedNPC = shuffledNPCs[randomIndex];

        if (selectedNPC.missionIcon != null)
        {
            Debug.Log($"El NPC {selectedNPC.name} ya tiene una misión. Eliminando de la lista.");
            shuffledNPCs.RemoveAt(randomIndex);
            return null;
        }

        Debug.Log($"NPC aleatorio seleccionado: {selectedNPC.name}");
        return selectedNPC;
    }

    private MissionData SelectMission(Dictionary<string, List<MissionData>> missionLists, ref int assignedCount, int numEasyMissions, int numMediumMissions)
    {
        Debug.Log("Seleccionando misión...");
        MissionData mission = null;

        if (assignedCount < numEasyMissions && missionLists["easy"].Count > 0)
        {
            mission = GetRandomMission(missionLists["easy"]);
            Debug.Log($"Misión fácil seleccionada: {mission.name}");
        }
        else if (assignedCount < numEasyMissions + numMediumMissions && missionLists["medium"].Count > 0)
        {
            mission = GetRandomMission(missionLists["medium"]);
            Debug.Log($"Misión media seleccionada: {mission.name}");
        }
        else if (missionLists["hard"].Count > 0)
        {
            mission = GetRandomMission(missionLists["hard"]);
            Debug.Log($"Misión difícil seleccionada: {mission.name}");
        }
        else
        {
            Debug.LogWarning("No hay misiones disponibles para asignar.");
        }

        return mission;
    }

    private void AssignMissionToNPC(NPC selectedNPC, MissionData mission)
    {
        Debug.Log($"Asignando misión {mission.name} a NPC: {selectedNPC.name}");
        MissionIcon missionIcon = MissionIconPoolManager.Instance.GetMissionIconPool().GetIcon();
        if (missionIcon == null)
        {
            Debug.LogError("No se pudo obtener un MissionIcon del pool.");
            return;
        }

        missionIcon.AssignMission(mission, this, selectedNPC);
        
        SetMissionIconPosition(selectedNPC, missionIcon);
        selectedNPC.missionIcon = missionIcon;
        selectedNPC.hasMission = true;

        assignedNPCs.Add(selectedNPC);
        Debug.Log($"Misión {mission.name} asignada correctamente a {selectedNPC.name}");
        // Lógica de asignación de misión

      
   
    }

    public void  specialMissionGeneration(List<NPC> assignedNpcs)
    {
        foreach (NPC npc in assignedNpcs)
        {
            if (npc.missionType == "PotionMission")
            {
                Debug.Log($"Asignando misión 'PotionMission' al NPC {npc.npcname}");

                // Generar los ingredientes alrededor del NPC
                GameObject[] spawnedIngredients = MissionObjectiveSpawner.Instance.SpawnIngredients(npc.transform.position, 3);

                // Verifica si los ingredientes han sido generados
                if (spawnedIngredients != null && spawnedIngredients.Length > 0)
                {
                    Debug.Log($"PotionMission: Generados {spawnedIngredients.Length} ingredientes alrededor del NPC {npc.npcname}");
                }
                else
                {
                    Debug.LogWarning("PotionMission: No se generaron ingredientes. Verifica la instancia de MissionObjectiveSpawner.");
                }

                // Asignar el NPC y el MissionIcon a los objetos generados
                foreach (GameObject ingredient in spawnedIngredients)
                {
                    

                    Debug.Log($"Asignando propiedades a ingrediente: {ingredient.name}");

                    Pickable pickable = ingredient.GetComponent<Pickable>();
                    if (pickable != null)
                    {
                        pickable.SetNPC(npc); // Asignar el NPC al objeto recolectable
                        pickable.missionIcon= npc.missionIcon;
                        Debug.Log($"Ingrediente {ingredient.name} asignado al NPC {npc.npcname} como Pickable.");
                    }
                    else
                    {
                        Debug.LogWarning($"Ingrediente {ingredient.name} no tiene componente Pickable.");
                    }

                    Interactable interactable = ingredient.GetComponent<Interactable>();
                    if (interactable != null)
                    {
                        interactable.missionIcon = npc.missionIcon; // Asignar el MissionIcon al objeto interactuable
                        Debug.Log($"Ingrediente {ingredient.name} asignado a MissionIcon {npc.missionIcon.name} como Interactable.");
                    }
                    else
                    {
                        Debug.LogWarning($"Ingrediente {ingredient.name} no tiene componente Interactable.");
                    }
                }

                Debug.Log("PotionMission: Asignación completa de 3 ingredientes con NPC y MissionIcon.");
            }

            if (npc.missionType == "LetterMision")
            {
                // Get a list of all NPCs in the scene
                NPC[] allNPCs = GetAllNPCs();

                // Create a list to hold NPC names, excluding the current NPC and those with a mission icon
                List<NPC> npcNames = new List<NPC>();

                foreach (var Npc in allNPCs)
                {
                    // Exclude the current NPC and those with a mission icon
                    if (!assignedNPCs.Contains(Npc) && Npc != npc)
                    {
                        Debug.Log($"Adding NPC: {Npc}");
                        if (!string.IsNullOrEmpty(Npc.npcname)) // Check if the name is not null or empty
                        {
                            npcNames.Add(Npc); // Add the name to the list
                        }
                        else
                        {
                            Debug.LogWarning("Found NPC with empty name.");
                        }
                    }
                }

                // Select a random NPC name from the list, if available
                if (npcNames.Count > 0)
                {
                    NPC randomNPC = npcNames[Random.Range(0, npcNames.Count)];
                    npc.missionIcon.addressee = randomNPC.npcname;
                    randomNPC.sender = npc;

                }
                else
                {
                    Debug.LogWarning("No other NPCs available to be an addressee.");
                }
            }

            npc.missionIcon.AssignMissionText(npc.missionIcon.currentMission, this, npc);
        }
        
    }

    public static NPC[] GetAllNPCs()
    {
        return GameObject.FindObjectsOfType<NPC>(); 
    }


    private void SetMissionIconPosition(NPC selectedNPC, MissionIcon missionIcon)
    {
        Transform bubbleTransform = selectedNPC.transform.Find("Bubble");
        if (bubbleTransform != null)
        {
            missionIcon.transform.SetParent(bubbleTransform, false);
            Vector3 desiredPosition = new Vector3(1.3051f, 0.9899999f, 0f);
            Vector3 desiredScale = new Vector3(0.1777365f, 0.6312358f, 0.4208236f);

            missionIcon.transform.localPosition = desiredPosition;
            missionIcon.transform.localScale = desiredScale;
            Debug.Log($"Icono de misión asignado a {selectedNPC.name} dentro de Bubble.");
        }
        else
        {
            Debug.LogError($"El NPC {selectedNPC.name} no tiene un componente 'Bubble'.");
        }
    }

    private MissionData GetRandomMission(List<MissionData> missions)
    {
        int randomIndex = UnityEngine.Random.Range(0, missions.Count);
        Debug.Log($"Misión aleatoria seleccionada: {missions[randomIndex].name}");
        return missions[randomIndex];
    }

    public void AssignNewMission(int numMissions)
    {
        numMissionsToAssign= numMissions;
        missionsCompleted++;
        currentRound = (missionsCompleted / 5) + 1;

        Debug.Log($"Misión completada. Total completadas: {missionsCompleted}. Ronda actual: {currentRound}");
        AssignMissions(numMissionsToAssign);
    }
}
