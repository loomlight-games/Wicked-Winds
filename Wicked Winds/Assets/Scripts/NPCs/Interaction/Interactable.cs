using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NPC))]
public class Interactable : MonoBehaviour
{
    private NPC npc; // Referencia al NPC con el que se interact�a
    public MissionIcon missionIcon; // Referencia al �cono de misi�n del NPC
    private NPC activeNPC; // Referencia al NPC con el que se est� interactuando
    public Dialogue dialoguePanel; // Referencia al script del bocadillo de texto

    private void Start()
    {
        // Busca el NPC en el GameObject padre o en los hijos
        npc = GetComponent<NPC>();
        missionIcon = npc.missionIcon; // Obtiene el icono de misi�n del NPC
    }

    // M�todo para manejar la interacci�n con el NPC
    public void Interact()
    {
        Debug.Log($"Interacted with NPC: {npc.name}");

        // Has mission assigned
        if (PlayerManager.Instance.hasActiveMission)
        {
            // If NPC is target
            if (PlayerManager.Instance.currentTargets.Contains(gameObject))
                npc.OnMissionCompleted();
            // NPC is the assigned but not objects have been found
            else{
                GameManager.Instance.feddBackText.text = "You must collect all items of the current mission.";
            }
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
                    
                    //Debug.Log($"Changed mission icon sprite to: {missionIcon.currentMission.missionIconSprite.name}");
                }
                else
                {
                    //Debug.LogError("SpriteRenderer es nulo en Interactable.");
                }

            }
            
            // Iniciar el di�logo con el mensaje del NPC
            if (dialoguePanel != null && activeNPC.message != null)
            {
                dialoguePanel.lines = new string[] { activeNPC.message }; // Asigna el mensaje del NPC al bocadillo
                dialoguePanel.StartDialogue(activeNPC); // Inicia el di�logo
            }


            PlayerManager.Instance.activeMission = activeNPC.missionIcon;
            GameManager.Instance.feddBackText.text = $"New mission accepted from {npc.name}: {missionIcon.name}.";
            
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

            PlayerManager.Instance.hasActiveMission = true; // Marca que el jugador tiene una misi�n activa
        }
    }
}
