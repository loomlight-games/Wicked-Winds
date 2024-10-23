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
    public Dictionary<BodyPart, CustomizableItem> customization = new(){
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
    public void RecognizeBodyPart (CustomizableItem newItem){
        switch(newItem.bodyPart){
            case BodyPart.Head:
                UpdateBodyPart(newItem, headTransform);
                break;
            case BodyPart.UpperBody:
                UpdateBodyPart(newItem, upperBodyTransform);
                break;
            case BodyPart.LowerBody:
                UpdateBodyPart(newItem, lowerBodyTransform);
                break;
            case BodyPart.Shoes:
               UpdateBodyPart(newItem, shoesTransform);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Handles creation and destruction of items 
    /// </summary>
    void UpdateBodyPart(CustomizableItem newItem, Transform bodyPartTransform){
        
        CustomizableItem currentItem = customization[newItem.bodyPart];
        
        // Current item not null
        if (currentItem != null){

            // Different from the given
            if(currentItem != newItem){

                // Not chosen anymore
                currentItem.chosen = false;

                // Destroys the current item gameobject
                Destroy(currentItem.GO);

                InstantiateItem(newItem, bodyPartTransform);
            
            // Same as given
            }else{
                // Current is chosen
                if (currentItem.chosen)
                    InstantiateItem(newItem, bodyPartTransform);

                // Current is not chosen
                else
                    // Destroys the current item gameobject 
                    Destroy(newItem.GO);
            } 
        // Current item is null
        }else
            InstantiateItem(newItem, bodyPartTransform);
    }

    /// <summary>
    /// Instantiates a copy of the item gameobject at the correspondant body transform as a child
    /// </summary>
    void InstantiateItem(CustomizableItem item, Transform bodyPartTransform){
        GameObject GOcopy = Instantiate(item.gameObject,bodyPartTransform.position, bodyPartTransform.rotation, bodyPartTransform);
        GOcopy.transform.localScale = new Vector3(1, 1, 1); // Ensure normal scale
        item.GO = GOcopy;
        customization[item.bodyPart] = item;
    }
}
