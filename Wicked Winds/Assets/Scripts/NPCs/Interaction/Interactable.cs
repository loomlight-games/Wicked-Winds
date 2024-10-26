using UnityEngine;

public class Interactable : MonoBehaviour
{
    private NPC npc; // Referencia al NPC con el que se interact�a
    public MissionIcon missionIcon; // Referencia al �cono de misi�n del NPC
    private NPC activeNPC; // Referencia al NPC con el que se est� interactuando actualmente
    [SerializeField] public int collectedItemsCount = 0; // Contador de objetos recolectados

    //private Player player; // Referencia al jugador

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

        // Verifica si el jugador ya tiene una misi�n activa
        if (PlayerManager.Instance.hasActiveMission)
        {
            Debug.Log($"{PlayerManager.Instance.name} already has an active mission and cannot accept a new one."); // Log si ya tiene misi�n activa
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

        PlayerManager.Instance.hasActiveMission = true; // Marca que el jugador tiene una misi�n activa
        Debug.Log($"{PlayerManager.Instance.name} accepted a new mission from {npc.name}."); // Log para aceptar la misi�n
    }

    public void CollectItem()
    {
        if (this.missionIcon != null) // Verifica si el NPC acepta la misi�n
        {
            missionIcon.CollectItem(); // Llama al m�todo de MissionIcon para contar el objeto recolectado
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
