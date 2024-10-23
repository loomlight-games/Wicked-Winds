using System;
using UnityEngine;

[Serializable]
public class CustomizableItem : MonoBehaviour
{
    public CustomizableCharacter.BodyPart bodyPart;
    public GameObject prefab;
    public GameObject instance;
}
