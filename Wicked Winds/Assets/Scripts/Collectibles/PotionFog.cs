using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionFog : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    // Metodo para recolectar el objeto
    public void CollectPotionFog()
    {
        if (soundManager != null) {
            soundManager.SelectAudio(2, 0.6f);
        }
        
        PlayerManager.Instance.potionFog = true;
        DesactivarPotionUI.Instance.activarFogUI = true;
        FogManager.Instance.ReenableFogAfterTime();

        gameObject.SetActive(false);



    }

  
}
