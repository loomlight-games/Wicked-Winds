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

            FogManager.Instance.StartFogTransition(FogManager.Instance.startFog, FogManager.Instance.endFog, FogManager.Instance.targetColor); // Activar niebla (hacia blanco)
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !PlayerManager.Instance.potionFog)
        {
            PlayerManager.Instance.playerIsInsideFog = false;
            FogManager.Instance.StartFogTransition(FogManager.Instance.startNoFog, FogManager.Instance.endNoFog, FogManager.Instance.transparentColor); // Desactivar niebla (hacia transparente)
            if (RenderSettings.fogDensity == 0)
            {
                //RenderSettings.fog = false;
            }

        }
    }




}
