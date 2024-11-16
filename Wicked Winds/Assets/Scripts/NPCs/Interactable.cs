using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NPC))]
public class Interactable : MonoBehaviour
{
    private NPC npc; // Referencia al NPC con el que se interactua
    public MissionIcon missionIcon; // Referencia al icono de mision del NPC
    private NPC activeNPC; // Referencia al NPC con el que se esta interactuando
    public Dialogue dialoguePanel; // Referencia al script del bocadillo de texto

    private void Start()
    {
        // Busca el NPC en el GameObject padre o en los hijos
        npc = GetComponent<NPC>();
        dialoguePanel = FindObjectOfType<Dialogue>();
        missionIcon = npc.missionIcon; // Obtiene el icono de mision del NPC
    }

    // Metodo para manejar la interaccion con el NPC
    public void Interact()
    {
        

        // Has mission assigned
        if (PlayerManager.Instance.hasActiveMission)
        {
            // If NPC is target
            if (PlayerManager.Instance.currentTargets.Contains(gameObject))
            {
                // Iniciar el dialogo con el mensaje del NPC
                if (dialoguePanel != null && activeNPC.responseMessage != null)
                {
                    dialoguePanel.lines = new string[] { activeNPC.responseMessage }; // Asigna el mensaje del NPC al bocadillo
                    dialoguePanel.StartDialogue(activeNPC); // Inicia el dialogo
                }
                npc.OnMissionCompleted();
            }
            // NPC is the assigned but not objects have been found
            else
            {
                GameManager.Instance.playState.feedBackText.text = "You must collect all items of the current mission.";
            }
        }
        // No mission assigned
        else
        {
            // If NPC has mission
            if (npc.hasMission)
            {
                // Empiezan conversacion
                activeNPC = npc; // Guarda el NPC con el que se interactua

                // Cambia el sprite del icono de mision al sprite de la mision
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

                // Iniciar el dialogo con el mensaje del NPC
                if (dialoguePanel != null && activeNPC.message != null)
                {
                    dialoguePanel.lines = new string[] { activeNPC.message }; // Asigna el mensaje del NPC al bocadillo
                    dialoguePanel.StartDialogue(activeNPC); // Inicia el dialogo
                }


                PlayerManager.Instance.activeMission = activeNPC.missionIcon;
                GameManager.Instance.playState.feedBackText.text = $"New mission accepted from {npc.name}: {missionIcon.name}.";

                if (PlayerManager.Instance.activeMission.currentMission.missionName == "LetterMision")
                {
                    string objetivo = activeNPC.missionIcon.addresseeName;

                    NPC[] allNPCS = FindObjectsOfType<NPC>();

                    foreach (NPC npc in allNPCS)
                    {
                        if (npc.npcname == objetivo)
                        {
                            PlayerManager.Instance.AddTarget(npc.gameObject);
                        }
                    }
                }

                PlayerManager.Instance.hasActiveMission = true; // Marca que el jugador tiene una mision activa
            }
        }
    }
}
