using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public void ChooseItem(ItemButton button){
        // Gets the item (must be the fist child)
        CustomizableItem item = button.item;

        // Flip chosen value
        item.chosen = !item.chosen;

        // Sends it to the player customization
        playerCustomizable.ChooseItem(item);
    }
}
