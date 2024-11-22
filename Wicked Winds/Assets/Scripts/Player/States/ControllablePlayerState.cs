using System;
using UnityEngine;

public class ControllablePlayerState : AState
{
    public event EventHandler SpeedPotionCollected, FlyPotionCollected;

    public override void Enter()
    {
        PlayerManager.Instance.customizable.LoadCoins();
        PlayerManager.Instance.playerController.Start();
        PlayerManager.Instance.compass.Start();
    }

    public override void Update()
    {
        PlayerManager.Instance.playerController.Update();
        PlayerManager.Instance.compass.Update();
    }

    public override void Exit()
    {

    }

    public override void OnTriggerEnter(Collider other)
    {
        // It's a speed potion
        if (other.gameObject.CompareTag("SpeedPotion"))
            // Any speed amount has been lost
            if (PlayerManager.Instance.playerController.speedPotionValue != 100)
            {
                //Deactivates it
                other.gameObject.SetActive(false);

                // Notifies boostable
                SpeedPotionCollected?.Invoke(this, null);
            }

        // It's a fly high potion
        if (other.gameObject.CompareTag("FlyHighPotion"))
            // Any fly high amount has been lost
            if (PlayerManager.Instance.playerController.flyPotionValue != 100)
            {
                //Deactivates it
                other.gameObject.SetActive(false);

                // Notifies boostable
                FlyPotionCollected?.Invoke(this, null);
            }

        // It's a coin
        if (other.gameObject.CompareTag("Coin"))
        {
            //Deactivates it
            other.gameObject.SetActive(false);

            // Adds coin to player
            PlayerManager.Instance.customizable.coins++;
            PlayerManager.Instance.customizable.SaveCoins();
        }

        if (other.gameObject.TryGetComponent(out PotionFog potion))
        {
            potion.CollectPotionFog();

        }

        if (other.gameObject.TryGetComponent(out teleportPotion potion2))
        {
            potion2.CollectTeleportPotion();

        }
    }

    public override void OnTriggerStay(Collider other)
    {
        // It's an NPC
        if (other.gameObject.TryGetComponent(out Interactable interactable))
            // Interact key is pressed
            if (PlayerManager.Instance.interactKey)
                //Debug.Log("Interacting with interactable");
                // Interact with NPC
                interactable.Interact();

        // It's a mission collectible
        if (other.gameObject.TryGetComponent(out Pickable pickableObject))
            // Interact key is pressed
            if (PlayerManager.Instance.interactKey)
                //Debug.Log("Interacting with collectible");
                // Collects it
                pickableObject.CollectItem();

        if (other.gameObject.TryGetComponent(out InteractableCat cat))
        {
            if (PlayerManager.Instance.interactKey)
            {
                Debug.Log("Interacción con el gato iniciada.");
                cat.InteractCat();
            }
        }

        
    }

}
