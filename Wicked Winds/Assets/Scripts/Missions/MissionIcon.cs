using System;
using UnityEngine;

public class MissionIcon : MonoBehaviour
{
    public GameObject bubble; // Referencia al GameObject del Bubble
    public Sprite spriteMission;
    public Sprite spriteMissionCompleted;

    public MissionData data; // La misi�n asignada a este �cono
    private MissionManager missionManager; // Referencia al MissionManager
    private MissionIconPool missionIconPool;
    public NpcController assignedNPC; // A�adimos una referencia al NPC
    public MessageGenerator messageGenerator;
    public string addresseeName = null;
    public NpcController addressee;
    public Guid missionID; // Identificador �nico para este �cono

    [SerializeField] public string message;
    [SerializeField] private string responseMessage;

    //contador para los objetos recogidos
    public int collectedItemsCount = 0;

    // M�todo para asignar una misi�n a este �cono
    public void AssignMission(MissionData mission, MissionManager manager, NpcController npc)
    {
        data = mission;
        missionManager = manager;
        assignedNPC = npc; // Asignamos el NPC
        assignedNPC.missionType = data.missionName;

        // Generar un ID �nico para esta misi�n
        missionID = Guid.NewGuid();

        // Obtiene el componente SpriteRenderer del GameObject al que est� adjunto este script
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        // Verifica si el componente SpriteRenderer existe
        if (spriteRenderer != null && spriteMission != null)
        {
            spriteRenderer.sprite = spriteMission;
        }
    }

    public void AssignMissionText(MissionData mission, MissionManager manager, NpcController npc)
    {

        // Almacenar la misi�n actual y el manager
        data = mission;
        missionManager = manager;
        assignedNPC = npc; // Asignamos el NPC

        // Generar IDs �nicos para el MissionIcon y NPC si no tienen
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

    // Este m�todo es llamado cuando el objeto es tomado del pool
    public void OnObjectSpawn()
    {
        if (data != null)
        {
            // Obtiene el componente SpriteRenderer del GameObject al que est� adjunto este script
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && spriteMission != null)
            {
                spriteRenderer.sprite = spriteMission;
            }

            data.isCompleted = false;
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
