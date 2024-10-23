using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomizableCharacter : MonoBehaviour
{
    // Types of body parts, basically of items
    public enum BodyPart{
        Head, UpperBody, LowerBody, Shoes
    }

    // Positions for items GOs
    public Transform headTransform, upperBodyTransform, lowerBodyTransform, shoesTransform;
    
    // Dictionary that maintains the relation between each bodypart and its item with its GO
    public Dictionary<BodyPart, ItemData> customization = new(){
        {BodyPart.Head, null},
        {BodyPart.UpperBody, null},
        {BodyPart.LowerBody, null},
        {BodyPart.Shoes, null},
    };

    /////////////////////////////////////////////////////////////////////////////////////////////
    // Start is called before the first frame update
    void Start()
    {
        // Read from memory

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Given an item updates its correspondant body part
    /// </summary>
    public void ChooseItem (CustomizableItem item){
        switch(item.bodyPart){
            case BodyPart.Head:
                UpdateBodyPart(item, headTransform);
                break;
            case BodyPart.UpperBody:
                UpdateBodyPart(item, upperBodyTransform);
                break;
            case BodyPart.LowerBody:
                UpdateBodyPart(item, lowerBodyTransform);
                break;
            case BodyPart.Shoes:
               UpdateBodyPart(item, shoesTransform);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Handles creation and destruction of items 
    /// </summary>
    void UpdateBodyPart(CustomizableItem item, Transform bodyPartTransform){
        // Current item not null
        if (customization[item.bodyPart] != null){
            // Different from the given
            if(customization[item.bodyPart].item != item){
                // Not chosen anymore
                customization[item.bodyPart].item.chosen = false;

                // Destroys the current item gameobject
                Destroy(customization[item.bodyPart].gameObject);

                // Instantiates a copy of the item gameobject and makes it a child of the body part
                GameObject GOcopy = Instantiate(item.gameObject,bodyPartTransform.position, bodyPartTransform.rotation, bodyPartTransform);
                GOcopy.transform.localScale = new Vector3(1, 1, 1); // Ensure normal scale

                // Add to body part in dictionary
                customization[item.bodyPart] = new(item,GOcopy);
            // Same as given
            }else{
                // Current is chosen
                if (customization[item.bodyPart].item.chosen){
                    // Instantiates a copy of the item gameobject
                    GameObject GOcopy = Instantiate(item.gameObject,bodyPartTransform.position, bodyPartTransform.rotation, bodyPartTransform);
                    GOcopy.transform.localScale = new Vector3(1, 1, 1); // Ensure normal scale

                    // Add to body part
                    customization[item.bodyPart] = new(item,GOcopy);
                }
                // Current is not chosen
                else{
                    // Destroys the current item gameobject 
                    Destroy(customization[item.bodyPart].gameObject);
                }   
            } 
        // Current item is null
        }else{
            GameObject GOcopy = Instantiate(item.gameObject,bodyPartTransform.position, bodyPartTransform.rotation, bodyPartTransform);
            GOcopy.transform.localScale = new Vector3(1, 1, 1); // Ensure normal scale

            customization[item.bodyPart] = new(item,GOcopy);
        }
    }
}
/////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>
/// Stores an item with its game object
/// </summary>
public class ItemData{
    public CustomizableItem item;
    public GameObject gameObject;

    public ItemData(){
        item = null;
        gameObject = null;
    }

    public ItemData(CustomizableItem _item, GameObject _GO){
        item = _item;
        gameObject = _GO;
    }
}
