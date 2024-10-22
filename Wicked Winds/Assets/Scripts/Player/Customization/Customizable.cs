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
    
    public Dictionary<BodyPart, GameObject> customization = new(){
        {BodyPart.Head,null},
        {BodyPart.UpperBody,null},
        {BodyPart.LowerBody,null},
        {BodyPart.Shoes,null},
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
        Debug.Log(item.transform.name);

        switch(item.bodyPart){
            case BodyPart.Head:
                UpdateBodyPart(BodyPart.Head, item, headTransform);
                break;
            case BodyPart.UpperBody:
                UpdateBodyPart(BodyPart.UpperBody, item, upperBodyTransform);
                break;
            case BodyPart.LowerBody:
                UpdateBodyPart(BodyPart.LowerBody, item, lowerBodyTransform);
                break;
            case BodyPart.Shoes:
               UpdateBodyPart(BodyPart.Shoes, item, shoesTransform);
                break;
            default:
                break;
        }
    }

    void UpdateBodyPart(BodyPart bodyPart, CustomizableItem item, Transform transform){
        // Destroys the previous item
        if (customization[bodyPart] != null) Destroy(customization[bodyPart]);

        // Instantiates a copy of the item
        GameObject newItem = Instantiate(item.gameObject,transform.position, transform.rotation, transform);
        newItem.transform.localScale = new Vector3(1, 1, 1); // Ensure normal scale

        // Updates dictionary
        customization[item.bodyPart] = newItem;
    }
}
