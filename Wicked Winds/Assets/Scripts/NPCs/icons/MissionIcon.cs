using System.Linq;
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



    // Método para asignar una misión a este ícono
    public void AssignMission(MissionData mission, MissionManager manager, NPC npc)
    {
        Debug.Log("Asignando misión...");

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
            Debug.LogError("SpriteRenderer o newSprite es nulo.");
        }
    }

    // Este método es llamado cuando el objeto es tomado del pool
    public void OnObjectSpawn()
    {
        Debug.Log("OnObjectSpawn llamado.");

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
                Debug.LogError("SpriteRenderer o newSprite es nulo en OnObjectSpawn.");
            }

            currentMission.isCompleted = false;
            Debug.Log("Estado de la misión restablecido: isCompleted = false.");
        }
    }

    // Llamar cuando la misión se complete
    public void CompleteMission()
    {
        Debug.Log("Completar misión llamado.");

        if (currentMission != null)
        {
            //aqui como que se completa to
            /*Debug.Log("Marcando misión como completada.");
            currentMission.isCompleted = true;*/

            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && spriteMissionCompleted != null)
            {
                Debug.Log("Cambiando al sprite de misión completada.");
                spriteRenderer.sprite = spriteMissionCompleted;
                currentMission = null;
                assignedNPC.OnObjectReturn();
                
            }
            else
            {
                Debug.LogError("SpriteRenderer o spriteMissionCompleted es nulo.");
            }
        }
    }

   
}
