using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ItemButton : MonoBehaviour
{
    public Garment garment;
    public float rotationSpeed,
                scaleUp = 1.5f;

    TextMeshProUGUI priceText;
    CustomizableCharacter player;
    PlayerCustomizationUI shopUI;
    Button button;
    GameObject pricePanel, center;

    Color semiTransparentWhite = new(1.0f, 1.0f, 1.0f, 0.5f), // 50% transparent white
        semiTransparentRed = new(1f, 0.5f, 0.5f, 0.5f), // 50% transparent red
        semiTransparentGreen = new(0.5f, 1f, 0.5f, 0.5f), // 50% transparent gren
        semiTransparentBlue = new(0.5f, 0.5f, 1.0f, 0.5f); // 50% transparent blue

    // Start is called before the first frame update
    void Start()
    {
        // Gets the button component
        button = transform.GetComponent<Button>();

        // Finds shop UI
        GameObject UI = GameObject.Find("UI");
        shopUI = UI.GetComponent<PlayerCustomizationUI>();

        // Find player
        player = PlayerManager.Instance.customizable;

        // Item must be first child and price panel named like that
        try
        {
            // Get item and price panel
            garment = transform.GetComponentInChildren<Garment>();

            pricePanel = transform.Find("Price panel").gameObject;
            priceText = pricePanel.transform.GetComponentInChildren<TextMeshProUGUI>();
            center = transform.Find("Center").gameObject;

            priceText.text = garment.price.ToString();
        }
        catch
        {
            Debug.LogError("Item is not first child or price panel is not 'Price panel'");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (garment == null) return;
        if (player == null) return;

        if (!player.GarmentIsPurchased(garment))
        {
            // Show price panel
            pricePanel.SetActive(true);

            // Enough money to buy it -> green
            if (shopUI.coinsNum >= garment.price)
                button.image.color = semiTransparentGreen;
            // Not enough money -> red
            else
                button.image.color = semiTransparentRed;
        }
        else // Item is purchased
        {
            // Hide price panel
            pricePanel.SetActive(false);

            // Move item to center
            garment.transform.position = Vector3.MoveTowards(garment.transform.position, center.transform.position, 2f * Time.deltaTime);

            // Character is wearing smth of that body part
            if (player.currentCustomization[garment.bodyPart] != null)
            {
                // Its this item -> blue
                if (player.currentCustomization[garment.bodyPart].tag == garment.tag)
                    button.image.color = semiTransparentBlue;
                else // Its not -> white
                    button.image.color = semiTransparentWhite;
            }
            else
                button.image.color = semiTransparentWhite;
        }

        rotationSpeed = PlayerManager.Instance.rotatorySpeedAtShop;

        // Rotates item
        garment.transform.Rotate(0, 360 * rotationSpeed * Time.deltaTime, 0);
    }
}
