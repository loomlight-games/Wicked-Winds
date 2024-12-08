using System.Collections;
using UnityEngine;

public class FogTrigger : MonoBehaviour
{
    public Color fogColor = Color.white;
    public float fogDensity = 0.2f;
    public float transitionSpeed = 0.5f; // Velocidad de transici�n de la niebla

    private Color targetColor;
    private float targetDensity;
    private Collider fogCollider;

    private void Start()
    {
        // Inicializa los valores de la niebla al entrar al juego
        targetColor = RenderSettings.fogColor;
        targetDensity = RenderSettings.fogDensity;
        fogCollider = GetComponent<Collider>();  // Obtener el collider del trigger
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && PlayerManager.Instance.potionFog == false) // Si el jugador entra en la zona especificada
        {
            GameManager.Instance.playState.feedBackText.text = "Perfect weather for some ghost stories...";
            PlayerManager.Instance.playerIsInsideFog = true;

            StartFogTransition(fogColor, fogDensity);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && PlayerManager.Instance.potionFog == false)
        {
            GameManager.Instance.playState.feedBackText.text = "Finally some sunlight!";
            PlayerManager.Instance.playerIsInsideFog = false;
            StartFogTransition(Color.clear, 0f);  // Transici�n a sin niebla
        }
    }

    // Funci�n para activar o desactivar el collider del trigger
    public void SetFogTriggerState(bool isActive)
    {
        fogCollider.enabled = isActive;
    }




    // Inicia la transici�n de la niebla
    private void StartFogTransition(Color targetFogColor, float targetFogDensity)
    {
        targetColor = targetFogColor;
        targetDensity = targetFogDensity;
    }

    private void Update()
    {
        // Si el jugador est� dentro de la niebla y la poci�n ha activado la niebla
        if (PlayerManager.Instance.potionFog && PlayerManager.Instance.playerIsInsideFog == true)
        {
            GameManager.Instance.playState.feedBackText.text = "WOW! The fog has magically disappeared!";
            PlayerManager.Instance.playerIsInsideFog = false;
            StartFogTransition(Color.white, 0f);  // Transici�n a sin niebla (blanco)
        }
        else if (!PlayerManager.Instance.potionFog)
        {
            // Si la poci�n ya no est� activa, restaurar la niebla
            if (!RenderSettings.fog) // Si la niebla est� desactivada
            {
                // Restablecer el color blanco de la niebla
                RenderSettings.fogColor = Color.white;  // Niebla blanca

                // Ajustar el modo de niebla a exponencial
                RenderSettings.fogMode = FogMode.Exponential;  // Nieblas exponenciales

                // Establecer la densidad de la niebla (ajusta el valor para obtener el efecto deseado)
                RenderSettings.fogDensity = 0.05f;  // Cambia este valor seg�n el efecto que busques

                RenderSettings.fog = true;  // Activar la niebla de nuevo

                // Iniciar la transici�n de vuelta a la niebla
                StartFogTransition(RenderSettings.fogColor, RenderSettings.fogDensity);
                Debug.Log("Potion effect ended, fog is restored.");
            }
        }

        // Solo actualiza la transici�n si hay un cambio
        if (RenderSettings.fogColor != targetColor || RenderSettings.fogDensity != targetDensity)
        {
            RenderSettings.fogColor =targetColor;
            RenderSettings.fogDensity = Mathf.Lerp(0, targetDensity, Time.deltaTime * transitionSpeed);

            // Si hemos alcanzado los valores objetivos, dejamos de actualizar
            if (Mathf.Approximately(RenderSettings.fogDensity, targetDensity) && RenderSettings.fogColor == targetColor)
            {
                // Si estamos completamente en el valor objetivo, podemos detener la interpolaci�n
                RenderSettings.fog = targetDensity > 0f; // Solo habilitamos la niebla si la densidad es mayor que 0
            }
        }
        else
        {
            // Asegurarse de que la niebla se active/desactive adecuadamente
            RenderSettings.fog = targetDensity > 0f;
        }
    }

}
