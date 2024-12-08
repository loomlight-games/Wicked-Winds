
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;


public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;

    public MissionData[] availableMissions; // Todas las misiones disponibles
    public List<NPC> allNPCs;
    public int numMissionsToAssign = 10; // N�mero de misiones por ronda

    private MissionIconPool missionIconPool;
    public List<NPC> assignedNPCs = new List<NPC>();
    private int currentRound = 1; // Empezamos con la primera ronda
    public int missionsCompleted = 0;
    public List<NPC> npcsWithCat;
    public List<NPC> npcsWithOwl;


    public void Awake()
    {
        //if there's not an instance, it creates one - SINGLETON
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);

    }

    void Start()
    {
        allNPCs.Clear();
        NPC[] npcs = FindObjectsOfType<NPC>();
        allNPCs.AddRange(npcs);

        if (allNPCs.Count <= 0)
        {
            Debug.LogError("No NPC found to assign any mission.");
            return;
        }

        missionIconPool = MissionIconPoolManager.Instance.GetMissionIconPool();
        assignedNPCs.Clear();
        AssignMissions();
    }

    public void AssignMissions()
    {
        Debug.Log("Iniciando la asignacion de misiones...");

        if (!CanAssignMissions())
            return;

        var missionLists = FilterMissionsByDifficulty();
        int numEasyMissions, numMediumMissions, numHardMissions;
        GetMissionCounts(out numEasyMissions, out numMediumMissions, out numHardMissions);

        Debug.Log($"Numero de misiones a asignar: Facil: {numEasyMissions}, Media: {numMediumMissions}, Dificil: {numHardMissions}");
        AssignMissionsToNPCs(missionLists, numEasyMissions, numMediumMissions, numHardMissions);
        Debug.Log("Finalizada la asignacion de misiones.");
    }

    private bool CanAssignMissions()
    {
        if (allNPCs.Count < numMissionsToAssign)
        {
            Debug.LogError("Not enough NPCs for all the missions to be assigned");
            return false;
        }

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
            Debug.Log($"Analizando mision: {mission.name} con dificultad: {mission.difficulty}");
            if (mission.difficulty == 0)
            {
                missionLists["easy"].Add(mission);
                Debug.Log($"Mision facil añadida: {mission.name}");
            }
            else if (mission.difficulty == 1)
            {
                missionLists["medium"].Add(mission);
                Debug.Log($"Mision media añadida: {mission.name}");
            }
            else if (mission.difficulty == 2)
            {
                missionLists["hard"].Add(mission);
                Debug.Log($"Mision dificil añadida: {mission.name}");
            }
        }

        Debug.Log($"Misiones faciles disponibles: {missionLists["easy"].Count}");
        Debug.Log($"Misiones medias disponibles: {missionLists["medium"].Count}");
        Debug.Log($"Misiones dificiles disponibles: {missionLists["hard"].Count}");

        return missionLists;
    }

    private void GetMissionCounts(out int numEasyMissions, out int numMediumMissions, out int numHardMissions)
    {
        numHardMissions = Mathf.Max(0, Mathf.Min(currentRound, numMissionsToAssign));
        numMediumMissions = Mathf.Max(0, Mathf.Min(currentRound + 1, numMissionsToAssign - numHardMissions));
        numEasyMissions = numMissionsToAssign - numMediumMissions - numHardMissions;

        numEasyMissions = Mathf.Max(numEasyMissions, 0);
        Debug.Log($"Numero calculado de misiones: Facil: {numEasyMissions}, Media: {numMediumMissions}, Dificil: {numHardMissions}");
    }

    private void AssignMissionsToNPCs(Dictionary<string, List<MissionData>> missionLists, int numEasyMissions, int numMediumMissions, int numHardMissions)
    {
        List<NPC> shuffledNPCs = new List<NPC>(allNPCs);
        npcsWithCat = allNPCs.Where(npc => npc.cat != null).ToList(); // Filtrar los NPCs que tienen gato
        npcsWithOwl = allNPCs.Where(npc => npc.owl != null).ToList(); // Filtrar los NPCs que tienen buho

        int assignedCount = 0;

        while (assignedCount < numMissionsToAssign && (shuffledNPCs.Count > 0 || npcsWithCat.Count > 0))
        {
            // Seleccionar una misión
            MissionData mission = SelectMission(missionLists, ref assignedCount, numEasyMissions, numMediumMissions);
            if (mission == null)
            {
                continue;
            }

            NPC selectedNPC = null;

            // Asignar NPC dependiendo del tipo de misión
            if (mission.type == "CatMission") // Si es una misión de tipo Cat
            {
                if (npcsWithCat.Count > 0)
                {
                    selectedNPC = GetRandomNPC(npcsWithCat);
                    npcsWithCat.Remove(selectedNPC); // Remover el NPC con gato de la lista
                }
            }
            else if (mission.type == "OwlMission")
            {

                if (npcsWithOwl.Count > 0)
                {
                    selectedNPC = GetRandomNPC(npcsWithOwl);
                    npcsWithOwl.Remove(selectedNPC); // Remover el NPC con gato de la lista
                }
            }
            else if (mission.type == "PotionMission" || mission.type == "LetterMision") // Si es una misión de tipo Potion o Letter
            {
                if (shuffledNPCs.Count > 0)
                {
                    selectedNPC = GetRandomNPC(shuffledNPCs);
                    shuffledNPCs.Remove(selectedNPC); // Remover el NPC de la lista
                }
            }

            if (selectedNPC != null)
            {
                AssignMissionToNPC(selectedNPC, mission);
                assignedCount++;
            }
        }

        Debug.Log("Todas las misiones han sido asignadas a los NPCs.");
        specialMissionGeneration(assignedNPCs);
    }

    private NPC GetRandomNPC(List<NPC> shuffledNPCs)
    {

        int randomIndex = UnityEngine.Random.Range(0, shuffledNPCs.Count);
        NPC selectedNPC = shuffledNPCs[randomIndex];

        if (selectedNPC.request != null)
        {

            shuffledNPCs.RemoveAt(randomIndex);
            return null;
        }


        return selectedNPC;
    }

    private MissionData SelectMission(Dictionary<string, List<MissionData>> missionLists, ref int assignedCount, int numEasyMissions, int numMediumMissions)
    {

        MissionData mission = null;

        if (assignedCount < numEasyMissions && missionLists["easy"].Count > 0)
        {
            mission = GetRandomMission(missionLists["easy"]);

        }
        else if (assignedCount < numEasyMissions + numMediumMissions && missionLists["medium"].Count > 0)
        {
            mission = GetRandomMission(missionLists["medium"]);
        }
        else if (missionLists["hard"].Count > 0)
        {
            mission = GetRandomMission(missionLists["hard"]);
        }
        else
        {
            Debug.LogWarning("No hay misiones disponibles para asignar.");
        }

        return mission;
    }

    private void AssignMissionToNPC(NPC selectedNPC, MissionData mission)
    {
        MissionIcon missionIcon = MissionIconPoolManager.Instance.GetMissionIconPool().GetIcon();
        if (missionIcon == null)
        {
            Debug.LogError("No se pudo obtener un MissionIcon del pool.");
            return;
        }

        missionIcon.AssignMission(mission, this, selectedNPC);

        SetMissionIconPosition(selectedNPC, missionIcon);
        selectedNPC.request = missionIcon;
        selectedNPC.hasMission = true;

        assignedNPCs.Add(selectedNPC);

    }

    public void specialMissionGeneration(List<NPC> assignedNpcs)
    {
        foreach (NPC npc in assignedNpcs)
        {
            if (npc.missionType == "PotionMission")
            {
                // Generar los ingredientes alrededor del NPC
                GameObject[] spawnedIngredients = MissionObjectiveSpawner.Instance.SpawnIngredients(npc.transform.position, 4);

                // Verifica si los ingredientes han sido generados
                if (spawnedIngredients == null && spawnedIngredients.Length == 0)
                {
                    Debug.LogWarning("PotionMission: No se generaron ingredientes. Verifica la instancia de MissionObjectiveSpawner.");
                }


                // Asignar el NPC y el MissionIcon a los objetos generados
                foreach (GameObject ingredient in spawnedIngredients)
                {
                    Pickable pickable = ingredient.GetComponent<Pickable>();
                    if (pickable != null)
                    {
                        pickable.SetNPC(npc); // Asignar el NPC al objeto recolectable
                        pickable.missionIcon = npc.request;

                    }
                    else
                    {
                        Debug.LogWarning($"Ingrediente {ingredient.name} no tiene componente Pickable.");
                    }


                }


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
                        if (!string.IsNullOrEmpty(Npc.name)) // Check if the name is not null or empty
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
                    npc.request.addresseeName = randomNPC.name;
                    npc.request.addressee = randomNPC;
                    randomNPC.sender = npc;


                }
                else
                {
                    Debug.LogWarning("No other NPCs available to be an addressee.");
                }
            }




            npc.request.AssignMissionText(npc.request.data, this, npc);
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
            Debug.Log($"Icono de mision asignado a {selectedNPC.name} dentro de Bubble.");
        }
        else
        {
            Debug.LogError($"El NPC {selectedNPC.name} no tiene un componente 'Bubble'.");
        }
    }

    private MissionData GetRandomMission(List<MissionData> missions)
    {
        int randomIndex = UnityEngine.Random.Range(0, missions.Count);
        Debug.Log($"Mision aleatoria seleccionada: {missions[randomIndex].name}");
        return missions[randomIndex];
    }

    public void AssignNewMission(int numMissions)
    {
        numMissionsToAssign = numMissions;
        missionsCompleted++;
        currentRound = (missionsCompleted / 5) + 1;
        GameManager.Instance.playState.feedBackText.text = $"Mision completada. Total completadas: {missionsCompleted}. Ronda actual: {currentRound}";
        AssignMissions();
    }
}
