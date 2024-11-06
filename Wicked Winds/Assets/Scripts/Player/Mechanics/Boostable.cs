using System;
using UnityEngine;

/// <summary>
/// Implements boost consumption
/// </summary>
public class Boostable
{
    public event EventHandler <float> BoostValueEvent;
    readonly float boostLossPerSecond;
    const float MAX_VALUE = 100;
    float currentBoost = MAX_VALUE;

    public Boostable(float boostLossPerSecond)
    {
        this.boostLossPerSecond = boostLossPerSecond;
    }

    ///////////////////////////////////////////////////////////////////////
    public void Start(){
        // Needs to know when its running
        PlayerManager.Instance.playerController.RunningEvent += BoostLoss;
        // Needs to know when to recover value
        PlayerManager.Instance.controllableState.RunBoostCollidedEvent += BoostGain;
    }

    public void Update(){
        // Sends value every frame
        BoostValueEvent?.Invoke(this, currentBoost);
    }

    /// <summary>
    /// Decreases current value
    /// </summary>
    public void BoostLoss(object sender, EventArgs any)
    {
        currentBoost -= boostLossPerSecond * Time.deltaTime;
    }

    /// <summary>
    /// Recovers max value
    /// </summary>
    public void BoostGain(object sender, EventArgs any)
    {
        currentBoost = MAX_VALUE;
    }

    /// <summary>
    /// Refills boost when boost object is triggered
    /// </summary>
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RunBoost"))
        {
            //currentBoost += other.GetComponent<RunBoost>().recoverValue;
            //Ensures stamina max
            //if (currentBoost > MAX_STAMINA)
                currentBoost = MAX_VALUE;

            //Deactivates it
            other.gameObject.SetActive(false);
        }
    }
}