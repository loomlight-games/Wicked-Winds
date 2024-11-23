using System.Collections;
using UnityEngine;

/// <summary>
/// Makes object rotate and reappear - its model - after after certain time if wanted
/// </summary>
public class Collectible : MonoBehaviour
{
    public bool reappearable = true, isModelActive;
    public float timeToReappear = 20f, 
                rotationSpeedX = 0.1f, 
                rotationSpeedY = 0.2f, 
                rotationSpeedZ = 0.1f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(360 * rotationSpeedX * Time.deltaTime,
                        360 * rotationSpeedY * Time.deltaTime,
                        360 * rotationSpeedZ * Time.deltaTime);

        isModelActive = transform.GetChild(0).gameObject.activeSelf;
    }

    /// <summary>
    /// Deactivates the object and starts a timer to reappear if applicable.
    /// </summary>
    public void Deactivate()
    {
        Debug.Log("Deactivate");

        if (reappearable)
            StartCoroutine(ReappearAfterTime());
        
        // Deactivate model
        transform.GetChild(0).gameObject.SetActive(false);
    }

    /// <summary>
    /// Coroutine to reactivate the object after a delay.
    /// </summary>
    private IEnumerator ReappearAfterTime()
    {
        Debug.Log("Timer to reappear");
        yield return new WaitForSeconds(timeToReappear);

        // Reactivate model
        transform.GetChild(0).gameObject.SetActive(true);
        Debug.Log("Reappeared");
    }
}
