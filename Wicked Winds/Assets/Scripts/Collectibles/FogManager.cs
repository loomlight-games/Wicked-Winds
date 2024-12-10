using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FogManager : MonoBehaviour
{

    public static FogManager Instance; //only one GameManager in the game (singleton)
    public GameObject FogTriggerPrefab;
    public GameObject FogPrefab;
    public float potionFogEffectTime = 20f;
    public float timer;  // Temporizador que se incrementa cada frame
    private bool isFogTimerActive = false;
    // Start is called before the first frame update
    private List<FogTrigger> fogTriggers = new List<FogTrigger>(); // Lista para almacenar todos los triggers de niebla
    public Color targetColor = new Color(0.9f, 0.9f, 0.9f); // Color base de la niebla (blanco)
    public float fogDensity = 0.5f; // Densidad maxima de la niebla
    public float transitionSpeed = 2f; // Velocidad de transicion
    public Color transparentColor; // Color completamente transparente

    public float targetFogDensity; // Densidad objetivo
    public Color targetFogColor; // Color objetivo

    public bool isTransitioning;

    public TextMeshProUGUI timerText;
   

    public void Awake()
    {
        //if there's not an instance, it creates one - SINGLETON
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);

        // Obtener todos los FogTrigger en la escena y anadirlos a la lista
        fogTriggers.AddRange(FindObjectsOfType<FogTrigger>());
    }

    private void Start()
    {
        timer = potionFogEffectTime;
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
        if (PlayerManager.Instance.playerIsInsideFog && !PlayerManager.Instance.potionFog)
        {
            StartFogTransition(fogDensity, targetColor);
        }

        if (isFogTimerActive)
        {
            
            timer -= Time.deltaTime;
            UpdatePotionTimer();

            if (timer <= 0)
            {
                PlayerManager.Instance.potionFog = false;
                DesactivarPotionUI.Instance.activarFogUI = false;
                timer = 0f;
                isFogTimerActive = false;
                
                

                // Ocultar y restablecer el texto del temporizador
                timerText.gameObject.SetActive(false);
                timerText.text = "";
                timerText.color = Color.white;
                timer = potionFogEffectTime;

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


            // Transicion de niebla
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

    private void UpdatePotionTimer()
    {
        if (timer < potionFogEffectTime)
        {
            if (!timerText.gameObject.activeSelf)
            {
                timerText.gameObject.SetActive(true); // Mostrar el texto si está oculto
            }

            timerText.text = Mathf.CeilToInt(timer).ToString(); // Actualizar el texto con el tiempo restante

            if (timer <= 5)
            {
                timerText.color = Color.red;
               
            }
            else if (timer <= 15)
            {
                timerText.color = Color.yellow;
                
            }
        }
    }

    public void ReenableFogAfterTime()
    {
        isFogTimerActive = true;

        
        
            FogManager.Instance.StartFogTransition(0f, new Color(0f, 0f, 0f, 0f));
        
    }


    public void StartFogTransition(float newFogDensity, Color newFogColor)
    {
        targetFogDensity = newFogDensity;
        targetFogColor = newFogColor;               
        ConfigureLinearFog();
        RenderSettings.fog = true; // Asegurarse de que la niebla esta activa
        isTransitioning = true;
    }
}