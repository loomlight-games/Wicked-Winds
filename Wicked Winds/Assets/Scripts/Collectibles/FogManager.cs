using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogManager : MonoBehaviour
{

    public static FogManager Instance; //only one GameManager in the game (singleton)
    public GameObject FogTriggerPrefab;
    public GameObject FogPrefab;
    public float potionFogEffectTime = 2f;
    float timer = 0f;  // Temporizador que se incrementa cada frame
    private bool isFogTimerActive = false;
    // Start is called before the first frame update
    private List<FogTrigger> fogTriggers = new List<FogTrigger>(); // Lista para almacenar todos los triggers de niebla
    public Color targetColor = new Color(0.9f, 0.9f, 0.9f); // Color base de la niebla (blanco)
    public float fogDensity = 0.5f; // Densidad m�xima de la niebla
    public float transitionSpeed = 2f; // Velocidad de transici�n
    public Color transparentColor; // Color completamente transparente

    public float targetFogDensity; // Densidad objetivo
    public Color targetFogColor; // Color objetivo

    public bool isTransitioning;
    public void Awake()
    {
        //if there's not an instance, it creates one - SINGLETON
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);

        // Obtener todos los FogTrigger en la escena y a�adirlos a la lista
        fogTriggers.AddRange(FindObjectsOfType<FogTrigger>());
    }

    private void Start()
    {
        // Establecer el color inicial de la niebla como completamente transparente
        transparentColor = new Color(targetColor.r, targetColor.g, targetColor.b, 0f);
        RenderSettings.fogColor = transparentColor;

        // Asegurarse de que la niebla est� desactivada al inicio
        RenderSettings.fog = false;
        RenderSettings.fogDensity = 0f;
    }

    void ConfigureLinearFog()
    {

        RenderSettings.fogMode = FogMode.Linear; // Configurar el modo de niebla como lineal
        RenderSettings.fogStartDistance = -10f; // Configurar el inicio de la niebla
        RenderSettings.fogEndDistance = 60f; // Configurar el final de la niebla

    }

    private void Update()
    {
        if (isFogTimerActive)
        {
            timer += Time.deltaTime;
            if (timer >= potionFogEffectTime)
            {
                PlayerManager.Instance.potionFog = false;
                DesactivarPotionUI.Instance.activarFogUI = false;
                timer = 0f;
                isFogTimerActive = false;
                FogManager.Instance.StartFogTransition(fogDensity, targetColor);


                // Reactivar todos los triggers de niebla
                foreach (var fogTrigger in fogTriggers)
                {
                    fogTrigger.SetFogTriggerState(true); // Activar los triggers despu�s de que la niebla se haya desactivado

                }

                Debug.Log("Potion fog effect ended.");
            }
        }


        if (PlayerManager.Instance.potionFog)
        {
            RenderSettings.fog = false; // Apagar inmediatamente la niebla
            isTransitioning = false; // Detener transiciones en curso
            RenderSettings.fogDensity = 0f; // Reiniciar la densidad
            RenderSettings.fogColor = transparentColor; // Ajustar el color
            return; // Salir del m�todo
        }

        if (isTransitioning)
        {


            // Transici�n de niebla
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, targetFogDensity, Time.deltaTime * transitionSpeed);
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, targetFogColor, Time.deltaTime * transitionSpeed);



            if (Mathf.Abs(RenderSettings.fogDensity - targetFogDensity) < 0.01f && RenderSettings.fogColor == targetFogColor)
            {
                RenderSettings.fogDensity = targetFogDensity;
                RenderSettings.fogColor = targetFogColor;
                isTransitioning = false;



                if (targetFogDensity == 0f)
                {
                    RenderSettings.fog = false; // Desactivar la niebla si el objetivo es 0

                }
            }
        }


    }

    public void ReenableFogAfterTime()
    {
        isFogTimerActive = true;

        // Desactivar todos los triggers de niebla mientras la poci�n est� activa
        foreach (var fogTrigger in fogTriggers)
        {
            fogTrigger.SetFogTriggerState(false); // Desactivar los triggers durante la poci�n

            // Forzar la desactivaci�n de la niebla en cada trigger
            FogManager.Instance.StartFogTransition(0f, new Color(0f, 0f, 0f, 0f));
        }
    }


    public void StartFogTransition(float newFogDensity, Color newFogColor)
    {
        targetFogDensity = newFogDensity;
        targetFogColor = newFogColor;
        ConfigureLinearFog();
        RenderSettings.fog = true; // Asegurarse de que la niebla est� activa
        isTransitioning = true;
    }
}