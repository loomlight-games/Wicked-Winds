using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TeleportEffect : MonoBehaviour
{
    public Canvas canvas; // Canvas que contiene la imagen del efecto
    public float flashDuration = 2f; // Duración del flash
    public Color flashColor = Color.black; // Color del flash

    private Image screenFlash; // Referencia a la imagen para el efecto
    private SoundManager soundManager;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    private void Start()
    {
        // Obtener la imagen del Canvas
        if (canvas != null)
        {
            screenFlash = canvas.GetComponentInChildren<Image>(true); // Buscar incluso si está desactivada
            if (screenFlash != null)
            {
                canvas.gameObject.SetActive(false); // Desactiva el Canvas inicialmente
                screenFlash.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0); // Asegura que la opacidad sea 0
            }
            else
            {
                Debug.LogError("No se encontró una imagen dentro del Canvas.");
            }
        }
        else
        {
            Debug.LogError("Canvas no asignado.");
        }
    }

    public IEnumerator FlashAndTeleport(Vector3 teleportPosition, System.Action onComplete)
    {
        soundManager.SelectAudio(4,0.6f);
        if (screenFlash != null)
        {
            // Activa el Canvas y comienza el efecto
            canvas.gameObject.SetActive(true);
            float timer = 0;

            // Parpadeo progresivo
            while (timer < flashDuration)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.PingPong(timer * 2, 1); // Cambiar el alpha
                screenFlash.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
                yield return null;
            }

            // Terminar el efecto de flash
            screenFlash.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0); // Opacidad 0
            canvas.gameObject.SetActive(false); // Desactiva el Canvas para que no obstruya

        
            // Teletransportar al jugador
            onComplete?.Invoke();
        }
        else
        {
            Debug.LogError("El efecto de flash no está configurado correctamente.");
        }
    }
}
