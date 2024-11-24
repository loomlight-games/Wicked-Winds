using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdPotion : MonoBehaviour
{

    // Metodo para recolectar el objeto
    public void CollectBirdPotion()
    {
        SoundManager.Instance.PlayPotionEffect();

        PlayerManager.Instance.potionBird = true;
        DesactivarPotionUI.Instance.activarBirdUI = true; // Muestra la UI de la poci�n

        // Desactivar todos los p�jaros
        StartCoroutine(DeactivateBirdsTemporarily());

        gameObject.SetActive(false); // Desactiva la poci�n
    }

    // Coroutine para desactivar los p�jaros durante 20 segundos
    private IEnumerator DeactivateBirdsTemporarily()
    {
        // Desactivar los p�jaros
        BirdManager.Instance.DeactivateBirds(); // Se asume que hay un BirdManager que controla los p�jaros

        // Espera 20 segundos
        yield return new WaitForSeconds(20f);

        // Reactivar los p�jaros
        BirdManager.Instance.ActivateBirds(); // Reactiva los p�jaros despu�s de 20 segundos

        // Ocultar la UI de la poci�n
        DesactivarPotionUI.Instance.activarBirdUI = false;
        PlayerManager.Instance.potionBird = false;
    }
}
