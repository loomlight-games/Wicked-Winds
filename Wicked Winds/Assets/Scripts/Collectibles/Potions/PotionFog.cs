using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionFog : MonoBehaviour
{
    // Metodo para recolectar el objeto
    public void CollectPotionFog()
    {
        SoundManager.Instance.PlaySoundEffect(2);
        
        PlayerManager.Instance.potionFog = true;
        DesactivarPotionUI.Instance.activarFogUI = true;
        
        FogManager.Instance.ReenableFogAfterTime();
    }
}
