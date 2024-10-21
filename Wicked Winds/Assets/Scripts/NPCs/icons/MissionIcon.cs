using System.Reflection;
using UnityEngine;

public class MissionIcon : MonoBehaviour, IPoolable
{
    public SpriteRenderer iconRenderer; // Referencia al SpriteRenderer del ícono
    public Sprite missionSprite; // Sprite para la misión activa
    public Sprite completedSprite; // Sprite de la carita feliz (cuando se complete la misión)

    public MissionData currentMission; // La misión asignada a este ícono
    private MissionManager missionManager; // Referencia al MissionManager

    // Método para asignar una misión a este ícono
    public void AssignMission(MissionData mission, MissionManager manager)
    {
        currentMission = mission;
        missionManager = manager;
    }

    // Este método es llamado cuando el objeto es tomado del pool
    public void OnObjectSpawn()
    {
        // Restablecer el sprite al de misión activa
        iconRenderer.sprite = missionSprite;

        // Si hay una misión asignada, marcarla como no completada
        if (currentMission != null)
        {
            currentMission.isCompleted = false;
        }
    }

    // Este método es llamado cuando el objeto es devuelto al pool
    public void OnObjectReturn()
    {
        // Limpiar la misión asignada
        currentMission = null;
    }

    // Llamar cuando la misión se complete
    public void CompleteMission()
    {
        if (currentMission != null && !currentMission.isCompleted)
        {
            // Marcar la misión como completada
            currentMission.isCompleted = true;

            // Cambiar el sprite al de misión completada (carita feliz)
            iconRenderer.sprite = completedSprite;

            // Avisar al MissionManager para verificar si todas las misiones se han completado
            missionManager.CheckMissionCompletion();
        }
    }
}
