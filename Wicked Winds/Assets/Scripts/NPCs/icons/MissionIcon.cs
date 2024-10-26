using System.Linq;
using System.Reflection;
using UnityEngine;

public class MissionIcon : MonoBehaviour
{
    public GameObject bubble; // Referencia al GameObject del Bubble
    public Sprite spriteMission;
    public Sprite spriteMissionCompleted;

    public MissionData currentMission; // La misión asignada a este ícono
    private MissionManager missionManager; // Referencia al MissionManager
    private MissionIconPool missionIconPool;
    private NPC assignedNPC; // Añadimos una referencia al NPC
    public MessageGenerator messageGenerator;
    public string addressee = null;

   
    [SerializeField] private string message;

    

    //contador para los objetos recogidos
    public int collectedItemsCount = 0;

    // Método para asignar una misión a este ícono
    public void AssignMission(MissionData mission, MissionManager manager, NPC npc)
    {
        Debug.Log($"Asignando misión {mission.name} al NPC {npc.name}");

        currentMission = mission;
        missionManager = manager;
        assignedNPC = npc; // Asignamos el NPC

        // Obtiene el componente SpriteRenderer del GameObject al que está adjunto este script
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        // Verifica si el componente SpriteRenderer existe
        if (spriteRenderer != null && spriteMission != null)
        {
            Debug.Log("Asignando sprite de misión.");
            spriteRenderer.sprite = spriteMission;
        }
        else
        {
            Debug.LogError("SpriteRenderer o spriteMission es nulo en AssignMission.");
        }
    }

    public void AssignMissionText(MissionData mission, MissionManager manager, NPC npc)
    {
        // Log para la asignación de la misión
        Debug.Log($"Asignando misión: {mission.missionName}");

        // Almacenar la misión actual y el manager
        currentMission = mission;
        missionManager = manager;
        assignedNPC = npc; // Asignamos el NPC

        messageGenerator = new();
        // Generar el mensaje para la misión
        message = messageGenerator.GenerateMessage(currentMission,assignedNPC,assignedNPC.missionIcon);
        // Check if the selected message template contains {NPC_NAME}
         if (message.Contains("{NPC_NAME}"))
         {

                 message = message.Replace("{NPC_NAME}", assignedNPC.missionIcon.addressee);
         }
         

        
        // Log para el mensaje generado
        Debug.Log($"Mensaje generado: {message}");

        // Log para el nombre del NPC asignado para verificación
        if (assignedNPC != null)
        {
            Debug.Log($"NPC asignado: {assignedNPC.name}");
        }
        else
        {
            Debug.LogWarning("¡No se ha asignado ningún NPC!");
        }

       
    }

    // Este método es llamado cuando el objeto es tomado del pool
    public void OnObjectSpawn()
    {
        Debug.Log("OnObjectSpawn llamado para restablecer el ícono de misión.");

        if (currentMission != null)
        {
            // Obtiene el componente SpriteRenderer del GameObject al que está adjunto este script
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && spriteMission != null)
            {
                Debug.Log("Restableciendo el sprite de misión en OnObjectSpawn.");
                spriteRenderer.sprite = spriteMission;
            }
            else
            {
                Debug.LogError("SpriteRenderer o spriteMission es nulo en OnObjectSpawn.");
            }

            currentMission.isCompleted = false;
            Debug.Log("Estado de la misión restablecido: isCompleted = false.");
        }
    }

    // Llamar cuando la misión se complete
    public void CompleteMission()
    {
        Debug.Log("Completar misión llamado para actualizar el estado de la misión.");

        if (currentMission != null)
        {
            // Eliminar el NPC de la lista de assignedNPCs en MissionManager
            Debug.Log($"Eliminando NPC {assignedNPC.name} de la lista de NPCs asignados.");
            missionManager.assignedNPCs.Remove(assignedNPC);
            assignedNPC.OnObjectReturn();

            Debug.Log("Restableciendo valores de currentMission y assignedNPC a null.");
            currentMission = null;
            assignedNPC = null;
            PlayerManager.Instance.hasActiveMission = false;

            // Asigna una nueva misión al completar la actual
            Debug.Log("Asignando nueva misión después de completar la misión actual.");
            missionManager.AssignNewMission(1);
            
            
        }
    }


}
