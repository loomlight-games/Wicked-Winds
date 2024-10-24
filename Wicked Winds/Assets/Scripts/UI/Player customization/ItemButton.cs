using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class ItemButton : MonoBehaviour
{
    public CustomizableItem item;
    TextMeshProUGUI priceText;
    public CustomizableCharacter character; //Player.Instance.customizable
    PlayerCustomizationUI shopUI;
    Button button;
    GameObject pricePanel;

    Color semiTransparentWhite = new (1.0f, 1.0f, 1.0f, 0.5f); // 50% transparent white
    Color semiTransparentRed = new (1f, 0.5f, 0.5f, 0.5f); // 50% transparent red
    Color semiTransparentGreen = new (0.5f, 1f, 0.5f, 0.5f); // 50% transparent gren
    Color semiTransparentBlue = new (0.5f, 0.5f, 1.0f, 0.5f); // 50% transparent blue

    // Start is called before the first frame update
    void Start()
    {
        // Gets the button component
        button = transform.GetComponent<Button>();

        // Finds shop UI
        GameObject UI = GameObject.Find("UI");
        shopUI = UI.GetComponent<PlayerCustomizationUI>();

        // Item must be first child and price panel named like that
        try{
            // Get item and price panel
            item = transform.GetComponentInChildren<CustomizableItem>();
            pricePanel = transform.Find("Price panel").gameObject;
            priceText = pricePanel.transform.GetComponentInChildren<TextMeshProUGUI>();

            //shopUI.OnCoinsChange += CheckItem;
            priceText.text = item.price.ToString();

            // CheckItem();
        } catch {
            Debug.LogError("Item (first child) or price panel is not 'Price panel'");
        }

        // // Character is wearing something in that body part
        // if (character.currentCustomization[item.bodyPart] != null){
        //     // If the item is the one the character is wearing
        //     if (character.currentCustomization[item.bodyPart].name == item.name)
        //         button.image.color = semiTransparentBlue;
        //     else
        //         button.image.color = semiTransparentWhite;
        // }else
        //     button.image.color = semiTransparentWhite;
    }

    // Update is called once per frame
    void Update()
    {
        if (item == null) return;
        if (character == null) return;
        // if (character.currentCustomization[item.bodyPart] == null){
        //     button.image.color = semiTransparentWhite;
        //     return;
        // }

        // Item is purchased
        if(item.isPurchased){
            // Hide price panel
            pricePanel.SetActive(false);

            // Character is wearing smth of that body part
            if (character.currentCustomization[item.bodyPart] != null){
                // Its this item -> blue
                if (character.currentCustomization[item.bodyPart].name == item.name)
                    button.image.color = semiTransparentBlue;
                else // Its not -> white
                    button.image.color = semiTransparentWhite;
            }else
                button.image.color = semiTransparentWhite;
        }else { // Item not purchased
            // Show price panel
            pricePanel.SetActive(true);

            // Enough money to buy it -> green
            if (shopUI.coinsNum >= item.price)
                button.image.color = semiTransparentGreen;
            // Not enough money -> red
            else
                button.image.color = semiTransparentRed;
        }
    }

    public void CheckItem(object sender, int coins){
        CheckItem();
    }

    void CheckItem(){
        // Item is purchased
        if(item.isPurchased){
            // Hide price panel
            pricePanel.SetActive(false);

            // Character is wearing smth of that body part
            if (character.currentCustomization[item.bodyPart] != null){
                // Its this item -> blue
                if (character.currentCustomization[item.bodyPart].name == item.name)
                    button.image.color = semiTransparentBlue;
                else // Its not -> white
                    button.image.color = semiTransparentWhite;
            }else
                button.image.color = semiTransparentWhite;
        }else { // Item not purchased
            // Show price panel
            pricePanel.SetActive(true);

            // Enough money to buy it -> green
            if (shopUI.coinsNum >= item.price)
                button.image.color = semiTransparentGreen;
            // Not enough money -> red
            else
                button.image.color = semiTransparentRed;
        }
    }
}
