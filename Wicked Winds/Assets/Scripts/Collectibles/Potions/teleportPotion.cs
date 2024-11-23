using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleportPotion : MonoBehaviour
{
    private SoundManager soundManager;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    public void CollectTeleportPotion()
    {
        if (PlayerManager.Instance.currentTargets.Count>0)
        {
            if(soundManager != null) { soundManager.SelectAudio(4, 0.6f); }
            
            // Obtener la posición del objetivo
            Vector3 targetPosition = PlayerManager.Instance.currentTargets[0].transform.position;

            // Generar un desplazamiento aleatorio
            Vector3 offset = new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));
            Vector3 teleportPosition = targetPosition + offset;

            // Obtener el componente de efecto
            TeleportEffect effect = FindObjectOfType<TeleportEffect>();
            if (effect != null)
            {
                StartCoroutine(effect.FlashAndTeleport(teleportPosition, () =>
                {
                    // Mover al jugador después del efecto
                    PlayerManager.Instance.controller.enabled = false;
                    PlayerManager.Instance.transform.position = teleportPosition;
                    PlayerManager.Instance.controller.enabled = true;

                    // Desactivar la poción
                    gameObject.SetActive(false);
                    GameManager.Instance.playState.feedBackText.text = "Woooooow quicker than my ex";
                }));
            }
        }
        else
        {
            GameManager.Instance.playState.feedBackText.text = "Looks like you don't have anywhere to go!! Get some life perspective ahah";
            // Desactivar la poción
            gameObject.SetActive(false);
        }
    }

}
