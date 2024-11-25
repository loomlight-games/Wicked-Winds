using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogManager : MonoBehaviour
{

    public static FogManager Instance; //only one GameManager in the game (singleton)
    public GameObject FogTriggerPrefab;
    public float potionFogEffectTime = 2f;
    float timer = 0f;  // Temporizador que se incrementa cada frame
    private bool isFogTimerActive = false;
    // Start is called before the first frame update

    public  void Awake()
    {
        //if there's not an instance, it creates one - SINGLETON
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);

       
    }

    private void Update()
    {
        if (isFogTimerActive)
        {
            timer += Time.deltaTime;
            if (timer > 5f) { GameManager.Instance.playState.feedBackText.text = "The potion only lasts 20 seconds you better hurry!"; }
            if (timer >= potionFogEffectTime)
            {
                // Reactivar la niebla y reiniciar el temporizador
                PlayerManager.Instance.potionFog = false;
                DesactivarPotionUI.Instance.activarFogUI = false;
                timer = 0f;
                isFogTimerActive = false;
                GameManager.Instance.playState.feedBackText.text =  "Back to London time...";
            }
        }
    }

    public void ReenableFogAfterTime()
    {
       
        isFogTimerActive = true; // Activar el temporizador
    }
}
