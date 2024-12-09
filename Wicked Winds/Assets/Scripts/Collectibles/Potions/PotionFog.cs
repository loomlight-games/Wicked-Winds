using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionFog : MonoBehaviour
{
    // Metodo para recolectar el objeto
    public void CollectPotionFog()
    {
        // Reproducir el sonido del efecto de la poción

        //SoundManager.Instance.PlayPotionEffect();
        

        // Activar la niebla de la poción
        PlayerManager.Instance.potionFog = true;
      
        // Activar la UI de la niebla
        DesactivarPotionUI.Instance.activarFogUI = true;

        // Reactivar el temporizador de niebla
        FogManager.Instance.ReenableFogAfterTime();
        FogManager.Instance.StartFogTransition(0f, new Color(0f, 0f, 0f, 0f)); // Desactivar la niebla inmediatamente
    }
}
