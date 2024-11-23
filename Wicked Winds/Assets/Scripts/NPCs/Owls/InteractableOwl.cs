using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableOwl : MonoBehaviour
{
    public OwlController owl; // Referencia al NPC con el que se interactua
    public NPC owner;
    public Dialogue dialoguePanel; // Referencia al script del bocadillo de texto
    public OwlController activeOwl;

    private void Start()
    {

        owl = GetComponent<OwlController>();

        owner = owl.owner;
    }

    // Metodo para manejar la interaccion con el NPC
    public void Interact()
    {

        // Has mission assigned
        if (PlayerManager.Instance.hasActiveMission)
        {
            if (PlayerManager.Instance.currentTargets.Contains(gameObject))
            {
                activeOwl = owl;

                // Iniciar el diálogo con el mensaje del gato
                if (dialoguePanel != null)
                {
                    String msg = "¡Uhú, uhú! \n ";
                    dialoguePanel.lines = null;
                    dialoguePanel.lines = new string[] { msg }; // Asigna el mensaje del NPC al bocadillo
                    dialoguePanel.StartDialogue(owner, msg, 1); // Inicia el dialogo
                    Debug.Log("Diálogo iniciado.");
                }

                
                owl.InteractWithOwl();
                GameManager.Instance.playState.feedBackText.text = "Buho encontrado. Llevalo junto a su dueño\n";



            }
            // NPC is the assigned but not objects have been found
            if (!PlayerManager.Instance.currentTargets.Contains(gameObject))
            {
                // Iniciar el diálogo con el mensaje del gato
                if (dialoguePanel != null)
                {
                    String msg1 = "¡Cruuu! ¡Hoo-hooo!  \n ¡Uhú, uhú!";
                    Debug.Log("El mensaje del GATO existe. Asignando el mensaje al bocadillo...");
                    dialoguePanel.lines = null;
                    dialoguePanel.lines = new string[] { msg1 }; // Asigna el mensaje del NPC al bocadillo
                    dialoguePanel.StartDialogue(owner, msg1, 1); // Inicia el dialogo
                    Debug.Log("Diálogo iniciado.");
                }
            }
        }
        if (!PlayerManager.Instance.hasActiveMission)
        {
            // Iniciar el diálogo con el mensaje del gato
            if (dialoguePanel != null)
            {
                String msg2 = "buuuuu \n  UUUUUHUUUUU";
                Debug.Log("El mensaje del GATO existe. Asignando el mensaje al bocadillo...");
                dialoguePanel.lines = null;
                dialoguePanel.lines = new string[] { msg2 }; // Asigna el mensaje del NPC al bocadillo
                dialoguePanel.StartDialogue(owner, msg2, 1); // Inicia el dialogo
                Debug.Log("Diálogo iniciado.");
            }
        }

    }
}
