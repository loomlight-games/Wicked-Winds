using UnityEngine;

public class FogTrigger : MonoBehaviour
{
    public Color fogColor = Color.gray;
    public float fogDensity = 0.1f;
    public float transitionSpeed = 2f; // Velocidad de transición de la niebla

    private Color targetColor;
    private float targetDensity;

    private void Start()
    {
        // Inicializa los valores de la niebla al entrar al juego
        targetColor = RenderSettings.fogColor;
        targetDensity = RenderSettings.fogDensity;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && PlayerManager.Instance.potionFog == false) // Si el jugador entra en la zona especificada
        {
            Debug.Log("Player entered the fog area.");
            PlayerManager.Instance.playerIsInsideFog = true;
            StartFogTransition(fogColor, fogDensity);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited the fog area.");
            PlayerManager.Instance.playerIsInsideFog = false;
            StartFogTransition(Color.clear, 0f);  // Transición a sin niebla
        }
    }

    // Inicia la transición de la niebla
    private void StartFogTransition(Color targetFogColor, float targetFogDensity)
    {
        targetColor = targetFogColor;
        targetDensity = targetFogDensity;
    }

    private void Update()
    {
        // Solo actualiza la transición si hay un cambio
        if (RenderSettings.fogColor != targetColor || RenderSettings.fogDensity != targetDensity)
        {
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, targetColor, Time.deltaTime * transitionSpeed);
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, targetDensity, Time.deltaTime * transitionSpeed);

            // Si hemos alcanzado los valores objetivos, dejamos de actualizar
            if (Mathf.Approximately(RenderSettings.fogDensity, targetDensity) && RenderSettings.fogColor == targetColor)
            {
                // Si estamos completamente en el valor objetivo, podemos detener la interpolación
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
