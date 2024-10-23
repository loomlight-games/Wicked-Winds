using System.Linq;
using UnityEngine;

public class MissionIcon : MonoBehaviour, IPoolable
{
    public GameObject bubble; // Referencia al GameObject del Bubble
    public Sprite spriteMission;
    public Sprite spriteMissionCompleted;

    public MissionData currentMission; // La misi�n asignada a este �cono
    private MissionManager missionManager; // Referencia al MissionManager
    private NPC assignedNPC; // Referencia al NPC asignado para este icono (nuevo cambio)



    // M�todo para asignar una misi�n a este �cono
    public void AssignMission(MissionData mission, MissionManager manager) // A�adido NPC como par�metro
    {
        currentMission = mission;
        missionManager = manager;
        

        // Obtiene el componente SpriteRenderer del GameObject al que est� adjunto este script
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

    // Este m�todo es llamado cuando el objeto es tomado del pool
    public void OnObjectSpawn()
    {
        // Restablecer el estado de los �conos
        if (currentMission != null)
        {
            // Obtiene el componente SpriteRenderer del GameObject al que est� adjunto este script
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

    // Llamar cuando la misi�n se complete
    public void CompleteMission()
    {
        if (currentMission != null && !currentMission.isCompleted)
        {
            // Marcar la misi�n como completada
            currentMission.isCompleted = true;

            // Cambiar el sprite al de misi�n completada
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && spriteMissionCompleted != null)
            {
                spriteRenderer.sprite = spriteMissionCompleted; // Cambia al sprite de completada
            }
            else
            {
                Debug.LogError("SpriteRenderer o spriteMissionCompleted es nulo.");
            }

            // Actualiza el estado del NPC asignado a este MissionIcon
            if (assignedNPC != null)
            {
                assignedNPC.bubble.SetActive(false); // Desactiva la burbuja del NPC
            }
            else
            {
                Debug.LogError("No se ha asignado un NPC a este MissionIcon.");
            }

            // Verifica si todas las misiones se completaron
            missionManager.CheckMissionCompletion();
        }
    }

    // Este m�todo es llamado cuando el objeto es devuelto al pool
    public void OnObjectReturn()
    {
        currentMission = null;
        if (assignedNPC != null)
        {
            assignedNPC.missionIcon = null; // pone null el MissionIcon en el NPC para que se marque que no tiene misi�n
        }
        assignedNPC = null; // Limpia la referencia al NPC
    }
}
