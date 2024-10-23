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
    }

    // Update is called once per frame
    void Update()
    {
        coinsNumText.text = coinsNum.ToString();

        if (adPanel.activeSelf || buyCoinsPanel.activeSelf)
            bodyParts.SetActive(false);
        else
            bodyParts.SetActive(true);
    }

    // Receives the button of the item to choose
    public void ChooseItem(ItemButton button){
        // Sends it to the player customization
        playerCustomizable.UpdateBodyPart(button.item);
    }

    public void BuyCoins(){
        coinsNum++;

        bodyParts.SetActive(!bodyParts.activeSelf);
        adPanel.SetActive(false);

        buyCoinsPanel.SetActive(!bodyParts.activeSelf);
    }

    void AddCoins(object sender, int coinsToAdd)
    {
        coinsNum += coinsToAdd;
    }
}
