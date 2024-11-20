using System;
using UnityEngine;

public class MissionIcon : MonoBehaviour
{
    public GameObject bubble; // Referencia al GameObject del Bubble
    public Sprite spriteMission;
    public Sprite spriteMissionCompleted;

    public MissionData currentMission; // La misión asignada a este ícono
    private MissionManager missionManager; // Referencia al MissionManager
    private MissionIconPool missionIconPool;
    public NPC assignedNPC; // Añadimos una referencia al NPC
    public MessageGenerator messageGenerator;
    public string addresseeName = null;
    public NPC addressee;
    public Guid missionID; // Identificador único para este ícono



    [SerializeField] public string message;
    [SerializeField] private string responseMessage;



    //contador para los objetos recogidos
    public int collectedItemsCount = 0;

    // Método para asignar una misión a este ícono
    public void AssignMission(MissionData mission, MissionManager manager, NPC npc)
    {

        currentMission = mission;
        missionManager = manager;
        assignedNPC = npc; // Asignamos el NPC
        assignedNPC.missionType = currentMission.missionName;
        // Generar un ID único para esta misión
        missionID = Guid.NewGuid();

        // Obtiene el componente SpriteRenderer del GameObject al que está adjunto este script
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        // Verifica si el componente SpriteRenderer existe
        if (spriteRenderer != null && spriteMission != null)
        {
            spriteRenderer.sprite = spriteMission;
        }
        
    }

    public void AssignMissionText(MissionData mission, MissionManager manager, NPC npc)
    {
        // Log para la asignación de la misión

        // Almacenar la misión actual y el manager
        currentMission = mission;
        missionManager = manager;
        assignedNPC = npc; // Asignamos el NPC

        // Generar IDs únicos para el MissionIcon y NPC si no tienen
        if (missionID == Guid.Empty) missionID = Guid.NewGuid();
        if (npc.npcID == Guid.Empty) npc.npcID = Guid.NewGuid();


        messageGenerator = new();
        // Generar el mensaje para la misión
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



    // Este método es llamado cuando el objeto es tomado del pool
    public void OnObjectSpawn()
    {
        

        if (currentMission != null)
        {
            // Obtiene el componente SpriteRenderer del GameObject al que está adjunto este script
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && spriteMission != null)
            {
               
                spriteRenderer.sprite = spriteMission;
            }
            

            currentMission.isCompleted = false;
            
        }
    }

    // Llamar cuando la misión se complete
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

            // Asigna una nueva misión al completar la actual
            missionManager.AssignNewMission(1);


        }
    }


}
