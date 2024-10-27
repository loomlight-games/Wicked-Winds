using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    // Referencia al NPC asociado
    public NPC npc;
    public MissionIcon missionIcon; // Referencia al ícono de misión del NPC
    public int numOfObjectsToCollect;
    public NewBehaviourScript playerTextBubble; // Referencia al bocadillo de texto

    // Método para establecer el NPC

    private void Update()
    {
        // Verifica si la misión del objeto es la misma que la misión activa del jugador y que ambas misiones no sean nulas
        if (this.missionIcon != null && PlayerManager.Instance.activeMission != null &&
            this.missionIcon == PlayerManager.Instance.activeMission)
        {
            PlayerManager.Instance.AddTarget(gameObject);
        }

    }
    public void SetNPC(NPC assignedNPC)
    {
        npc = assignedNPC;
        Debug.Log($"Assigned NPC to pickable item: {npc.name}"); // Log para asignación de NPC
    }

    // Método para recolectar el objeto
    public void CollectItem()
    {
        if (npc != null && PlayerManager.Instance.currentTargets != null)
        {
            if (PlayerManager.Instance.currentTargets.Contains(gameObject))
            {
                missionIcon = npc.missionIcon;
                if (this.missionIcon != null)
                {
                    this.missionIcon.collectedItemsCount++;
                    Debug.Log($"Objeto recolectado. Total recolectados: {this.missionIcon.collectedItemsCount}/{numOfObjectsToCollect}");

                    if (this.missionIcon.collectedItemsCount >= numOfObjectsToCollect)
                    {
                        // Reinicia el contador para futuras misiones
                        this.missionIcon.collectedItemsCount = 0;

                        // Añade el NPC como nuevo objetivo en `currentTargets`
                        PlayerManager.Instance.AddTarget(missionIcon.assignedNPC.gameObject);

                        Debug.Log("Todos los objetos recolectados. Regresa al NPC para completar la misión.");
                        // Activa el bocadillo de texto y muestra el mensaje
                        if (playerTextBubble != null)
                        {
                            Debug.Log("no hay player text bubble");
                            string texto = "Todos los objetos recolectados. Regresa al NPC para completar la misión.\n";
                            playerTextBubble.StartDialogue(texto); // Inicia el diálogo en el bocadillo de texto
                        }

                    }
                }
            
            }
            else
            {
                Debug.Log($"{npc.name} cannot collect the item because acceptMission is false.");
            }

            if (PlayerManager.Instance.currentTargets.Contains(gameObject)){
                // Quitar el ingrediente de la lista de objetivos y destruir el objeto recolectado
                PlayerManager.Instance.RemoveTarget(gameObject);
                Destroy(gameObject);
                Debug.Log($"{npc.name} collected the item: {gameObject.name}");


            }
            else
            {
                Debug.Log($"Este ingrediente aún no se ha marcado como objetivo por lo q no se puede recoger");
            }
            
        }
        else
        {
            Debug.Log($"{npc.name} cannot collect the item because acceptMission is false.");
        }
    }

}
