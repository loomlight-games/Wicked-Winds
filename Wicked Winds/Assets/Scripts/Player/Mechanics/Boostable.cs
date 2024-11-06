using System;
using UnityEngine;

/// <summary>
/// Implements boost consumption
/// </summary>
public class Boostable
{
    public event EventHandler <float> BoostValueEvent;
    const float MAX_VALUE = 100;
    public float speedPotionValue = MAX_VALUE;
    readonly float speedPotionLossPerSecond;

    public Boostable(float speedPotionLossPerSecond)
    {
        this.speedPotionLossPerSecond = speedPotionLossPerSecond;
    }

    ///////////////////////////////////////////////////////////////////////
    public void Start(){
        // Needs to know when its running
        //PlayerManager.Instance.playerController.SpeedEvent += SpeedPotionLoss;
        // Needs to know when to recover value
        PlayerManager.Instance.controllableState.SpeedPotionCollected += SpeedPotionGain;
    }

    public void Update(){
        // Sends value every frame
        BoostValueEvent?.Invoke(this, speedPotionValue);
    }

    /// <summary>
    /// Decreases current value
    /// </summary>
    public void SpeedPotionLoss(object sender, EventArgs any) {
        speedPotionValue -= speedPotionLossPerSecond * Time.deltaTime;
    }

    /// <summary>
    /// Recovers max value
    /// </summary>
    public void SpeedPotionGain(object sender, EventArgs any)
    {
        speedPotionValue = MAX_VALUE;
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
                speedPotionValue = MAX_VALUE;

            //Deactivates it
            other.gameObject.SetActive(false);
        }
    }
}