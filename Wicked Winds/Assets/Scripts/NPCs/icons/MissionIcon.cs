using System.Linq;
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



    // M�todo para asignar una misi�n a este �cono
    public void AssignMission(MissionData mission, MissionManager manager, NPC npc)
    {
        Debug.Log("Asignando misi�n...");

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
            Debug.LogError("SpriteRenderer o newSprite es nulo.");
        }
    }

    // Este m�todo es llamado cuando el objeto es tomado del pool
    public void OnObjectSpawn()
    {
        Debug.Log("OnObjectSpawn llamado.");

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
                Debug.LogError("SpriteRenderer o newSprite es nulo en OnObjectSpawn.");
            }

            currentMission.isCompleted = false;
            Debug.Log("Estado de la misi�n restablecido: isCompleted = false.");
        }
    }

    // Llamar cuando la misi�n se complete
    public void CompleteMission()
    {
        Debug.Log("Completar misi�n llamado.");

        if (currentMission != null)
        {
            //aqui como que se completa to
            /*Debug.Log("Marcando misi�n como completada.");
            currentMission.isCompleted = true;*/

            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && spriteMissionCompleted != null)
            {
                Debug.Log("Cambiando al sprite de misi�n completada.");
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
