using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionBubble : MonoBehaviour
{
    //ESTO ES PARA MOSTRAR O NO EL BOCADILLO, NO EL EMOJI DE DENTRO
    public AObjectPool<MissionIcon> iconPool; // Referencia al pool genérico
    public Transform iconPosition; // La posición donde quieres que aparezca el ícono
    public float detectionRadius = 3.0f; // El radio de detección para que aparezca el ícono de misión
    public bool missionCompleted = false; // Estado de la misión (completada o no)

    private GameObject player; // El jugador
    private MissionIcon activeIcon; // El ícono actualmente activo
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

    // Muestra el ícono de misión en el bubble
    void ShowMissionIcon()
    {
        if (activeIcon == null) // Si no hay ícono activo, obtén uno del pool
        {
            activeIcon = iconPool.GetObject(); // Obtén un objeto del pool
            if (activeIcon != null)
            {
                activeIcon.transform.position = iconPosition.position; // Coloca el ícono en la posición correcta
                activeIcon.bubble.SetActive(true); // Asegúrate de activar el bubble si es necesario
            }
        }
    }

    // Oculta el ícono de misión cuando el jugador sale del rango
    void HideMissionIcon()
    {
        if (activeIcon != null)
        {
            activeIcon.bubble.SetActive(false); // Desactiva el bubble en lugar del ícono
            iconPool.ReturnObject(activeIcon); // Devuelve el ícono al pool
            activeIcon = null; // Reinicia la referencia al ícono activo
        }
    }

    // Llama a este método para completar la misión y cambiar el ícono
    public void CompleteMission()
    {
        missionCompleted = true;
        if (activeIcon != null)
        {
            activeIcon.CompleteMission();  // Cambia el ícono a la carita feliz dentro del `Bubble`
        }
    }
}
