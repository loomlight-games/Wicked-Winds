using System;
using UnityEngine;

[Serializable]
public class CustomizableItem : MonoBehaviour
{
    public CustomizableCharacter.BodyPart bodyPart; // Head, upper body, lower body or shoes
    public GameObject prefab;  // Reference to the prefab
    public GameObject instance;  // Instance of the item in the scene
    public int price; // Coins this item costs
    public bool isPurchased = false;
}