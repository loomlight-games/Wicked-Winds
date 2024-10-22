using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Customizable : MonoBehaviour
{
    public enum BodyPart{
        Head, UpperBody, LowerBody, Shoes
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
        // Current item
        if (customization[item.bodyPart] != null){
            // Not chosen anymore
            customization[item.bodyPart].item.chosen = false;

            // Destroys the current item gameobject
            if (customization[item.bodyPart] != null) 
                Destroy(customization[item.bodyPart].gameObject);
            
            // Delete from bodypart
            customization[item.bodyPart] = null;
        }else{
            Debug.Log(item.transform.name);

            // Instantiates a copy of the item gameobject
            GameObject GOcopy = Instantiate(item.gameObject,transform.position, transform.rotation, transform);
            GOcopy.transform.localScale = new Vector3(1, 1, 1); // Ensure normal scale

            // Add to body part
            customization[item.bodyPart] = new(item,GOcopy);
        }
    }
}
