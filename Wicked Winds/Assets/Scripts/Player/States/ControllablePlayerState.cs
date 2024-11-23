using System;
using Unity.VisualScripting;
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
        if (other.gameObject.CompareTag("SpeedPotion") && other.gameObject.TryGetComponent(out speedPotion potion4))
            // Any speed amount has been lost
            if (PlayerManager.Instance.playerController.speedPotionValue != 100)
            {
                //Deactivates it
                other.gameObject.SetActive(false);
                potion4.reproduceSound();
                // Notifies boostable
                SpeedPotionCollected?.Invoke(this, null);
            }

        // It's a fly high potion
        if (other.gameObject.CompareTag("FlyHighPotion") && other.gameObject.TryGetComponent(out flyPotionHigh potion3))
            // Any fly high amount has been lost
            if (PlayerManager.Instance.playerController.flyPotionValue != 100)
            {
                //Deactivates it
                other.gameObject.SetActive(false);
                potion3.reproduceSound();
                // Notifies boostable
                FlyPotionCollected?.Invoke(this, null);
            }

        // It's a coin
        if (other.gameObject.CompareTag("Coin"))
        // Attempt to get the Collectible component
        Collectible collectible = other.GetComponent<Collectible>();

        // Switch based on the tag of the collided object
        switch (other.gameObject.tag)
        {
            case "SpeedPotion":
                // Check if the player has lost any speed amount
                if (PlayerManager.Instance.playerController.speedPotionValue != PlayerManager.Instance.MAX_VALUE){
                    // If the object has a Collectible component, deactivate it if not deactivated already
                    if (collectible != null && collectible.isModelActive){
                        collectible.Deactivate();
                    
                        // Notify that a speed potion was collected
                        SpeedPotionCollected?.Invoke(this, null);
                    }
                }
                break;

            case "FlyHighPotion":
                // Check if the player has lost any fly high amount
                if (PlayerManager.Instance.playerController.flyPotionValue != PlayerManager.Instance.MAX_VALUE){
                    // If the object has a Collectible component, deactivate it if not deactivated already
                    if (collectible != null && collectible.isModelActive){
                        collectible.Deactivate();
                        

                        // Notify that a fly high potion was collected
                        FlyPotionCollected?.Invoke(this, null);
                    }
                }
                break;

            case "Coin":
                // If the object has a Collectible component, deactivate it if not deactivated already
                if (collectible != null && collectible.isModelActive){
                    collectible.Deactivate();
                        
                    // Increment the player's coin count and save it
                    PlayerManager.Instance.customizable.coins++;
                    PlayerManager.Instance.customizable.SaveCoins();
                }
                break;

            default:
                break;
        }

        if (other.gameObject.TryGetComponent(out PotionFog potion))
        {
            potion.CollectPotionFog();

        }

        if (other.gameObject.TryGetComponent(out teleportPotion potion2))
        {
            potion2.CollectTeleportPotion();

        }

        //ALBAAA AQUIIIII
        /*
        if(other.gameObject.TryGetComponent(out birdPotion potion4))
        {
            potion4.CollectbirdPotion();
        } */
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

                // Collects it
                pickableObject.CollectItem();

        if (other.gameObject.TryGetComponent(out InteractableCat cat))
        {
            if (PlayerManager.Instance.interactKey)
            {
                GameManager.Instance.playState.feedBackText.text = "This cat is LOUDDDD";

                cat.InteractCat();
            }
        }

        if (other.gameObject.TryGetComponent(out InteractableOwl owl))
        {
            GameManager.Instance.playState.feedBackText.text = "Gotcha! You can run, but you can't hide from me!";
            if (PlayerManager.Instance.interactKey)
            {
                GameManager.Instance.playState.feedBackText.text = "A wise owl said something to me";
                //Deactivates it
                owl.Interact();
            }
        }


    }

}
