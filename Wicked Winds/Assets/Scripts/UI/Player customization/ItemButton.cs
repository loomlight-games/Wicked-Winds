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
    Button button;
    public TextMeshProUGUI priceText;
    public CustomizableCharacter character; //Player.Instance.customizable

    Color semiTransparentWhite = new (1.0f, 1.0f, 1.0f, 0.5f); // 50% transparent white
    Color semiTransparentBlue = new (0.5f, 0.5f, 1.0f, 0.5f); // 50% transparent blue
    

    // Start is called before the first frame update
    void Start()
    {
        try{
            // Gets the item (must be the fist child)
            item = transform.GetChild(0).gameObject.GetComponent<CustomizableItem>();
        } catch {
            Debug.LogError("The CustomizableItem component is not found on the first child of the transform.");
        }

        // Gets the button
        button = transform.GetComponent<Button>();

        if (character.customization[item.bodyPart] != null){
            // If the item is the one the character is wearing
            if (character.customization[item.bodyPart].name == item.name)
                button.image.color = semiTransparentBlue;
            else
                button.image.color = semiTransparentWhite;
        }

        priceText.text = item.price.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (item == null) return;
        if (character == null) return;
        if (character.customization[item.bodyPart] == null){
            button.image.color = semiTransparentWhite;
            return;
        }

        // If the item is the one the character is wearing
        if (character.customization[item.bodyPart].name == item.name)
            button.image.color = semiTransparentBlue;
        else
            button.image.color = semiTransparentWhite;
    }
}
