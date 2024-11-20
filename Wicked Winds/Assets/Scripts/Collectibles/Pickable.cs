using UnityEngine;

public class Pickable : MonoBehaviour
{
    // Referencia al NPC asociado
    public NPC npc;
    public MissionIcon missionIcon; // Referencia al icono de mision del NPC
    public int numOfObjectsToCollect;
    public Dialogue textBubble; // Referencia al bocadillo de texto



    // Metodo para establecer el NPC

    private void Start()
    {
        textBubble = FindObjectOfType<Dialogue>();
    }

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

    // M�todo para recolectar el objeto
    public void CollectItem()
    {
        if (npc != null && PlayerManager.Instance.currentTargets != null)
        {
            if (PlayerManager.Instance.currentTargets.Contains(this.gameObject))
            {
                missionIcon = npc.missionIcon;
                if (this.missionIcon != null)
                {
                    this.missionIcon.collectedItemsCount++;
                    // Quitar el ingrediente de la lista de objetivos y destruir el objeto recolectado
                    PlayerManager.Instance.RemoveTarget(gameObject);
                    Destroy(gameObject);
                    Debug.Log($"{npc.name} collected the item: {gameObject.name}");

                    if (this.missionIcon.collectedItemsCount >= numOfObjectsToCollect)
                    {
                        // Reinicia el contador para futuras misiones
                        this.missionIcon.collectedItemsCount = 0;

                        // Aniade el NPC como nuevo objetivo en `currentTargets`
                        PlayerManager.Instance.AddTarget(missionIcon.assignedNPC.gameObject);
                        GameManager.Instance.playState.feedBackText.text = "Todos los objetos recolectados. Regresa al NPC para completar la mision.\n";
                            
                        

                    }
                }

            }
            
           


        }
        
    }

}
