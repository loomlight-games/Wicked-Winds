using System;
using UnityEngine;

[Serializable]
public class Garment : MonoBehaviour
{
    public BodyPart bodyPart; // Head, upper body, lower body or shoes
    public int price; // Coins this item costs
}

// Types of body parts, basically of garments
[Serializable]
public enum BodyPart
{
    Head = 0,
    UpperBody = 1,
    LowerBody = 2,
    Broom = 3
}