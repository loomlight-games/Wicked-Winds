using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionFog : MonoBehaviour
{
    // Metodo para recolectar el objeto
    public void CollectPotionFog()
    {
        // Reproducir el sonido del efecto de la pocion
        SoundManager.PlaySound(SoundType.Potion);

        // Activar la niebla de la pocion
        PlayerManager.Instance.potionFog = true;
        // Activar la UI de la niebla
        DesactivarPotionUI.Instance.activarFogUI = true;

        // Reactivar el temporizador de niebla
        FogManager.Instance.ReenableFogAfterTime();
        //FogManager.Instance.StartFogTransition(FogManager.Instance.startNoFog, FogManager.Instance.endNoFog, FogManager.Instance.transparentColor); // Desactivar niebla (hacia transparente)    }
    }
}
