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
        PlayerManager.Instance.playerAnimator.Update();
    }

    public override void OnTriggerEnter(Collider other)
    {
        // Attempt to get the Collectible component
        Collectible collectible = other.GetComponent<Collectible>();

        // Switch based on the tag of the collided object
        switch (other.gameObject.tag)
        {
            case "SpeedPotion":
                // Check if the player has lost any speed amount
                if (PlayerManager.Instance.playerController.speedPotionValue != PlayerManager.Instance.MAX_VALUE)
                {
                    // If the object has a Collectible component, deactivate it if not deactivated already
                    if (collectible != null && collectible.isModelActive)
                    {
                        collectible.Deactivate();

                        SoundManager.Instance.PlayPotionEffect();

                        // Notify that a speed potion was collected
                        SpeedPotionCollected?.Invoke(this, null);
                    }
                }
                break;

            case "FlyHighPotion":
                // Check if the player has lost any fly high amount
                if (PlayerManager.Instance.playerController.flyPotionValue != PlayerManager.Instance.MAX_VALUE)
                {
                    // If the object has a Collectible component, deactivate it if not deactivated already
                    if (collectible != null && collectible.isModelActive)
                    {
                        collectible.Deactivate();

                        SoundManager.Instance.PlayPotionEffect();

                        // Notify that a fly high potion was collected
                        FlyPotionCollected?.Invoke(this, null);
                    }
                }
                break;

            case "Coin":
                // If the object has a Collectible component, deactivate it if not deactivated already
                if (collectible != null && collectible.isModelActive)
                {
                    collectible.Deactivate();
                    SoundManager.Instance.PlayCoinEffect();

                    // Increment the player's coin count and save it
                    PlayerManager.Instance.customizable.coins++;
                    PlayerManager.Instance.customizable.SaveCoins();
                }
                break;

            case "FogPotion":
                // If the object has a Collectible component, deactivate it if not deactivated already
                if (collectible != null && collectible.isModelActive)
                {
                    collectible.Deactivate();

                    if (other.gameObject.TryGetComponent(out PotionFog fogPotion))
                        fogPotion.CollectPotionFog();
                }
                break;

            case "TeleportPotion":
                // If the object has a Collectible component, deactivate it if not deactivated already
                if (collectible != null && collectible.isModelActive)
                {
                    collectible.Deactivate();

                    if (other.gameObject.TryGetComponent(out TeleportPotion teleportPotion))
                    {
                        teleportPotion.CollectTeleportPotion();

                    }
                }
                break;

            case "BirdsPotion":
                // If the object has a Collectible component, deactivate it if not deactivated already
                if (collectible != null && collectible.isModelActive)
                {
                    collectible.Deactivate();

                    if (other.gameObject.TryGetComponent(out BirdPotion birdsPotion))
                        birdsPotion.CollectBirdPotion();
                }
                break;
            case "Bird":
                // Obt�n la direcci�n opuesta a la colisi�n para empujar al personaje hacia atr�s
                Vector3 pushBackDirection = PlayerManager.Instance.transform.position - other.transform.position;

                // Normaliza la direcci�n y aplica la fuerza de retroceso utilizando el CharacterController
                pushBackDirection.y = 0; // Asegura que no haya movimiento en el eje Y (no saltar ni caer)

                // Aplicamos el retroceso
                PlayerManager.Instance.controller.Move(pushBackDirection.normalized * PlayerManager.Instance.pushBackForce * Time.deltaTime);
                break;
            default:
                break;
        }
    }

    public override void OnTriggerStay(Collider other)
    {
        // It's an NPC
        if (other.gameObject.TryGetComponent(out Interactable interactable))
        {
            // Interact key is pressed
            if (PlayerManager.Instance.interactKey)
            {
                Debug.Log("Interacting with interactable");
                // Interact with NPC
                interactable.Interact();
            }
        }
        // It's a mission collectible
        else if (other.gameObject.TryGetComponent(out Pickable pickableObject))
        {
            // Interact key is pressed
            if (PlayerManager.Instance.interactKey)
                // Collects it
                pickableObject.CollectItem();
        }
        else if (other.gameObject.TryGetComponent(out InteractableCat cat))
        {
            if (PlayerManager.Instance.interactKey)
            {
                GameManager.Instance.playState.feedBackText.text = "This cat is LOUDDDD";

                cat.InteractCat();
            }
        }
        else if (other.gameObject.TryGetComponent(out InteractableOwl owl))
        {
            GameManager.Instance.playState.feedBackText.text = "Gotcha! You can run, but you can't hide from me!";

            GameManager.Instance.playState.feedBackText.text = "A wise owl said something to me";
            //Deactivates it
            owl.Interact();

        }
    }
}
