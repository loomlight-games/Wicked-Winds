using UnityEngine;

[CreateAssetMenu(fileName = "New Mission", menuName = "Mission")]
public class MissionData : ScriptableObject
{
    public string missionName;
    public string missionDescription;
    public int difficulty; // 1 f�cil, 2 intermedio, 3 dif�cil
    public bool isCompleted = false;
}
