using System;
using UnityEngine;

public class MissionIcon : MonoBehaviour
{
    public GameObject bubble; 

    public MissionData data; 
    private MissionManager missionManager; 
    public NpcController assignedNPC; 
    public MessageGenerator messageGenerator;
    public string addresseeName = null;
    public NpcController addressee;
    public Guid missionID; 

    [SerializeField] public string message;
    [SerializeField] private string responseMessage;

    //contador para los objetos recogidos
    public int collectedItemsCount = 0;


    public void AssignMission(MissionData mission, MissionManager manager, NpcController npc)
    {
        data = mission;
        missionManager = manager;
        assignedNPC = npc; 
        assignedNPC.missionType = data.missionName;

        missionID = Guid.NewGuid();

        

    }

    public void AssignMissionText(MissionData mission, MissionManager manager, NpcController npc)
    {

        data = mission;
        missionManager = manager;
        assignedNPC = npc; 

        if (missionID == Guid.Empty) missionID = Guid.NewGuid();
        if (npc.npcID == Guid.Empty) npc.npcID = Guid.NewGuid();

        messageGenerator = new();
        var (message, response) = messageGenerator.GenerateMessage(data, assignedNPC, assignedNPC.request);

        // Check if the  message template contains {NPC_NAME}
        if (message.Contains("{NPC_NAME}"))
        {
            message = message.Replace("{NPC_NAME}", assignedNPC.request.addresseeName);
        }

        if (response.Contains("{NPC_NAME}"))
        {
            response = response.Replace("{NPC_NAME}", assignedNPC.request.addresseeName);
        }

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

    public void CompleteMission()
    {
        if (data != null)
        {
            // Creates another mission
            this.addressee = null;
            this.addresseeName = null;
            this.assignedNPC = null;
            MissionIconPoolManager.Instance.GetMissionIconPool().ReleaseIcon(this);
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
