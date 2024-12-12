using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FogManager : MonoBehaviour
{

    public static FogManager Instance; //only one GameManager in the game (singleton)
    public GameObject FogTriggerPrefab;
    public float potionFogEffectTime = 20f;
    public float timer;  // Temporizador que se incrementa cada frame
    private bool isFogTimerActive = false;
    // Start is called before the first frame update
    private List<FogTrigger> fogTriggers = new List<FogTrigger>(); // Lista para almacenar todos los triggers de niebla
    public Color targetColor = new Color(0.8f, 0.8f, 0.8f); // Color base de la niebla (blanco)
    public float fogDensity = 0.3f; // Densidad maxima de la niebla
    public float transitionSpeed = 1f; // Velocidad de transicion
    public Color transparentColor; // Color completamente transparente

    public float targetStart;
    public float targetEnd;

    public float startNoFog = 0;
    public float endNoFog = 100000;
    public float startFog = -10;
    public float endFog = 35;
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
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Linear; // Configurar el modo de niebla como lineal
        RenderSettings.fogStartDistance = 0f; // Configurar el inicio de la niebla
        RenderSettings.fogEndDistance = 100000f; // Configurar el final de la niebla

    }

    private void Update()
    {
        if (PlayerManager.Instance.potionFog)
        {
            RenderSettings.fogStartDistance = 0f; // Configurar el inicio de la niebla
            RenderSettings.fogEndDistance = 100000f;
        }
        

        if (isFogTimerActive)
        {

            timer -= Time.deltaTime;


            if (timer <= 0)
            {
                if (PlayerManager.Instance.playerIsInsideFog == true && PlayerManager.Instance.potionFog == false)
                {
                    StartFogTransition(startFog, endFog, targetColor); // Activar niebla (hacia blanco)
                }
                PlayerManager.Instance.potionFog = false;
                DesactivarPotionUI.Instance.activarFogUI = false;
                timer = 0f;
                // Ocultar y restablecer el texto del temporizador
                timerText.gameObject.SetActive(false);
                timerText.text = "";
                timerText.color = Color.white;
                timer = potionFogEffectTime;

                Debug.Log("Potion fog effect ended.");
                isFogTimerActive = false;
            }

            UpdatePotionTimer();
        }




        if (isTransitioning)
        {
            // Transicion de niebla
            RenderSettings.fogStartDistance = Mathf.Lerp(RenderSettings.fogStartDistance, targetStart, Time.deltaTime * transitionSpeed);
            RenderSettings.fogEndDistance = Mathf.Lerp(RenderSettings.fogEndDistance, targetEnd, Time.deltaTime * transitionSpeed);
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, targetFogColor, Time.deltaTime * transitionSpeed);
            if(RenderSettings.fogColor== targetFogColor) { isTransitioning = false; }
           
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
    }


    public void StartFogTransition(float start, float end, Color newFogColor)
    {
        targetStart = start;
        targetEnd = end;
        targetFogColor = newFogColor;
        isTransitioning = true;
    }
}