using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    // Referencia al NPC asociado
    private NPC npc;
    public MissionIcon missionIcon; // Referencia al �cono de misi�n del NPC
    public int numOfObjectsToCollect;

    // M�todo para establecer el NPC
    public void SetNPC(NPC assignedNPC)
    {
        npc = assignedNPC;
        Debug.Log($"Assigned NPC to pickable item: {npc.name}"); // Log para asignaci�n de NPC
    }

    // M�todo para recolectar el objeto
    public void CollectItem()
    {
        //if (npc != null && npc.acceptMission) // Verifica si el NPC acepta la misi�n
        if (npc != null)
        {
            missionIcon = npc.missionIcon; // Obtiene el icono de misi�n del NPC
            if (this.missionIcon != null) // Verifica si el NPC acepta la misi�n
            {
                this.missionIcon.collectedItemsCount++; ;
                
                Debug.Log($"Objeto recolectado. Total recolectados: {this.missionIcon.collectedItemsCount}/3");

                if (this.missionIcon.collectedItemsCount >= numOfObjectsToCollect)
                {
                    this.missionIcon.collectedItemsCount = 0;// Reinicia el contador para futuras misiones
                    missionIcon.CompleteMission();
                    Debug.Log("Misi�n completada.");
                     
                }
            }
            else
            {
                Debug.Log($"{npc.name} cannot collect the item because acceptMission is false.");
            }
            // Destruir el objeto cuando se recolecte
            Destroy(gameObject);
            Debug.Log($"{npc.name} collected the item: {gameObject.name}"); // Log de objeto recogido
        }
        else
        {
            Debug.Log($"{npc.name} cannot collect the item because acceptMission is false."); // Log si no se puede recoger
        }
    }
}
