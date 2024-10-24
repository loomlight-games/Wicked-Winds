using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icon : MonoBehaviour, IPoolableObject
{

    public GameObject bubble; // Referencia al GameObject del Bubble
    public Sprite spriteMission;
    public Sprite spriteMissionCompleted;

    public MissionData currentMission; // La misi�n asignada a este �cono
    private MissionManager missionManager; // Referencia al MissionManager
    private NPC assignedNPC; // Referencia al NPC asignado para este icono (nuevo cambio)
    private MissionIconPool missionIconPool;



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
        }
    }

    // Este m�todo es llamado cuando el objeto es devuelto al pool
    public void OnObjectReturn()
    {
        Debug.Log($"se quiere devolver el missionIcon al `pool de : {assignedNPC}");
        // Si hay un NPC asignado, limpia su �cono de misi�n
        if (assignedNPC != null)
        {
            Debug.Log($"El mision icon q se quiere devolver es : {assignedNPC.missionIcon}");
            // Verifica que el NPC tenga un �cono de misi�n antes de limpiarlo
            if (assignedNPC.missionIcon != null)
            {
                // Devuelve el �cono de misi�n al pool
                missionIconPool.ReleaseIcon(assignedNPC.missionIcon);
                assignedNPC.missionIcon = null; // Limpia la referencia del �cono de misi�n en el NPC
            }

            assignedNPC.hasMission = false; // Actualiza el estado del NPC
        }

        // Limpia la referencia al NPC
        assignedNPC = null;

        // Establece currentMission a null si es necesario (aqu� asumiendo que se requiere)
        currentMission = null; // Limpia la referencia a la misi�n actual
    }



    // Implementaci�n de los m�todos de la interfaz
    public void setActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public bool isActive()
    {
        return gameObject.activeSelf;
    }

    public void reset()
    {
        // Resetea el estado del icono, si es necesario
        
    }
}