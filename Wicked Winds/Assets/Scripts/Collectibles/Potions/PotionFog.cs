using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionFog : MonoBehaviour
{
    // Metodo para recolectar el objeto
    public void CollectPotionFog()
    {
        // Log para verificar cuando se llama al método
        Debug.Log("CollectPotionFog called");

        // Reproducir el sonido del efecto de la poción
        SoundManager.Instance.PlayPotionEffect();
        Debug.Log("Potion effect sound played");  // Log para el sonido

        // Activar la niebla de la poción
        PlayerManager.Instance.potionFog = true;
        Debug.Log("Potion fog effect activated in PlayerManager");

        // Activar la UI de la niebla
        DesactivarPotionUI.Instance.activarFogUI = true;
        Debug.Log("Potion fog UI activated");

        // Reactivar el temporizador de niebla
        FogManager.Instance.ReenableFogAfterTime();
        Debug.Log("Fog timer re-enabled from PotionFog");
    }
}
