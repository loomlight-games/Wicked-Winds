using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPotion : MonoBehaviour
{
    private void Awake()
    {

    }

    public void CollectTeleportPotion()
    {
        if (PlayerManager.Instance.currentTargets.Count > 0)
        {
            SoundManager.PlaySound(SoundType.Potion);

            // Obtener la posici�n del objetivo
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
                    // Mover al jugador despu�s del efecto
                    PlayerManager.Instance.controller.enabled = false;
                    PlayerManager.Instance.transform.position = teleportPosition;
                    PlayerManager.Instance.controller.enabled = true;
                }));
            }
        }
    }

}
