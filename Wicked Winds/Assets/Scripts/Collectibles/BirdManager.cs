using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdManager : MonoBehaviour
{
    public static BirdManager Instance { get; private set; }
    public List<GameObject> flocks; // Lista de pájaros en la escena
    private bool birdsActive = true;
    private bool isBirdsTimerActive = false;
    float timer = 0f;  // Temporizador que se incrementa cada frame
    float potionBirdsEffectTime = 20f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);


    }

    public void DeactivateAllBirds()
    {
        if (!birdsActive) return; // Evita desactivar si ya están desactivados
        birdsActive = false;

        foreach (GameObject flock in flocks)
        {
            foreach (Transform child in flock.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void ActivateAllBirds()
    {
        if (birdsActive) return; // Evita activar si ya están activos
        birdsActive = true;

        foreach (GameObject flock in flocks)
        {
            foreach (Transform child in flock.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    public bool AreBirdsActive()
    {
        return birdsActive;
    }

    public void ReenableBirdsAfterTime()
    {
        isBirdsTimerActive = true;
    }

    private void Update()
    {
        if (isBirdsTimerActive)
        {
            timer += Time.deltaTime;
            if (timer >= potionBirdsEffectTime)
            {
                PlayerManager.Instance.potionBird = false;
                DesactivarPotionUI.Instance.activarBirdUI = false;
                timer = 0f;
                isBirdsTimerActive = false;
                ActivateAllBirds();
                GameManager.Instance.playState.feedBackText.text = "Birds again in the sky";

            }
        }





    }
}
