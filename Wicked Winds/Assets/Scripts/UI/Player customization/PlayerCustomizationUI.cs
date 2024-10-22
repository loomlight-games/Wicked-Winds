using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustomizationUI : MonoBehaviour
{
    public Customizable playerCustomizable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Receives the button of the item to choose
    public void ChooseItem(GameObject button){
        // Gets the item (must be the fist child)
        CustomizableItem item = button.transform.GetChild(0).gameObject.
            GetComponent<CustomizableItem>();

        // Sends it to the player customization
        playerCustomizable.ChooseItem(item);
    }
}
