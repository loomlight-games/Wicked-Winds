using System;
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
    public float rotationSpeed = 0.1f,
        scaleUp = 1.5f;

    Vector3 initialItemScale;
    TextMeshProUGUI priceText;
    CustomizableCharacter player; //Player.Instance.customizable
    PlayerCustomizationUI shopUI;
    Button button;
    GameObject pricePanel, center;

    Color semiTransparentWhite = new (1.0f, 1.0f, 1.0f, 0.5f), // 50% transparent white
        semiTransparentRed = new (1f, 0.5f, 0.5f, 0.5f), // 50% transparent red
        semiTransparentGreen = new (0.5f, 1f, 0.5f, 0.5f), // 50% transparent gren
        semiTransparentBlue = new (0.5f, 0.5f, 1.0f, 0.5f); // 50% transparent blue

    // Start is called before the first frame update
    void Start()
    {
        // Gets the button component
        button = transform.GetComponent<Button>();

        // Finds shop UI
        GameObject UI = GameObject.Find("UI");
        shopUI = UI.GetComponent<PlayerCustomizationUI>();

        // Find player
        player = GameObject.Find("Player").GetComponent<CustomizableCharacter> ();
        
        // Item must be first child and price panel named like that
        try{
            // Get item and price panel
            item = transform.GetComponentInChildren<CustomizableItem>();
            initialItemScale = item.transform.localScale;

            pricePanel = transform.Find("Price panel").gameObject;
            priceText = pricePanel.transform.GetComponentInChildren<TextMeshProUGUI>();
            center = transform.Find("Center").gameObject;

            priceText.text = item.price.ToString();
        } catch {
            Debug.LogError("Item is not first child or price panel is not 'Price panel'");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (item == null) return;
        if (player == null) return;

        // Item is purchased
        if(item.isPurchased){
            // Hide price panel
            pricePanel.SetActive(false);

            // Move item to center
            item.transform.position = Vector3.MoveTowards(item.transform.position, center.transform.position, 2f * Time.deltaTime);

            // Scale it up a little
            item.transform.localScale = Vector3.Lerp(item.transform.localScale, initialItemScale * scaleUp, 2f * Time.deltaTime);

            // Character is wearing smth of that body part
            if (player.currentCustomization[item.bodyPart] != null){
                // Its this item -> blue
                if (player.currentCustomization[item.bodyPart].name == item.name)
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

        try{
             // Check if item is in the the purchased items list of player
            foreach (CustomizableItem purchasedItem in player.purchasedItems){
                if (purchasedItem.name == item.name) item.isPurchased = true;
            }
        }catch{
            Debug.LogWarning("No purchased items");
        }

        // Rotates item
        item.transform.Rotate(0, 360 * rotationSpeed * Time.deltaTime, 0);
    }
}
