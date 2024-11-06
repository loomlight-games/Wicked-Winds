using System;
using UnityEngine;

public class ControllablePlayerState : AState
{
    public event EventHandler RunBoostCollidedEvent;

    public override void Enter()
    {
        PlayerManager.Instance.customizable.LoadCoins();

        PlayerManager.Instance.playerController.Start();
        PlayerManager.Instance.boostable.Start();
        PlayerManager.Instance.compass.Start();
    }

    public override void Update()
    {
        PlayerManager.Instance.playerController.Update();
        PlayerManager.Instance.boostable.Update();
        PlayerManager.Instance.interactions.Update();
        PlayerManager.Instance.compass.Update();
    }

    public override void Exit()
    {
        
    }

    public override void OnTriggerEnter(Collider other)
    {
        //PlayerManager.Instance.boostable.OnTriggerEnter(other);

        // Its a run boost
        if (other.gameObject.CompareTag("RunBoost"))
        {
            //Deactivates it
            other.gameObject.SetActive(false);

            // Notifies boostable
            RunBoostCollidedEvent?.Invoke(this, null);
        }
        // Its a run boost
        else if (other.gameObject.CompareTag("Coin"))
        {
            //Deactivates it
            other.gameObject.SetActive(false);

            // Adds coin to player
            PlayerManager.Instance.customizable.coins++;
            PlayerManager.Instance.customizable.SaveCoins();
        }
    }
}
