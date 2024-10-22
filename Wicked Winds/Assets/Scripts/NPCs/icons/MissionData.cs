using UnityEngine;

[CreateAssetMenu(fileName = "New Mission", menuName = "Mission")]
public class MissionData : ScriptableObject
{
    public string missionName;
    public string missionDescription;
    public int difficulty; // 0 fácil, 1 medio, 2 difícil
    public Sprite missionIconSprite; // El ícono específico para esta misión
    public bool isCompleted = false;

}
