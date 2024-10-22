using System.Linq;
using UnityEngine;

public class MissionIcon : MonoBehaviour, IPoolable
{
    public GameObject bubble; // Referencia al GameObject del Bubble
    public Sprite spriteMission;
    public Sprite spriteMissionCompleted;

    public MissionData currentMission; // La misión asignada a este ícono
    private MissionManager missionManager; // Referencia al MissionManager
    
    

    // Método para asignar una misión a este ícono
    public void AssignMission(MissionData mission, MissionManager manager)
    {
        currentMission = mission;
        missionManager = manager;

        // Obtiene el componente SpriteRenderer del GameObject al que está adjunto este script
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        // Verifica si el componente SpriteRenderer existe
        if (spriteRenderer != null && spriteMission != null)
        {
            // Asigna el nuevo sprite al SpriteRenderer
            spriteRenderer.sprite = spriteMission;
        }
        else
        {
            Debug.LogError("SpriteRenderer o newSprite es nulo.");
        }
    }

    // Este método es llamado cuando el objeto es tomado del pool
    public void OnObjectSpawn()
    {
        // Restablecer el estado de los íconos
        if (currentMission != null)
        {
            // Obtiene el componente SpriteRenderer del GameObject al que está adjunto este script
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && spriteMission != null)
            {
                // Asigna el nuevo sprite al SpriteRenderer
                spriteRenderer.sprite = spriteMission;
            }
            else
            {
                Debug.LogError("SpriteRenderer o newSprite es nulo.");
            }

            currentMission.isCompleted = false;
        }
        
    }

    // Llamar cuando la misión se complete
    public void CompleteMission()
    {
        if (currentMission != null && !currentMission.isCompleted)
        {
            // Marcar la misión como completada
            currentMission.isCompleted = true;

            // Cambiar el sprite al de misión completada
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && spriteMissionCompleted != null)
            {
                spriteRenderer.sprite = spriteMissionCompleted; // Cambia al sprite de completada
            }
            else
            {
                Debug.LogError("SpriteRenderer o spriteMissionCompleted es nulo.");
            }

            // Actualiza el estado del NPC que tiene este MissionIcon asignado
            if (missionManager != null)
            {
                NPC assignedNPC = missionManager.assignedNPCs.Find(npc => npc.missionIcon == this);
                if (assignedNPC != null)
                {
                    assignedNPC.hasMission = false; // Actualiza el estado del NPC al completar la misión
                    assignedNPC.bubble.SetActive(false); // Desactiva la burbuja del NPC
                }
            }

            // Verifica si todas las misiones se completaron
            missionManager.CheckMissionCompletion();
        }
    }



    // Este método es llamado cuando el objeto es devuelto al pool
    public void OnObjectReturn()
    {
        currentMission = null;

        // Obtiene el componente SpriteRenderer del GameObject al que está adjunto este script
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = null;

        // Buscar el NPC que tiene este MissionIcon asignado
        NPC assignedNPC = missionManager.assignedNPCs.Find(npc => npc.missionIcon == this);
        if (assignedNPC != null)
        {
            assignedNPC.missionIcon = null; // Limpiar la referencia al MissionIcon
            assignedNPC.hasMission = false; // Actualizar el estado del NPC
        }
    }

}
