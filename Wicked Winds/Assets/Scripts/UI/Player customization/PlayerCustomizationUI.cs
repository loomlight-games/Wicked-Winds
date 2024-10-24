using UnityEngine;
using TMPro;
using System;

public class PlayerCustomizationUI : MonoBehaviour
{
    public CustomizableCharacter playerCustomizable; //Player.Instance.customizable

    public TextMeshProUGUI coinsNumText;
    public int coinsNum;
    public GameObject bodyParts, adPanel, buyCoinsPanel;

    // Start is called before the first frame update
    void Start()
    {
        AdPanel panel = adPanel.GetComponent<AdPanel>();
        panel.EarnCoinsEvent += AddCoins;
        BuyCoinsPanel buyPanel= buyCoinsPanel.GetComponent<BuyCoinsPanel>();
        buyPanel.PayCoinsEvent += AddCoins;
    }

    // Update is called once per frame
    void Update()
    {
        if (adPanel.activeSelf || buyCoinsPanel.activeSelf){
            bodyParts.SetActive(false);

            if (adPanel.activeSelf)
                buyCoinsPanel.SetActive(false);
            else if (buyCoinsPanel.activeSelf)
                adPanel.SetActive(false);
        }  
        else
            bodyParts.SetActive(true);
    }

    // Receives the button of the item to choose
    public void ChooseItem(ItemButton button){
        // Sends it to the player customization
        playerCustomizable.UpdateBodyPart(button.item);
    }

    void AddCoins(object sender, int coinsToAdd)
    {
        coinsNum += coinsToAdd;

        coinsNumText.text = coinsNum.ToString();
    }
}
