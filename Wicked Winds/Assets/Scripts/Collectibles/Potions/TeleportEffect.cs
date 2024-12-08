using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TeleportEffect : MonoBehaviour
{
    public Canvas canvas; // Canvas que contiene la imagen del efecto
    public float flashDuration = 2f; // Duraci�n del flash
    public Color flashColor = Color.black; // Color del flash

    private Image screenFlash; // Referencia a la imagen para el efecto

    private void Awake()
    {

    }

    private void Start()
    {
        if (canvas != null)
        {
            // Buscar la imagen por etiqueta o nombre
            foreach (Image img in canvas.GetComponentsInChildren<Image>(true))
            {
                if (img.CompareTag("ScreenFlash")) // Aseg�rate de asignar esta etiqueta a la imagen
                {
                    screenFlash = img;
                    break;
                }
            }

            if (screenFlash != null)
            {

                screenFlash.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0); // Asegura que la opacidad sea 0
            }
            else
            {
                Debug.LogError("No se encontro una imagen con la etiqueta 'ScreenFlash' dentro del Canvas.");
            }
        }
        else
        {
            Debug.LogError("Canvas no asignado.");
        }
    }

    public IEnumerator FlashAndTeleport(Vector3 teleportPosition, System.Action onComplete)
    {
        SoundManager.PlaySound(SoundType.Teleport);

        if (screenFlash != null)
        {

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
            Debug.LogError("El efecto de flash no est� configurado correctamente.");
        }
    }
}
