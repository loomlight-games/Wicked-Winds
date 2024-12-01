
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
            DontDestroyOnLoad(Instance);
        }
        else
            Destroy(gameObject);

    }

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
        Debug.Log("Iniciando la asignacion de misiones...");

        if (!CanAssignMissions())
            return;

        var missionLists = FilterMissionsByDifficulty();
        int numEasyMissions, numMediumMissions, numHardMissions, numVeryHardMissions;
        GetMissionCounts(out numEasyMissions, out numMediumMissions, out numHardMissions, out numVeryHardMissions);

        Debug.Log($"Numero de misiones a asignar: Facil: {numEasyMissions}, Media: {numMediumMissions}, Dificil: {numHardMissions}");
        AssignMissionsToNPCs(missionLists, numEasyMissions, numMediumMissions, numHardMissions);
        Debug.Log("Finalizada la asignacion de misiones.");
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
            { "hard", new List<MissionData>() },
            { "veryhard", new List<MissionData>() }

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
            else if(mission.difficulty == 3)
            {
                missionLists["veryhard"].Add(mission);
                Debug.Log($"Mision dificil añadida: {mission.name}");
            }
        }

        Debug.Log($"Misiones faciles disponibles: {missionLists["easy"].Count}");
        Debug.Log($"Misiones medias disponibles: {missionLists["medium"].Count}");
        Debug.Log($"Misiones dificiles disponibles: {missionLists["hard"].Count}");

        return missionLists;
    }

    private void GetMissionCounts(
    out int numEasyMissions,
    out int numMediumMissions,
    out int numHardMissions,
    out int numVeryHardMissions)
    {
        // Calcular el número de misiones "Very Hard" basadas en las rondas actuales y totales
        numVeryHardMissions = Mathf.Max(1, Mathf.Min(currentRound / 2, numMissionsToAssign)); // Opcional: Ajusta la lógica según tus necesidades

        // Calcular el número de misiones "Hard"
        numHardMissions = Mathf.Max(1, Mathf.Min(currentRound+1, numMissionsToAssign - numVeryHardMissions));

        // Calcular el número de misiones "Medium"
        numMediumMissions = Mathf.Max(1, Mathf.Min(currentRound + 2, numMissionsToAssign - numHardMissions - numVeryHardMissions));

        // El resto se asigna como misiones "Easy"
        numEasyMissions = numMissionsToAssign - numMediumMissions - numHardMissions - numVeryHardMissions;

        // Asegurarse de que el número de misiones fáciles no sea negativo
        numEasyMissions = Mathf.Max(numEasyMissions, 0);

        // Debug para confirmar la distribución
        Debug.Log($"Numero calculado de misiones: Facil: {numEasyMissions}, Media: {numMediumMissions}, Dificil: {numHardMissions}, Muy Dificil: {numVeryHardMissions}");
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
            if (mission.missionName =="CatMission") // Si es una misión de tipo Cat
            {
                if (npcsWithCat.Count > 0)
                {
                    selectedNPC = GetRandomNPC(npcsWithCat);
                    npcsWithCat.Remove(selectedNPC); // Remover el NPC con gato de la lista
                }
            }
            else if (mission.missionName == "OwlMission") {

                if (npcsWithOwl.Count > 0)
                {
                    selectedNPC = GetRandomNPC(npcsWithOwl);
                    npcsWithOwl.Remove(selectedNPC); // Remover el NPC con gato de la lista
                }
            }
            else if (mission.missionName == "PotionMission" || mission.missionName == "LetterMision") // Si es una misión de tipo Potion o Letter
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

    public void  specialMissionGeneration(List<NPC> assignedNpcs)
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
                        pickable.missionIcon= npc.missionIcon;
       
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
                    npc.missionIcon.addresseeName = randomNPC.npcname;
                    npc.missionIcon.addressee = randomNPC;
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
        numMissionsToAssign= numMissions;
        missionsCompleted++;
        currentRound = (missionsCompleted / 5) + 1;
        GameManager.Instance.playState.feedBackText.text = $"Mision completada. Total completadas: {missionsCompleted}. Ronda actual: {currentRound}";
        AssignMissions(numMissionsToAssign);
    }


}
