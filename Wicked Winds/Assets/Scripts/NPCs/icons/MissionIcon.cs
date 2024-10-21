using System.Reflection;
using UnityEngine;

public class MissionIcon : MonoBehaviour, IPoolable
{
    public SpriteRenderer iconRenderer; // Referencia al SpriteRenderer del �cono
    public Sprite missionSprite; // Sprite para la misi�n activa
    public Sprite completedSprite; // Sprite de la carita feliz (cuando se complete la misi�n)

    public MissionData currentMission; // La misi�n asignada a este �cono
    private MissionManager missionManager; // Referencia al MissionManager

    // M�todo para asignar una misi�n a este �cono
    public void AssignMission(MissionData mission, MissionManager manager)
    {
        currentMission = mission;
        missionManager = manager;
    }

    // Este m�todo es llamado cuando el objeto es tomado del pool
    public void OnObjectSpawn()
    {
        // Restablecer el sprite al de misi�n activa
        iconRenderer.sprite = missionSprite;

        // Si hay una misi�n asignada, marcarla como no completada
        if (currentMission != null)
        {
            currentMission.isCompleted = false;
        }
    }

    // Este m�todo es llamado cuando el objeto es devuelto al pool
    public void OnObjectReturn()
    {
        // Limpiar la misi�n asignada
        currentMission = null;
    }

    // Llamar cuando la misi�n se complete
    public void CompleteMission()
    {
        if (currentMission != null && !currentMission.isCompleted)
        {
            // Marcar la misi�n como completada
            currentMission.isCompleted = true;

            // Cambiar el sprite al de misi�n completada (carita feliz)
            iconRenderer.sprite = completedSprite;

            // Avisar al MissionManager para verificar si todas las misiones se han completado
            missionManager.CheckMissionCompletion();
        }
    }
}
