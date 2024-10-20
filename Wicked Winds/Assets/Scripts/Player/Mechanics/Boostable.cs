using System;
using UnityEngine;

/// <summary>
/// Implements boost consumption
/// </summary>
public class Boostable : MonoBehaviour
{
    public event EventHandler <float> BoostValueEvent;
    public float lossPerSecond = 1f;
    const float MAX_STAMINA = 100;
    public float currentBoost = MAX_STAMINA;

    // TO IMPLEMENT WITH STATECONTROLLER
    // public Boostable(float staminaLoss)
    // {
    //     lossPerSecond = Math.Abs(staminaLoss);// Always positive
    //     currentBoost = MAX_STAMINA;
    // }

    void Awake(){
        Movable movable= GetComponent<Movable>();

        // Needs to know when its running
        movable.RunningEvent += BoostLoss; 
    }

    void Update(){
        // Sends value every frame
        BoostValueEvent?.Invoke(this, currentBoost);
    }

    /// <summary>
    /// Decreases current value
    /// </summary>
    public void BoostLoss(object sender, EventArgs any)
    {
        currentBoost -= lossPerSecond * Time.deltaTime;
    }

    /// <summary>
    /// Refills boost when boost object is triggered
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RunBoost"))
        {
            //currentBoost += other.GetComponent<RunBoost>().recoverValue;
            //Ensures stamina max
            //if (currentBoost > MAX_STAMINA)
                currentBoost = MAX_STAMINA;

            //Deactivates it
            other.gameObject.SetActive(false);
        }
    }
}