using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdPotion : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    // Metodo para recolectar el objeto
    public void CollectBirdPotion()
    {
        if (soundManager != null)
        {
            soundManager.SelectAudio(2, 0.6f);
        }

        PlayerManager.Instance.potionBird = true;
        DesactivarPotionUI.Instance.activarBirdUI = true; // Muestra la UI de la poción

        // Desactivar todos los pájaros
        StartCoroutine(DeactivateBirdsTemporarily());

        gameObject.SetActive(false); // Desactiva la poción
    }

    // Coroutine para desactivar los pájaros durante 20 segundos
    private IEnumerator DeactivateBirdsTemporarily()
    {
        // Desactivar los pájaros
        BirdManager.Instance.DeactivateBirds(); // Se asume que hay un BirdManager que controla los pájaros

        // Espera 20 segundos
        yield return new WaitForSeconds(20f);

        // Reactivar los pájaros
        BirdManager.Instance.ActivateBirds(); // Reactiva los pájaros después de 20 segundos

        // Ocultar la UI de la poción
        DesactivarPotionUI.Instance.activarBirdUI = false;
        PlayerManager.Instance.potionBird = false;
    }
}
