using UnityEngine;

public class Interactable : MonoBehaviour
{
    private NPC npc; // Referencia al NPC con el que se interact�a
    private MissionIcon missionIcon; // Referencia al �cono de misi�n del NPC
    private NPC activeNPC; // Referencia al NPC con el que se est� interactuando actualmente

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
            return;

        Debug.Log($"Interacted with NPC: {npc.name}");
        activeNPC = npc; // Guarda el NPC con el que se interact�a

        // Cambia el sprite del icono de misi�n al sprite de la misi�n
        if (missionIcon != null && missionIcon.currentMission != null)
        {
            SpriteRenderer spriteRenderer = missionIcon.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = missionIcon.currentMission.missionIconSprite; // Cambia al sprite de la misi�n
            }
            else
            {
                Debug.LogError("SpriteRenderer es nulo en Interactable.");
            }
        }
    }

    private void Update()
    {
        // Si se presiona la tecla 'C' y se ha interactuado con un NPC
        if (Input.GetKeyDown(KeyCode.C) && activeNPC != null && missionIcon != null)
        {
            missionIcon.CompleteMission(); // Completa la misi�n del NPC actual
            missionIcon.transform.parent.gameObject.SetActive(false); // Desactiva la burbuja del NPC actual
        }
    }
}