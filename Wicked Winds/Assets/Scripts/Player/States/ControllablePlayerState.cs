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

                        SoundManager.PlaySound(SoundType.Potion);

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

                        SoundManager.PlaySound(SoundType.Potion);

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
                    SoundManager.PlaySound(SoundType.Coin);

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

                Debug.Log("Colisión detectada con un pájaro.");

                // Obtén la dirección opuesta a la colisión para empujar al personaje hacia atrás
                Vector3 pushBackDirection = PlayerManager.Instance.transform.position - other.transform.position;

                // Normaliza la dirección y aplica la fuerza de retroceso utilizando el CharacterController
                pushBackDirection.y = 0; // Asegura que no haya movimiento en el eje Y (no saltar ni caer)

                // Asegúrate de que el CharacterController esté configurado correctamente para aceptar movimientos
                if (PlayerManager.Instance.controller != null)
                {
                    PlayerManager.Instance.controller.Move(pushBackDirection.normalized * PlayerManager.Instance.pushBackForce * Time.deltaTime);
                }
                else
                {
                    Debug.LogWarning("CharacterController no está asignado al PlayerManager.");
                }
                break;

            default:
                break;
        }
    }

    public override void OnTriggerStay(Collider other)
    {
        // Interact key is pressed and game is not in talking state
        if (PlayerManager.Instance.interactKey &&
            PlayerManager.Instance.GetState() != PlayerManager.Instance.talkingState)
        {
            // Player is not facing this collider
            if (!PlayerManager.Instance.playerController.PlayerIsFacing(other.transform))
                return; // Won't do anything if the player is not facing the collider

            // NPC
            if (other.gameObject.TryGetComponent(out NpcController npc))
                npc.Interact();
            // Mission collectible
            else if (other.gameObject.TryGetComponent(out Pickable pickableObject))
                pickableObject.CollectItem();
            // Cat
            else if (other.gameObject.TryGetComponent(out CatController cat))
                cat.Interact();
            // Owl
            else if (other.gameObject.TryGetComponent(out OwlController owl))
                owl.Interact();
        }
    }
}
