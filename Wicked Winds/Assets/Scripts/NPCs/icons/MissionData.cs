using UnityEngine;

[CreateAssetMenu(fileName = "New Mission", menuName = "Mission")]
public class MissionData : ScriptableObject
{
    public string missionName;
    public string missionDescription;
    public int difficulty; // 0 f�cil, 1 medio, 2 dif�cil
    public Sprite missionIconSprite; // El �cono espec�fico para esta misi�n
    public bool isCompleted = false;

}
