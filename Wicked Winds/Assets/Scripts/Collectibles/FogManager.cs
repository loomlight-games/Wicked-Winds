using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogManager : MonoBehaviour
{

    public static FogManager Instance; //only one GameManager in the game (singleton)
    public GameObject FogTriggerPrefab;
    public GameObject FogPrefab;
    public float potionFogEffectTime = 2f;
    float timer = 0f;  // Temporizador que se incrementa cada frame
    private bool isFogTimerActive = false;
    // Start is called before the first frame update
    private List<FogTrigger> fogTriggers = new List<FogTrigger>(); // Lista para almacenar todos los triggers de niebla
    public void Awake()
    {
        //if there's not an instance, it creates one - SINGLETON
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);

        // Obtener todos los FogTrigger en la escena y añadirlos a la lista
        fogTriggers.AddRange(FindObjectsOfType<FogTrigger>());
    }

    private void Update()
    {
        if (isFogTimerActive)
        {
            timer += Time.deltaTime;
            if (timer >= potionFogEffectTime)
            {
                PlayerManager.Instance.potionFog = false;
                DesactivarPotionUI.Instance.activarFogUI = false;
                timer = 0f;
                isFogTimerActive = false;
                GameManager.Instance.playState.feedBackText.text = "Back to London time...";

                // Reactivar todos los triggers de niebla
                foreach (var fogTrigger in fogTriggers)
                {
                    fogTrigger.SetFogTriggerState(true); // Activar los triggers después de que la niebla se haya desactivado
                }

                Debug.Log("Potion fog effect ended.");
            }
        }
    }

    public void ReenableFogAfterTime()
    {
        // Este método puede ser llamado después de tomar la poción
        isFogTimerActive = true;
        Debug.Log("Fog timer re-enabled");

        // Desactivar todos los triggers de niebla mientras la poción esté activa
        foreach (var fogTrigger in fogTriggers)
        {
            fogTrigger.SetFogTriggerState(false); // Desactivar los triggers durante la poción
        }
    }
}