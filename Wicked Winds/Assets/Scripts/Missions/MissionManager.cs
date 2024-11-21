
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public MissionData[] availableMissions; // Todas las misiones disponibles
    public List<NPC> allNPCs;
    public int numMissionsToAssign = 10; // N�mero de misiones por ronda

    private MissionIconPool missionIconPool;
    public List<NPC> assignedNPCs = new List<NPC>();
    private int currentRound = 1; // Empezamos con la primera ronda
    public int missionsCompleted = 0;





    void Start()
    {

        allNPCs.Clear();
        NPC[] npcs = FindObjectsOfType<NPC>();
        allNPCs.AddRange(npcs);

        missionIconPool = MissionIconPoolManager.Instance.GetMissionIconPool();
        assignedNPCs.Clear();
        AssignMissions(numMissionsToAssign);
    }

    public void AssignMissions(int numMissionsToAssign)
    {

        if (!CanAssignMissions())
            return;

        var missionLists = FilterMissionsByDifficulty();
        int numEasyMissions, numMediumMissions, numHardMissions;
        GetMissionCounts(out numEasyMissions, out numMediumMissions, out numHardMissions);

        AssignMissionsToNPCs(missionLists, numEasyMissions, numMediumMissions, numHardMissions);
    }

    private bool CanAssignMissions()
    {
        if (allNPCs.Count < numMissionsToAssign)
        {
            Debug.LogError("No hay suficientes NPCs para asignar misiones.");
            return false;
        }

        return true;
    }

    private Dictionary<string, List<MissionData>> FilterMissionsByDifficulty()
    {

        var missionLists = new Dictionary<string, List<MissionData>>()
        {
            { "easy", new List<MissionData>() },
            { "medium", new List<MissionData>() },
            { "hard", new List<MissionData>() }
        };

        foreach (MissionData mission in availableMissions)
        {
            if (mission.difficulty == 0)
            {
                missionLists["easy"].Add(mission);
            }
            else if (mission.difficulty == 1)
            {
                missionLists["medium"].Add(mission);
            }
            else if (mission.difficulty == 2)
            {
                missionLists["hard"].Add(mission);
            }
        }

        

        return missionLists;
    }

    private void GetMissionCounts(out int numEasyMissions, out int numMediumMissions, out int numHardMissions)
    {
        /*numHardMissions = Mathf.Max(0, Mathf.Min(currentRound, numMissionsToAssign));
        numMediumMissions = Mathf.Max(0, Mathf.Min(currentRound + 1, numMissionsToAssign - numHardMissions));
        numEasyMissions = numMissionsToAssign - numMediumMissions - numHardMissions;

        numEasyMissions = Mathf.Max(numEasyMissions, 0);*/

        numEasyMissions = 0;
        numMediumMissions = 0;
        numHardMissions = numMissionsToAssign;
    }

    private void AssignMissionsToNPCs(Dictionary<string, List<MissionData>> missionLists, int numEasyMissions, int numMediumMissions, int numHardMissions)
    {
        List<NPC> shuffledNPCs = new List<NPC>(allNPCs);
        List<NPC> npcsWithCat = allNPCs.Where(npc => npc.cat != null).ToList(); // Filtrar los NPCs que tienen gato

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
            if (mission.missionName == "CatMission") // Si es una misión de tipo Cat
            {
                if (npcsWithCat.Count > 0)
                {
                    selectedNPC = GetRandomNPC(npcsWithCat);
                    npcsWithCat.Remove(selectedNPC); // Remover el NPC con gato de la lista
                }
            }
            else // Si es una misión de tipo Potion o Letter
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

        specialMissionGeneration(assignedNPCs);
    }

    private NPC GetRandomNPC(List<NPC> shuffledNPCs)
    {

        int randomIndex = UnityEngine.Random.Range(0, shuffledNPCs.Count);
        NPC selectedNPC = shuffledNPCs[randomIndex];

        if (selectedNPC.missionIcon != null)
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
        selectedNPC.missionIcon = missionIcon;
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
                List<GameObject> spawnedIngredients = new List<GameObject>(MissionObjectiveSpawner.Instance.SpawnIngredients(npc.transform.position, 4));

                // Verifica si los ingredientes han sido generados
                if (spawnedIngredients == null || spawnedIngredients.Count == 0)
                {
                    Debug.LogWarning("PotionMission: No se generaron ingredientes. Verifica la instancia de MissionObjectiveSpawner.");
                    continue;

                }
                else
                {
                    // Asignar el NPC y el MissionIcon a los objetos generados
                    foreach (GameObject ingredient in spawnedIngredients)
                    {
                        Pickable pickable = ingredient.GetComponent<Pickable>();
                        if (pickable != null)
                        {
                            pickable.SetNPC(npc); // Asignar el NPC al objeto recolectable
                            pickable.missionIcon = npc.missionIcon;

                            continue;

                        }
                       


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
                        if (!string.IsNullOrEmpty(Npc.npcname)) // Check if the name is not null or empty
                        {
                            npcNames.Add(Npc); // Add the name to the list
                        }
                       
                    }
                }

                // Select a random NPC name from the list, if available
                if (npcNames.Count > 0)
                {
                    NPC randomNPC = npcNames[UnityEngine.Random.Range(0, npcNames.Count)];
                    npc.missionIcon.addresseeName = randomNPC.npcname;
                    npc.missionIcon.addressee = randomNPC;
                    randomNPC.sender = npc;


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
        }
        
    }

    private MissionData GetRandomMission(List<MissionData> missions)
    {
        int randomIndex = UnityEngine.Random.Range(0, missions.Count);
        return missions[randomIndex];
    }

    public void AssignNewMission(int numMissions)
    {
        numMissionsToAssign = numMissions;
        missionsCompleted++;
        currentRound = (missionsCompleted / 5) + 1;
        GameManager.Instance.playState.feedBackText.text = $"Mision completada. Total completadas: {missionsCompleted}. Ronda actual: {currentRound}";
        AssignMissions(numMissionsToAssign);
    }


}
