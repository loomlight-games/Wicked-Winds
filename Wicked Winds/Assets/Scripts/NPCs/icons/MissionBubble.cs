using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionBubble : MonoBehaviour
{
    public AObjectPool<MissionIcon> iconPool; // Referencia al pool gen�rico
    public Transform iconPosition; // La posici�n donde quieres que aparezca el �cono
    public float detectionRadius = 3.0f; // El radio de detecci�n para que aparezca el �cono de misi�n
    public bool missionCompleted = false; // Estado de la misi�n (completada o no)

    private GameObject player; // El jugador
    private MissionIcon activeIcon; // El �cono actualmente activo

    void Start()
    {
        player = GameObject.FindWithTag("Player"); // Encuentra al jugador usando el tag "Player"
    }

    void Update()
    {
        if (player != null)
        {
            // Distancia entre el jugador y el NPC
            float distance = Vector3.Distance(transform.position, player.transform.position);

            // Si el jugador est� dentro del rango, muestra el �cono
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

    // Muestra el �cono de misi�n o la carita feliz seg�n el estado de la misi�n
    void ShowMissionIcon()
    {
        if (activeIcon == null) // Si no hay �cono activo, obt�n uno del pool
        {
            activeIcon = iconPool.GetObject(); // Obt�n un objeto del pool
            if (activeIcon != null)
            {
                activeIcon.transform.position = iconPosition.position; // Coloca el �cono en la posici�n correcta
                SetMissionIcon(); // Establece el �cono adecuado seg�n el estado de la misi�n
            }
        }
    }

    // Oculta el �cono de misi�n cuando el jugador sale del rango
    void HideMissionIcon()
    {
        if (activeIcon != null)
        {
            iconPool.ReturnObject(activeIcon); // Devuelve el �cono al pool
            activeIcon = null; // Reinicia la referencia al �cono activo
        }
    }

    // Cambia el �cono seg�n el estado de la misi�n
    void SetMissionIcon()
    {
        if (missionCompleted)
        {
            activeIcon.iconRenderer.sprite = /* tu sprite de carita feliz */ null;
        }
        else
        {
            activeIcon.iconRenderer.sprite = /* tu sprite de misi�n */ null;
        }
    }

    // Llama a este m�todo para completar la misi�n y cambiar el �cono
    public void CompleteMission()
    {
        missionCompleted = true;
        SetMissionIcon(); // Cambia el �cono a la carita feliz
    }
}