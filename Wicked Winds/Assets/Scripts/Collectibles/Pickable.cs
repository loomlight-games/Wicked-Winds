using UnityEngine;

public class Pickable : MonoBehaviour
{
    // Referencia al NPC asociado
    public NPC npc;
    public MissionIcon missionIcon; // Referencia al icono de mision del NPC
    public int numOfObjectsToCollect;

    private void Update()
    {
        if (PlayerManager.Instance.activeMission != null &&
            this.missionIcon != null &&
            this.missionIcon.missionID == PlayerManager.Instance.activeMission.missionID)
        {
            if (!PlayerManager.Instance.currentTargets.Contains(gameObject))
            {
                PlayerManager.Instance.AddTarget(gameObject);
            }
        }
    }

    public void SetNPC(NPC assignedNPC)
    {
        npc = assignedNPC;
    }

    /// <summary>
    /// Collects the item and updates the mission icon.
    /// </summary>
    public void CollectItem()
    {
        if (npc != null && PlayerManager.Instance.currentTargets != null)
        {
            if (PlayerManager.Instance.currentTargets.Contains(gameObject))
            {
                missionIcon = npc.request;

                if (missionIcon != null)
                {
                    missionIcon.collectedItemsCount++;

                    // Remove ingredient from targets and destroy it
                    PlayerManager.Instance.RemoveTarget(gameObject);
                    Destroy(gameObject);

                    // At least all targets found
                    if (missionIcon.collectedItemsCount >= numOfObjectsToCollect)
                    {
                        // Restarts counter
                        missionIcon.collectedItemsCount = 0;

                        // Makes NPC the next target 
                        PlayerManager.Instance.AddTarget(missionIcon.assignedNPC.gameObject);
                        GameManager.Instance.playState.feedBackText.text = $"All ingredients found! Bring them to {npc.npcName}\n";
                    }
                }
            }
        }
    }
}
