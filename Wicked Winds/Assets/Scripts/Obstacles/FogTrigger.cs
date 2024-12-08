using UnityEngine;

public class FogTrigger : MonoBehaviour
{
    public Color fogColor = Color.white; // Color de la niebla
    public float fogDensity = 0.2f; // Densidad máxima de la niebla
    public float transitionSpeed = 0.5f; // Velocidad de transición

    private float targetFogDensity; // Densidad objetivo
    private bool isTransitioning;

    private void Start()
    {
        // Asegurarse de que la niebla esté desactivada al inicio
        RenderSettings.fog = false;
        RenderSettings.fogDensity = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !PlayerManager.Instance.potionFog)
        {
            GameManager.Instance.playState.feedBackText.text = "Perfect weather for some ghost stories...";
            StartFogTransition(fogDensity); // Activar niebla
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !PlayerManager.Instance.potionFog)
        {
            GameManager.Instance.playState.feedBackText.text = "Finally some sunlight!";
            StartFogTransition(0f); // Desactivar niebla
        }
    }

    private void StartFogTransition(float newFogDensity)
    {
        RenderSettings.fogColor = fogColor; // Asegurar que el color de la niebla esté establecido
        RenderSettings.fog = true; // Activar la niebla
        targetFogDensity = newFogDensity;
        isTransitioning = true;
    }

    private void Update()
    {
        if (isTransitioning)
        {
            // Interpolación de la densidad de la niebla
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, targetFogDensity, Time.deltaTime * transitionSpeed);

            // Verificar si la densidad está cerca del objetivo
            if (Mathf.Abs(RenderSettings.fogDensity - targetFogDensity) < 0.01f)
            {
                RenderSettings.fogDensity = targetFogDensity;
                isTransitioning = false;

                // Si la densidad objetivo es 0, desactivar la niebla
                if (targetFogDensity == 0f)
                {
                    RenderSettings.fog = false;
                }
            }
        }

        // Manejar caso especial de la poción
        if (PlayerManager.Instance.potionFog)
        {
            GameManager.Instance.playState.feedBackText.text = "WOW! The fog has magically disappeared!";
            StartFogTransition(0f); // Desactivar niebla inmediatamente
        }
    }
}
