using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudCollider : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CloudPool.Instance.ChangePlayerSpeed(true);
            Debug.LogWarning("ESTA DEBAJO DE LA NUBE");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CloudPool.Instance.ChangePlayerSpeed(false);
            Debug.Log("Player has exited the cloud.");
        }
        Debug.LogWarning("ESTA SALIENDO DE LA NUBE");
    }
}
