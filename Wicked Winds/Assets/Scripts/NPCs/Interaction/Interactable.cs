using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private NPC npc; // Referencia al NPC con el que se interact�a
    public MissionIcon missionIcon; // Referencia al �cono de misi�n del NPC
    private NPC activeNPC; // Referencia al NPC con el que se est� interactuando
    public Dialogue textBubble; // Referencia al script del bocadillo de texto

    private void Start()
    {
        // Busca el NPC en el GameObject padre o en los hijos
        npc = GetComponent<NPC>();
        if (npc != null)
        {
            missionIcon = npc.missionIcon; // Obtiene el icono de misi�n del NPC
        }
    }

    // M�todo para manejar la interacci�n con el NPC
    public void Interact()
    {
        
        if (npc == null)
        {
            Debug.Log("No NPC found for interaction.");
            return;
        }

        Debug.Log($"Interacted with NPC: {npc.name}");

        // Has mission assigned
        if (PlayerManager.Instance.hasActiveMission)
        {
            // If NPC is target
            if (PlayerManager.Instance.currentTargets.Contains(npc.gameObject))
                npc.OnInteractAfterCollection();
            // NPC is the assigned but not objects have been found
            else{
                Debug.Log("Aún necesitas recolectar todos los objetos antes de completar la misión.");
            }
                // completa
            // si es q se lo ha asignaio
                // aparece otra vez solo conversación
            // si no es ninguno
                // aviso de que no puede tener otra
            
        }
        // No mission assigned
        else
        {
            // empiezan conversación
            activeNPC = npc; // Guarda el NPC con el que se interact�a

            // Cambia el sprite del icono de misi�n al sprite de la misi�n
            if (missionIcon != null && missionIcon.currentMission != null)
            {
                SpriteRenderer spriteRenderer = missionIcon.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.sprite = missionIcon.currentMission.missionIconSprite;
                    
                    Debug.Log($"Changed mission icon sprite to: {missionIcon.currentMission.missionIconSprite.name}");
                }
                else
                {
                    Debug.LogError("SpriteRenderer es nulo en Interactable.");
                }

            }
            
            // Iniciar el di�logo con el mensaje del NPC
            if (textBubble != null && activeNPC.message != null)
            {
                textBubble.lines = new string[] { activeNPC.message }; // Asigna el mensaje del NPC al bocadillo
                textBubble.StartDialogue(activeNPC); // Inicia el di�logo
            }

            PlayerManager.Instance.hasActiveMission = true; // Marca que el jugador tiene una misi�n activa
            PlayerManager.Instance.activeMission = missionIcon;
            Debug.Log($"{PlayerManager.Instance.name} accepted a new mission from {npc.name}.");
            if (PlayerManager.Instance.activeMission.currentMission.missionName == "LetterMision")
            {
                string objetivo = activeNPC.missionIcon.addressee;
                NPC[] allNPCS = FindObjectsOfType<NPC>();
                foreach (NPC npc in allNPCS)
                {
                    if (npc.npcname == objetivo)
                    {
                        PlayerManager.Instance.AddTarget(npc.gameObject);
                    }
                }
            }
        }
    }
}
