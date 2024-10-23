using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizableItem : MonoBehaviour
{
    public CustomizableCharacter.BodyPart bodyPart;
    public bool chosen = false;
    public GameObject prefab;

    public  CustomizableItem(GameObject _prefab){
        prefab = _prefab;
    }
}
