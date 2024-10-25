using System;
using UnityEngine;

/// <summary>
/// Implements boost consumption
/// </summary>
public class Boostable
{
    public event EventHandler <float> BoostValueEvent;
    readonly float boostLossPerSecond;
    const float MAX_STAMINA = 100;
    float currentBoost = MAX_STAMINA;

    public Boostable(float boostLossPerSecond)
    {
        this.boostLossPerSecond = boostLossPerSecond;
    }

    ///////////////////////////////////////////////////////////////////////
    public void Start(){
        // Needs to know when its running
        PlayerManager.Instance.movable.RunningEvent += BoostLoss; 
    }

    public void Update(){
        // Sends value every frame
        BoostValueEvent?.Invoke(this, currentBoost);
    }

    public void ResetBoost ()
    {
        currentBoost = MAX_STAMINA;
    }

    /// <summary>
    /// Decreases current value
    /// </summary>
    public void BoostLoss(object sender, EventArgs any)
    {
        currentBoost -= boostLossPerSecond * Time.deltaTime;
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
                currentBoost = MAX_STAMINA;

            //Deactivates it
            other.gameObject.SetActive(false);
        }
    }
}