using System;
using UnityEngine;

public class ControllablePlayerState : AState
{
    public event EventHandler SpeedPotionCollected, FlyPotionCollected;

    public override void Enter()
    {
        PlayerManager.Instance.customizable.LoadCoins();

        PlayerManager.Instance.playerController.Start();
        //PlayerManager.Instance.boostable.Start();
        PlayerManager.Instance.compass.Start();
    }

    public override void Update()
    {
        PlayerManager.Instance.playerController.Update();
        //PlayerManager.Instance.boostable.Update();
        PlayerManager.Instance.interactions.Update();
        PlayerManager.Instance.compass.Update();
    }

    public override void Exit()
    {
        
    }

    public override void OnTriggerEnter(Collider other)
    {
        // It's a speed potion
        if (other.gameObject.CompareTag("SpeedPotion"))
        {
            // Any speed amount has been lost
            if (PlayerManager.Instance.playerController.speedPotionValue != 100){
                //Deactivates it
                other.gameObject.SetActive(false);

                // Notifies boostable
                SpeedPotionCollected?.Invoke(this, null);
            }
        }

        // It's a fly high potion
        if (other.gameObject.CompareTag("FlyHighPotion"))
        {
            // Any fly high amount has been lost
            if (PlayerManager.Instance.playerController.flyPotionValue != 100){
                //Deactivates it
                other.gameObject.SetActive(false);

                // Notifies boostable
                FlyPotionCollected?.Invoke(this, null);
            }
        }
        // It's a coin
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
