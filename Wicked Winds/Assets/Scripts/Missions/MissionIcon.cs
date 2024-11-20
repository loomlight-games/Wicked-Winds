using System;
using UnityEngine;

public class MissionIcon : MonoBehaviour
{
    public GameObject bubble; // Referencia al GameObject del Bubble
    public Sprite spriteMission;
    public Sprite spriteMissionCompleted;

    public MissionData currentMission; // La misi�n asignada a este �cono
    private MissionManager missionManager; // Referencia al MissionManager
    private MissionIconPool missionIconPool;
    public NPC assignedNPC; // A�adimos una referencia al NPC
    public MessageGenerator messageGenerator;
    public string addresseeName = null;
    public NPC addressee;
    public Guid missionID; // Identificador �nico para este �cono



    [SerializeField] public string message;
    [SerializeField] private string responseMessage;



    //contador para los objetos recogidos
    public int collectedItemsCount = 0;

    // M�todo para asignar una misi�n a este �cono
    public void AssignMission(MissionData mission, MissionManager manager, NPC npc)
    {

        currentMission = mission;
        missionManager = manager;
        assignedNPC = npc; // Asignamos el NPC
        assignedNPC.missionType = currentMission.missionName;
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

    public void AssignMissionText(MissionData mission, MissionManager manager, NPC npc)
    {
        // Log para la asignaci�n de la misi�n

        // Almacenar la misi�n actual y el manager
        currentMission = mission;
        missionManager = manager;
        assignedNPC = npc; // Asignamos el NPC

        // Generar IDs �nicos para el MissionIcon y NPC si no tienen
        if (missionID == Guid.Empty) missionID = Guid.NewGuid();
        if (npc.npcID == Guid.Empty) npc.npcID = Guid.NewGuid();


        messageGenerator = new();
        // Generar el mensaje para la misi�n
        var (message, response) = messageGenerator.GenerateMessage(currentMission, assignedNPC, assignedNPC.missionIcon);


        // Check if the selected message template contains {NPC_NAME}
        if (message.Contains("{NPC_NAME}"))
        {

            message = message.Replace("{NPC_NAME}", assignedNPC.missionIcon.addresseeName);
        }

        if (response.Contains("{NPC_NAME}"))
        {

            response = response.Replace("{NPC_NAME}", assignedNPC.missionIcon.addresseeName);
        }




        // Log para el mensaje generado
        
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
        

        if (currentMission != null)
        {
            // Obtiene el componente SpriteRenderer del GameObject al que est� adjunto este script
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && spriteMission != null)
            {
               
                spriteRenderer.sprite = spriteMission;
            }
            

            currentMission.isCompleted = false;
            
        }
    }

    // Llamar cuando la misi�n se complete
    public void CompleteMission()
    {

        if (currentMission != null)
        {
            GameManager.Instance.remainingTime += currentMission.timeBonus;
            GameManager.Instance.missionsTimes.Add(GameManager.Instance.missionTime);
            GameManager.Instance.missionTime = 0f;
            // Eliminar el NPC de la lista de assignedNPCs en MissionManager
            missionManager.assignedNPCs.Remove(assignedNPC);
            assignedNPC.OnObjectReturn();

            currentMission = null;
            assignedNPC = null;
            PlayerManager.Instance.hasActiveMission = false;
            PlayerManager.Instance.activeMission = null;

            // Asigna una nueva misi�n al completar la actual
            missionManager.AssignNewMission(1);


        }
    }


}
