using UnityEngine;

public class Interactable : MonoBehaviour
{
    private NPC npc; // Referencia al NPC con el que se interactúa
    private MissionIcon missionIcon; // Referencia al ícono de misión del NPC
    private NPC activeNPC; // Referencia al NPC con el que se está interactuando actualmente

    private void Start()
    {
        // Busca el NPC en el GameObject padre o en los hijos
        npc = GetComponent<NPC>();
        if (npc != null)
        {
            missionIcon = npc.missionIcon; // Obtiene el icono de misión del NPC
        }
    }

    // Método para manejar la interacción con el NPC
    public void Interact()
    {
        if (npc == null)
            return;

        Debug.Log($"Interacted with NPC: {npc.name}");
        activeNPC = npc; // Guarda el NPC con el que se interactúa

        // Cambia el sprite del icono de misión al sprite de la misión
        if (missionIcon != null && missionIcon.currentMission != null)
        {
            SpriteRenderer spriteRenderer = missionIcon.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = missionIcon.currentMission.missionIconSprite; // Cambia al sprite de la misión
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
            missionIcon.CompleteMission(); // Completa la misión del NPC actual
            missionIcon.transform.parent.gameObject.SetActive(false); // Desactiva la burbuja del NPC actual
        }
    }
}