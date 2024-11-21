using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionFog : MonoBehaviour
{
    public FogTrigger[] fogs;


    private void Start()
    {
        fogs = FindObjectsOfType<FogTrigger>();
    }

    // Metodo para recolectar el objeto
    public void CollectPotionFog()
    {
        if (fogs.Length > 0)
        {
           Destroy(gameObject);
           PlayerManager.Instance.potionFog = true;
            StartCoroutine(ReenableFogAfterTime(GameManager.Instance.potionFogEffectTime));
        }

    }

    // Coroutine para esperar 30 segundos y luego habilitar de nuevo el prefab
    private IEnumerator ReenableFogAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        PlayerManager.Instance.potionFog = true;
    }
}
