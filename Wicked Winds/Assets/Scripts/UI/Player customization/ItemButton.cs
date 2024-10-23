using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class ItemButton : MonoBehaviour
{
    public CustomizableItem item;
    Button button;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (item == null) return;

        if(item.chosen)
            button.image.color = semiTransparentBlue;
        else
            button.image.color = semiTransparentWhite;
    }
}
