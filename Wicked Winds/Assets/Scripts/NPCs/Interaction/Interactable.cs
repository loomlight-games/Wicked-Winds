using UnityEngine;

public class Interactable : MonoBehaviour
{
    private NPC npc; // Referencia al NPC con el que se interact�a
    public MissionIcon missionIcon; // Referencia al �cono de misi�n del NPC
    private NPC activeNPC; // Referencia al NPC con el que se est� interactuando
    public NewBehaviourScript textBubble; // Referencia al script del bocadillo de texto

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
        npc.OnInteractAfterCollection(); //miro a ver si es el target y si lo es completara la mision

        // Verifica si el jugador ya tiene una misi�n activa
        if (PlayerManager.Instance.hasActiveMission)
        {
            Debug.Log($"{PlayerManager.Instance.name} already has an active mission and cannot accept a new one.");
            return; // Sale del m�todo si ya hay una misi�n activa
        }

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
        Debug.Log($"{PlayerManager.Instance.name} accepted a new mission from {npc.name}.");
    }
}
