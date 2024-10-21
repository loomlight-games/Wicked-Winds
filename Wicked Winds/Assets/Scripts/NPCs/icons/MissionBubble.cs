using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionBubble : MonoBehaviour
{
    public AObjectPool<MissionIcon> iconPool; // Referencia al pool genérico
    public Transform iconPosition; // La posición donde quieres que aparezca el ícono
    public float detectionRadius = 3.0f; // El radio de detección para que aparezca el ícono de misión
    public bool missionCompleted = false; // Estado de la misión (completada o no)

    private GameObject player; // El jugador
    private MissionIcon activeIcon; // El ícono actualmente activo

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

            // Si el jugador está dentro del rango, muestra el ícono
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

    // Muestra el ícono de misión o la carita feliz según el estado de la misión
    void ShowMissionIcon()
    {
        if (activeIcon == null) // Si no hay ícono activo, obtén uno del pool
        {
            activeIcon = iconPool.GetObject(); // Obtén un objeto del pool
            if (activeIcon != null)
            {
                activeIcon.transform.position = iconPosition.position; // Coloca el ícono en la posición correcta
                SetMissionIcon(); // Establece el ícono adecuado según el estado de la misión
            }
        }
    }

    // Oculta el ícono de misión cuando el jugador sale del rango
    void HideMissionIcon()
    {
        if (activeIcon != null)
        {
            iconPool.ReturnObject(activeIcon); // Devuelve el ícono al pool
            activeIcon = null; // Reinicia la referencia al ícono activo
        }
    }

    // Cambia el ícono según el estado de la misión
    void SetMissionIcon()
    {
        if (missionCompleted)
        {
            activeIcon.iconRenderer.sprite = /* tu sprite de carita feliz */ null;
        }
        else
        {
            activeIcon.iconRenderer.sprite = /* tu sprite de misión */ null;
        }
    }

    // Llama a este método para completar la misión y cambiar el ícono
    public void CompleteMission()
    {
        missionCompleted = true;
        SetMissionIcon(); // Cambia el ícono a la carita feliz
    }
}