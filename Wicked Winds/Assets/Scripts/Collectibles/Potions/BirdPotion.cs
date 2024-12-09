using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdPotion : MonoBehaviour
{

    // Metodo para recolectar el objeto
    public void CollectBirdPotion()
    {
        SoundManager.PlaySound(SoundType.Potion);

        PlayerManager.Instance.potionBird = true;
        DesactivarPotionUI.Instance.activarBirdUI = true; // Muestra la UI de la poci�n

        BirdManager.Instance.DeactivateAllBirds();

        gameObject.SetActive(false); // Desactiva la pocion
        BirdManager.Instance.ReenableBirdsAfterTime();
    }


}
