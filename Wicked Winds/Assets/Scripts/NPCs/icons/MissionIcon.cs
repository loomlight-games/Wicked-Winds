using System.Linq;
using System.Reflection;
using UnityEngine;

public class MissionIcon : MonoBehaviour
{
    public GameObject bubble; // Referencia al GameObject del Bubble
    public Sprite spriteMission;
    public Sprite spriteMissionCompleted;

    public MissionData currentMission; // La misi�n asignada a este �cono
    private MissionManager missionManager; // Referencia al MissionManager
    private MissionIconPool missionIconPool;
    private NPC assignedNPC; // A�adimos una referencia al NPC
    public MessageGenerator messageGenerator;
    public string addressee = null;

   
    [SerializeField] private string message;

    

    //contador para los objetos recogidos
    public int collectedItemsCount = 0;

    // M�todo para asignar una misi�n a este �cono
    public void AssignMission(MissionData mission, MissionManager manager, NPC npc)
    {
        Debug.Log($"Asignando misi�n {mission.name} al NPC {npc.name}");

        currentMission = mission;
        missionManager = manager;
        assignedNPC = npc; // Asignamos el NPC

        // Obtiene el componente SpriteRenderer del GameObject al que est� adjunto este script
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        // Verifica si el componente SpriteRenderer existe
        if (spriteRenderer != null && spriteMission != null)
        {
            Debug.Log("Asignando sprite de misi�n.");
            spriteRenderer.sprite = spriteMission;
        }
        else
        {
            Debug.LogError("SpriteRenderer o spriteMission es nulo en AssignMission.");
        }
    }

    public void AssignMissionText(MissionData mission, MissionManager manager, NPC npc)
    {
        // Log para la asignaci�n de la misi�n
        Debug.Log($"Asignando misi�n: {mission.missionName}");

        // Almacenar la misi�n actual y el manager
        currentMission = mission;
        missionManager = manager;
        assignedNPC = npc; // Asignamos el NPC

        messageGenerator = new();
        // Generar el mensaje para la misi�n
        message = messageGenerator.GenerateMessage(currentMission,assignedNPC,assignedNPC.missionIcon);
        // Check if the selected message template contains {NPC_NAME}
         if (message.Contains("{NPC_NAME}"))
         {

                 message = message.Replace("{NPC_NAME}", assignedNPC.missionIcon.addressee);
         }
         

        
        // Log para el mensaje generado
        Debug.Log($"Mensaje generado: {message}");

        // Log para el nombre del NPC asignado para verificaci�n
        if (assignedNPC != null)
        {
            Debug.Log($"NPC asignado: {assignedNPC.name}");
        }
        else
        {
            Debug.LogWarning("�No se ha asignado ning�n NPC!");
        }

       
    }

    // Este m�todo es llamado cuando el objeto es tomado del pool
    public void OnObjectSpawn()
    {
        Debug.Log("OnObjectSpawn llamado para restablecer el �cono de misi�n.");

        if (currentMission != null)
        {
            // Obtiene el componente SpriteRenderer del GameObject al que est� adjunto este script
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && spriteMission != null)
            {
                Debug.Log("Restableciendo el sprite de misi�n en OnObjectSpawn.");
                spriteRenderer.sprite = spriteMission;
            }
            else
            {
                Debug.LogError("SpriteRenderer o spriteMission es nulo en OnObjectSpawn.");
            }

            currentMission.isCompleted = false;
            Debug.Log("Estado de la misi�n restablecido: isCompleted = false.");
        }
    }

    // Llamar cuando la misi�n se complete
    public void CompleteMission()
    {
        Debug.Log("Completar misi�n llamado para actualizar el estado de la misi�n.");

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

            // Asigna una nueva misi�n al completar la actual
            Debug.Log("Asignando nueva misi�n despu�s de completar la misi�n actual.");
            missionManager.AssignNewMission(1);
            
            
        }
    }


}
