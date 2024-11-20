using System;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

[RequireComponent(typeof(CatController))]
public class InteractableCat : MonoBehaviour
{
    public CatController cat; // Referencia al NPC con el que se interactua
    public NPC owner;
    public Dialogue dialoguePanel; // Referencia al script del bocadillo de texto
    public CatController activeCat;

    private void Start()
    {

        cat = GetComponent<CatController>();

        owner = cat.owner;
    }

    // Metodo para manejar la interaccion con el NPC
    public void InteractCat()
    {

        // Has mission assigned
        if (PlayerManager.Instance.hasActiveMission)
        {
            if (PlayerManager.Instance.currentTargets.Contains(gameObject))
            {
                activeCat = cat;
               
                // Iniciar el diálogo con el mensaje del gato
                if (dialoguePanel != null)
                {
                    String msg = "miau miau miau\n ";
                    Debug.Log("El mensaje del GATO existe. Asignando el mensaje al bocadillo...");
                    dialoguePanel.lines = null;
                    dialoguePanel.lines = new string[] { msg }; // Asigna el mensaje del NPC al bocadillo
                    dialoguePanel.StartDialogue(owner, msg,1); // Inicia el dialogo
                    Debug.Log("Diálogo iniciado.");
                }


                cat.InteractWithCat();
                GameManager.Instance.playState.feedBackText.text = "Gato encontrado. Llevalo junto a su dueño\n";



            }
            // NPC is the assigned but not objects have been found
            if (!PlayerManager.Instance.currentTargets.Contains(gameObject))
            {
                // Iniciar el diálogo con el mensaje del gato
                if (dialoguePanel != null)
                {
                    String msg1 = "meow meow miaramiaumiau \n shhhhhhhhhhhhh";
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
                String msg2 = "MAAAAAUW \n  miuamiau miau";
                Debug.Log("El mensaje del GATO existe. Asignando el mensaje al bocadillo...");
                dialoguePanel.lines = null;
                dialoguePanel.lines = new string[] { msg2 }; // Asigna el mensaje del NPC al bocadillo
                dialoguePanel.StartDialogue(owner, msg2, 1); // Inicia el dialogo
                Debug.Log("Diálogo iniciado.");
                 }
        }

    }


     
}
