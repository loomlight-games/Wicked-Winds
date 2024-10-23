using System;
using UnityEngine;

[Serializable]
public class CustomizableItem : MonoBehaviour
{
    public CustomizableCharacter.BodyPart bodyPart;
    public bool chosen = false;
    public GameObject prefab;
    public GameObject GO;
    //public string prefabName;
}
