using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Customizable : MonoBehaviour
{
    public enum BodyPart{
        Head, UpperBody, LowerBody, Shoes
    }

    public Transform headTransform, upperBodyTransform, lowerBodyTransform, shoesTransform;
    
    public Dictionary<BodyPart, ItemGO> customization = new(){
        {BodyPart.Head, null},
        {BodyPart.UpperBody, null},
        {BodyPart.LowerBody, null},
        {BodyPart.Shoes, null},
    };

    // Start is called before the first frame update
    void Start()
    {
        // Read from memory

    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

    void UpdateBodyPart(CustomizableItem item, Transform transform){
        // Current item not null
        if (customization[item.bodyPart] != null){
            // Different from the given
            if(customization[item.bodyPart].item != item){
                // Not chosen anymore
                customization[item.bodyPart].item.chosen = false;

                // Destroys the current item gameobject
                Destroy(customization[item.bodyPart].gameObject);

                // Instantiates a copy of the item gameobject
                GameObject GOcopy = Instantiate(item.gameObject,transform.position, transform.rotation, transform);
                GOcopy.transform.localScale = new Vector3(1, 1, 1); // Ensure normal scale

                // Add to body part
                customization[item.bodyPart] = new(item,GOcopy);
            }else{
                if (customization[item.bodyPart].item.chosen){
                    // Instantiates a copy of the item gameobject
                    GameObject GOcopy = Instantiate(item.gameObject,transform.position, transform.rotation, transform);
                    GOcopy.transform.localScale = new Vector3(1, 1, 1); // Ensure normal scale

                    // Add to body part
                    customization[item.bodyPart] = new(item,GOcopy);
                }
                else{
                    // Destroys the current item gameobject 
                    Destroy(customization[item.bodyPart].gameObject);
                }   
            }  
        }else{
            // Instantiates a copy of the item gameobject
            GameObject GOcopy = Instantiate(item.gameObject,transform.position, transform.rotation, transform);
            GOcopy.transform.localScale = new Vector3(1, 1, 1); // Ensure normal scale

            // Add to body part
            customization[item.bodyPart] = new(item,GOcopy);
        }
    }
}

public class ItemGO{
    public CustomizableItem item;
    public GameObject gameObject;

    public ItemGO(){
        item = null;
        gameObject = null;
    }

    public ItemGO(CustomizableItem _item, GameObject _GO){
        item = _item;
        gameObject = _GO;
    }
}
