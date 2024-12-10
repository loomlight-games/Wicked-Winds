using System;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class MissionIcon : MonoBehaviour
{
    public GameObject bubble; // Referencia al GameObject del Bubble

    public MissionData data; // La mision asignada a este icono
    private MissionManager missionManager; // Referencia al MissionManager
    private MissionIconPool missionIconPool;
    public NPC assignedNPC; // Añadimos una referencia al NPC
    public MessageGenerator messageGenerator;
    public string addresseeName = null;
    public NPC addressee;
    public Guid missionID; // Identificador Unico para este icono

    [SerializeField] public string message;
    [SerializeField] private string responseMessage;

    //contador para los objetos recogidos
    public int collectedItemsCount = 0;

    // Metodo para asignar una mision a este icono
    public void AssignMission(MissionData mission, MissionManager manager, NPC npc)
    {
        data = mission;
        missionManager = manager;
        assignedNPC = npc; // Asignamos el NPC
        assignedNPC.missionType = data.missionName;

        // Generar un ID unico para esta mision
        this.missionID = Guid.NewGuid();

      
    }

    public void AssignMissionText(MissionData mission, MissionManager manager, NPC npc)
    {

        // Almacenar la mision actual y el manager
        data = mission;
        missionManager = manager;
        assignedNPC = npc; // Asignamos el NPC

        // Generar IDs unicos para el MissionIcon y NPC si no tienen
        if (missionID == Guid.Empty) missionID = Guid.NewGuid();
        if (npc.npcID == Guid.Empty) npc.npcID = Guid.NewGuid();

        messageGenerator = new();
        // Generar el mensaje para la misi�n
        var (message, response) = messageGenerator.GenerateMessage(data, assignedNPC, assignedNPC.request);

        // Check if the selected message template contains {NPC_NAME}
        if (message.Contains("{NPC_NAME}"))
        {
            message = message.Replace("{NPC_NAME}", assignedNPC.request.addresseeName);
        }

        if (response.Contains("{NPC_NAME}"))
        {
            response = response.Replace("{NPC_NAME}", assignedNPC.request.addresseeName);
        }

        // Luego puedes asignar el mensaje y la respuesta a las propiedades de NPC
        assignedNPC.message = message;
        if (mission.missionName == "LetterMision")
        {
            assignedNPC.responseMessage = null;
            addressee.responseMessage = response;
        }
        else
        {
            assignedNPC.responseMessage = response;
        }
    }

    // Este metodo es llamado cuando el objeto es tomado del pool
    public void OnObjectSpawn()
    {
        if (data != null)
        {
            
        }
    }

    public void CompleteMission()
    {
        if (data != null)
        {
            // Creates another mission
            missionManager.AssignNewMission(1);

            // Adds time and makes animation
            GameManager.Instance.AddTime(data.timeBonus);

            GameManager.Instance.missionsTimes.Add(GameManager.Instance.missionTime);
            GameManager.Instance.missionTime = 0f;

            // Removes NPC from the list of assigned from Mission Manager
            missionManager.assignedNPCs.Remove(assignedNPC);
            assignedNPC.OnObjectReturn();

            data = null;
            assignedNPC = null;

            PlayerManager.Instance.hasActiveMission = false;
            PlayerManager.Instance.activeMission = null;
        }
    }
}
