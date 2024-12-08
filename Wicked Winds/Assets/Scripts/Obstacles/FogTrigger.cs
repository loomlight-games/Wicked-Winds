using UnityEngine;

public class FogTrigger : MonoBehaviour
{
    
    
    private Collider fogCollider;

    private void Start()
    {
        

        fogCollider = GetComponent<Collider>(); // Obtener el collider del trigger
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !PlayerManager.Instance.potionFog)
        {
            GameManager.Instance.playState.feedBackText.text = "Perfect weather for some ghost stories...";
            FogManager.Instance.StartFogTransition(FogManager.Instance.fogDensity, FogManager.Instance.targetColor); // Activar niebla (hacia blanco)
        }
    }

   
   /* private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !PlayerManager.Instance.potionFog)
        {
            // Esto se ejecuta constantemente mientras el jugador esté dentro del Trigger
            GameManager.Instance.playState.feedBackText.text = "Perfect weather for some ghost stories...";
            StartFogTransition(fogDensity, targetColor); // Activar niebla si es necesario
        }
    }*/

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !PlayerManager.Instance.potionFog)
        {
            GameManager.Instance.playState.feedBackText.text = "Finally some sunlight!";
            FogManager.Instance.StartFogTransition(0f, FogManager.Instance.transparentColor); // Desactivar niebla (hacia transparente)
            RenderSettings.fog = false;
        }
    }



    public void SetFogTriggerState(bool isActive)
    {
        fogCollider.enabled = isActive;
    }


    
}
