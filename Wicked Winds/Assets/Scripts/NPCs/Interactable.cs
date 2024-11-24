using System;
using UnityEngine;

[RequireComponent(typeof(NPC))]
public class Interactable : MonoBehaviour
{
    public NPC npc; // Referencia al NPC con el que se interactua
    public MissionIcon missionIcon; // Referencia al icono de mision del NPC
    public NPC activeNPC;


    public Dialogue dialoguePanel; // Referencia al script del bocadillo de texto

    private void Start()
    {
        // Busca el NPC en el GameObject padre o en los hijos
        npc = GetComponent<NPC>();

        missionIcon = this.npc.missionIcon; // Obtiene el icono de mision del NPC
    }

    // Metodo para manejar la interaccion con el NPC
    public void Interact()
    {

        // Has mission assigned
        if (PlayerManager.Instance.hasActiveMission)
        {
            Debug.Log("el personaje tiene mision activa");

            // If NPC is target
            if (PlayerManager.Instance.currentTargets.Contains(gameObject))
            {
                if(desactivarOwlUI.Instance.activateOwlUI==true)
                {
                    desactivarOwlUI.Instance.activateOwlUI = false;
                }
                activeNPC = npc;
                Debug.Log("como el npc es target");
                PlayerManager.Instance.npcObjective = npc;
                Debug.Log($"El NPC es el objetivo. Iniciando diálogo... {PlayerManager.Instance.npcObjective}");

                // Iniciar el diálogo con el mensaje del NPC
                if (dialoguePanel != null && PlayerManager.Instance.npcObjective.responseMessage != null)
                {
                    Debug.Log("El mensaje del NPC existe. Asignando el mensaje al bocadillo...");
                    dialoguePanel.lines = null;
                    dialoguePanel.lines = new string[] { activeNPC.responseMessage }; // Asigna el mensaje del NPC al bocadillo
                    dialoguePanel.StartDialogue(activeNPC, activeNPC.responseMessage,0); // Inicia el dialogo
                    Debug.Log("Diálogo iniciado.");
                }
                else
                {
                    if (dialoguePanel == null)
                        Debug.LogWarning("dialoguePanel es null.");

                    if (npc.responseMessage == null)
                        Debug.LogWarning("npc.responseMessage es null.");
                }

                // Completar la misión del NPC
                Debug.Log("Llamando a OnMissionCompleted...");

                if(PlayerManager.Instance.npcMissionActive.cat!= null)
                {
                    PlayerManager.Instance.npcMissionActive.cat.ChangeState(PlayerManager.Instance.npcMissionActive.cat.followingOwnerState);
                }
                PlayerManager.Instance.npcMissionActive.OnMissionCompleted();
                PlayerManager.Instance.currentTargets.Remove(PlayerManager.Instance.npcObjective.gameObject);
                PlayerManager.Instance.npcMissionActive = null;
                PlayerManager.Instance.npcObjective = null;
                
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
                activeNPC = npc;
                Debug.Log("NUEVA MSISION ACEPTADA");
                // Empiezan conversacion
                PlayerManager.Instance.npcMissionActive = npc; // Guarda el NPC con el que se interactua


                // Cambia el sprite del icono de mision al sprite de la mision
                if (missionIcon != null && missionIcon.currentMission != null)
                {
                    SpriteRenderer spriteRenderer = missionIcon.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.sprite = missionIcon.currentMission.missionIconSprite;

                        Debug.Log($"Changed mission icon sprite to: {missionIcon.currentMission.missionIconSprite.name}");
                    }

                }

                // Iniciar el dialogo con el mensaje del NPC
                if (dialoguePanel != null && PlayerManager.Instance.npcMissionActive.message != null)
                {
                    Debug.Log("EL PERSONAJE ESTA DICIENDO LA MISION");
                    dialoguePanel.lines = null;
                    dialoguePanel.lines = new string[] { PlayerManager.Instance.npcMissionActive.message }; // Asigna el mensaje del NPC al bocadillo
                    dialoguePanel.StartDialogue(activeNPC, activeNPC.message,0); // Inicia el dialogo
                }


                PlayerManager.Instance.activeMission = PlayerManager.Instance.npcMissionActive.missionIcon;
                GameManager.Instance.playState.feedBackText.text = $"New mission accepted from {PlayerManager.Instance.npcMissionActive.name}: {PlayerManager.Instance.npcMissionActive.missionIcon.name}.";
                if (PlayerManager.Instance.activeMission.currentMission.missionName == "LetterMision")
                {
                    // Obtén el ID del NPC objetivo desde la misión activa
                    Guid objetivoID = PlayerManager.Instance.npcMissionActive.missionIcon.addressee.npcID;

                    // Encuentra todos los NPCs en la escena
                    NPC[] allNPCs = FindObjectsOfType<NPC>();

                    foreach (NPC npc in allNPCs)
                    {
                        // Compara el ID del NPC con el objetivo
                        if (npc.npcID == objetivoID)
                        {
                            Debug.Log("añadiendo como target al destinatario");
                            PlayerManager.Instance.AddTarget(npc.gameObject);
                        }
                    }
                }

                if (PlayerManager.Instance.activeMission.currentMission.missionName == "CatMission")
                {
                    GameObject objetivo = PlayerManager.Instance.npcMissionActive.cat.gameObject;
                    PlayerManager.Instance.AddTarget(objetivo.gameObject);
                    Debug.Log("Añadiendo como target al gato perdido");


                }

                PlayerManager.Instance.hasActiveMission = true; // Marca que el jugador tiene una mision activa
            }
        }
    }
}
