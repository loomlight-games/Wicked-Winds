using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionBubble : MonoBehaviour
{
    //ESTO ES PARA MOSTRAR O NO EL BOCADILLO, NO EL EMOJI DE DENTRO
    public AObjectPool<MissionIcon> iconPool; // Referencia al pool gen�rico
    public Transform iconPosition; // La posici�n donde quieres que aparezca el �cono
    public float detectionRadius = 3.0f; // El radio de detecci�n para que aparezca el �cono de misi�n
    public bool missionCompleted = false; // Estado de la misi�n (completada o no)

    private GameObject player; // El jugador
    private MissionIcon activeIcon; // El �cono actualmente activo
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

    // Muestra el �cono de misi�n en el bubble
    void ShowMissionIcon()
    {
        if (activeIcon == null) // Si no hay �cono activo, obt�n uno del pool
        {
            activeIcon = iconPool.GetObject(); // Obt�n un objeto del pool
            if (activeIcon != null)
            {
                activeIcon.transform.position = iconPosition.position; // Coloca el �cono en la posici�n correcta
                activeIcon.bubble.SetActive(true); // Aseg�rate de activar el bubble si es necesario
            }
        }
    }

    // Oculta el �cono de misi�n cuando el jugador sale del rango
    void HideMissionIcon()
    {
        if (activeIcon != null)
        {
            activeIcon.bubble.SetActive(false); // Desactiva el bubble en lugar del �cono
            iconPool.ReturnObject(activeIcon); // Devuelve el �cono al pool
            activeIcon = null; // Reinicia la referencia al �cono activo
        }
    }

    // Llama a este m�todo para completar la misi�n y cambiar el �cono
    public void CompleteMission()
    {
        missionCompleted = true;
        if (activeIcon != null)
        {
            activeIcon.CompleteMission();  // Cambia el �cono a la carita feliz dentro del `Bubble`
        }
    }
}
