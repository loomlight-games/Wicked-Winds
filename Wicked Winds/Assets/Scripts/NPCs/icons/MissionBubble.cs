using UnityEngine;

public class MissionBubble : MonoBehaviour
{
    public IconPool iconPool; // Referencia al pool genérico
    public Transform iconPosition; // La posición donde quieres que aparezca el ícono
    public float detectionRadius = 3.0f; // El radio de detección para que aparezca el ícono de misión
    public bool missionCompleted = false; // Estado de la misión (completada o no)

    private GameObject player; // El jugador
    private Icon activeIcon; // El ícono actualmente activo
    private float checkInterval = 0.5f; // Comprobar cada 0.5 segundos
    private float checkTimer = 0f;

    void Start()
    {
        player = GameObject.FindWithTag("Player"); // Encuentra al jugador usando el tag "Player"
    }

    void Update()
    {
        checkTimer += Time.deltaTime;
        if (checkTimer >= checkInterval)
        {
            checkTimer = 0f;

            if (player != null)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);

                if (distance <= detectionRadius)
                {
                    ShowMissionIcon();
                }
                else
                {
                    HideMissionIcon();
                }
            }
        }
    }

    void ShowMissionIcon()
    {
        if (activeIcon == null) // Si no hay ícono activo, obtén uno del pool
        {
            // Cambia esto para asegurarte de que obtienes un Icon y no IPoolableObject
            IPoolableObject poolableObject = iconPool.get(); // Obtén un objeto del pool
            if (poolableObject is Icon icon) // Comprueba si es del tipo Icon
            {
                activeIcon = icon; // Asigna el icono activo
                activeIcon.transform.position = iconPosition.position; // Coloca el ícono en la posición correcta
                activeIcon.bubble.SetActive(true); // Asegúrate de activar la burbuja si es necesario
            }
        }
    }

    void HideMissionIcon()
    {
        if (activeIcon != null)
        {
            activeIcon.bubble.SetActive(false); // Desactiva la burbuja en lugar del ícono
            iconPool.release(activeIcon); // Devuelve el ícono al pool
            activeIcon = null; // Reinicia la referencia al ícono activo
        }
    }

    public void CompleteMission()
    {
        missionCompleted = true;
        if (activeIcon != null)
        {
            activeIcon.CompleteMission();  // Cambia el ícono a la carita feliz dentro del `Bubble`
        }
    }
}
