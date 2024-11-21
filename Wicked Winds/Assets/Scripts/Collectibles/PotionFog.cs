using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionFog : MonoBehaviour
{

    // Metodo para recolectar el objeto
    public void CollectPotionFog()
    {
        PlayerManager.Instance.potionFog = true;
        GameManager.Instance.ReenableFogAfterTime();

        Destroy(gameObject);
          
        

    }

  
}
