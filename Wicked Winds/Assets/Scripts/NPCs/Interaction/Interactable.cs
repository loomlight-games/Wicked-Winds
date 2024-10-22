using UnityEngine;

public class Interactable : MonoBehaviour
{
    private NPC npc; // Referencia al NPC con el que se interactúa
    private MissionIcon missionIcon; // Referencia al ícono de misión del NPC

    private void Start()
    {
        // Busca el NPC en el GameObject padre o en los hijos
        npc = GetComponent<NPC>();
        if (npc != null)
        {
            missionIcon = npc.missionIcon; // Obtiene el icono de misión del NPC
        }
    }

    public void Interact()
    {
        if (npc == null)
            return;

        Debug.Log($"Interacted with NPC: {npc.name}");

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
        if (Input.GetKeyDown(KeyCode.C) && npc != null && missionIcon != null)
        {
            missionIcon.CompleteMission(); // Completa la misión del NPC
            missionIcon.transform.parent.gameObject.SetActive(false); // Desactiva la burbuja
        }
    }
}
