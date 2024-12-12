using System;
using UnityEngine;

[Serializable]
public class Garment : MonoBehaviour
{
    public BodyPart bodyPart; // Head, upper body, lower body or shoes
    // public GameObject prefab;  // Reference to the prefab
    // public GameObject instance;  // Instance of the item in the scene
    public int price; // Coins this item costs
    public bool isPurchased = false;
}

// Types of body parts, basically of garments
[Serializable]
public enum BodyPart
{
    Head = 0,
    UpperBody = 1,
    LowerBody = 2,
    Shoes = 3
}