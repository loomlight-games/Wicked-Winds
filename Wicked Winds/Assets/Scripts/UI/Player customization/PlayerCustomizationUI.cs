using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCustomizationUI : MonoBehaviour
{
    public CustomizableCharacter playerCustomizable;

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
        // Flip chosen value if its item
        button.item.chosen = !button.item.chosen;

        // Sends it to the player customization
        playerCustomizable.ChooseItem(button.item);
    }
}
