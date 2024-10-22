using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public CustomizableItem item;
    Button button;

    Color semiTransparentWhite = new (1.0f, 1.0f, 1.0f, 0.5f); // 50% transparent white
    Color semiTransparentBlue = new (0.5f, 0.5f, 1.0f, 0.5f); // 50% transparent white
    

    // Start is called before the first frame update
    void Start()
    {
        // Gets the item (must be the fist child)
        item = transform.GetChild(0).gameObject.GetComponent<CustomizableItem>();

        // Gets the button
        button = transform.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if(item.chosen)
            button.image.color = semiTransparentBlue;
        else
            button.image.color = semiTransparentWhite;
    }
}
