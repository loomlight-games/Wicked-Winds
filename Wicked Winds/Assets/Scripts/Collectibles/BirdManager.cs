using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BirdManager : MonoBehaviour
{
    public static BirdManager Instance { get; private set; }
    public List<GameObject> flocks; // Lista de p�jaros en la escena
    private bool birdsActive = true;
    private bool isBirdsTimerActive = false;
    float timer;  // Temporizador que se incrementa cada frame
    float potionBirdsEffectTime = 20f;

    public TextMeshProUGUI timerText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);


    }

    private void Start()
    {
        timer = potionBirdsEffectTime;
    }

    public void DeactivateAllBirds()
    {
        if (!birdsActive) return; // Evita desactivar si ya est�n desactivados
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
        if (birdsActive) return; // Evita activar si ya est�n activos
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
            timer -= Time.deltaTime;
            UpdatePotionTimer();

            if (timer <= 0)
            {
                PlayerManager.Instance.potionBird = false;
                DesactivarPotionUI.Instance.activarBirdUI = false;
                timer = 0f;
                isBirdsTimerActive = false;
                // Ocultar y restablecer el texto del temporizador
                timerText.gameObject.SetActive(false);
                timerText.text = "";
                timerText.color = Color.white;
                timer = potionBirdsEffectTime;
                ActivateAllBirds();
            }
        }
    }

    private void UpdatePotionTimer()
    {
        if (timer < potionBirdsEffectTime)
        {
            if (!timerText.gameObject.activeSelf)
            {
                timerText.gameObject.SetActive(true); // Mostrar el texto si está oculto
            }

            timerText.text = Mathf.CeilToInt(timer).ToString(); // Actualizar el texto con el tiempo restante

            if (timer <= 5)
            {
                timerText.color = Color.red;

            }
            else if (timer <= 15)
            {
                timerText.color = Color.yellow;

            }
        }
    }
}
