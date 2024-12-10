using UnityEngine;

public class FogTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager.Instance.playerIsInsideFog = true;
        }
        if (other.CompareTag("Player") && !PlayerManager.Instance.potionFog)
        {
            
            FogManager.Instance.StartFogTransition(FogManager.Instance.fogDensity, FogManager.Instance.targetColor); // Activar niebla (hacia blanco)
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !PlayerManager.Instance.potionFog)
        {
            PlayerManager.Instance.playerIsInsideFog = false;
            FogManager.Instance.StartFogTransition(0f, FogManager.Instance.transparentColor); // Desactivar niebla (hacia transparente)
            RenderSettings.fog = false;
        }
    }




}
