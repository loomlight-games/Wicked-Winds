using UnityEngine;

[CreateAssetMenu(fileName = "New Mission", menuName = "Mission")]
public class MissionData : ScriptableObject
{
    public string missionName;
    public string missionDescription;
    public int difficulty; // 1 fácil, 2 intermedio, 3 difícil
    public bool isCompleted = false;
}
