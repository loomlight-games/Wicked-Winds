using UnityEngine;

public class Interactable : MonoBehaviour
{
    private NPC npc; // Referencia al NPC con el que se interactúa
    public MissionIcon missionIcon; // Referencia al ícono de misión del NPC
    private NPC activeNPC; // Referencia al NPC con el que se está interactuando actualmente
    [SerializeField] private int collectedItemsCount = 0; // Contador de objetos recolectados

    private Player player; // Referencia al jugador

    private void Start()
    {
        // Busca el NPC en el GameObject padre o en los hijos
        npc = GetComponent<NPC>();
        player = FindObjectOfType<Player>(); // Encuentra el jugador en la escena
        if (npc != null)
        {
            missionIcon = npc.missionIcon; // Obtiene el icono de misión del NPC
        }
    }

    // Método para manejar la interacción con el NPC
    public void Interact()
    {
        if (npc == null)
        {
            Debug.LogWarning("No NPC found for interaction.");
            return;
        }

        Debug.Log($"Interacted with NPC: {npc.name}");

        // Verifica si el jugador ya tiene una misión activa
        if (player.hasActiveMission)
        {
            Debug.Log($"{player.name} already has an active mission and cannot accept a new one."); // Log si ya tiene misión activa
            return; // Sale del método si ya hay una misión activa
        }

        activeNPC = npc; // Guarda el NPC con el que se interactúa

        // Cambia el sprite del icono de misión al sprite de la misión
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

        player.hasActiveMission = true; // Marca que el jugador tiene una misión activa
        Debug.Log($"{player.name} accepted a new mission from {npc.name}."); // Log para aceptar la misión
    }

    public void CollectItem()
    {
        if (npc != null && npc.acceptMission) // Verifica si el NPC acepta la misión
        {
            collectedItemsCount++;
            Debug.Log($"Objeto recolectado. Total recolectados: {collectedItemsCount}/3");

            if (collectedItemsCount >= 3)
            {
                missionIcon.CompleteMission();
                Debug.Log("Misión completada.");
                collectedItemsCount = 0; // Reinicia el contador para futuras misiones
                player.hasActiveMission = false; // Reinicia el estado de la misión activa
                Debug.Log($"{player.name} has completed the mission."); // Log de misión completada
            }
        }
        else
        {
            Debug.Log($"{npc.name} cannot collect the item because acceptMission is false.");
        }
    }

    private void Update()
    {
        // Si se presiona la tecla 'C' y se ha interactuado con un NPC
        if (Input.GetKeyDown(KeyCode.C) && activeNPC != null && missionIcon != null)
        {
            Debug.Log("Interacting again with the NPC.");
            missionIcon.CompleteMission();
        }
    }
}
