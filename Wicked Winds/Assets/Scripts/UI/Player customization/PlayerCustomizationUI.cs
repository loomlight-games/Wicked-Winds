using UnityEngine;
using TMPro;
using System;

public class PlayerCustomizationUI : MonoBehaviour
{
    public event EventHandler<int> OnCoinsChange;
    public CustomizableCharacter player; //Player.Instance.customizable
    public TextMeshProUGUI coinsNumText;
    public int coinsNum, lastPage = 1;
    public GameObject bodyParts1, bodyParts2, adPanel, buyCoinsPanel;

    // Start is called before the first frame update
    void Start()
    {
        AdPanel panel = adPanel.GetComponent<AdPanel>();
        panel.EarnCoinsEvent += AddCoins;
        BuyCoinsPanel buyPanel= buyCoinsPanel.GetComponent<BuyCoinsPanel>();
        buyPanel.PayCoinsEvent += AddCoins;

        bodyParts1.SetActive(true);

        // Find player
        player = GameObject.Find("Player").GetComponent<CustomizableCharacter> ();

        coinsNum = player.coins;
    }

    // Update is called once per frame
    void Update()
    {
        coinsNumText.text = coinsNum.ToString();
        
        if (adPanel.activeSelf || buyCoinsPanel.activeSelf){
            bodyParts1.SetActive(false);
            bodyParts2.SetActive(false);

            if (adPanel.activeSelf)
                buyCoinsPanel.SetActive(false);
            else if (buyCoinsPanel.activeSelf)
                adPanel.SetActive(false);
        }else{
            if (lastPage == 1)
                bodyParts1.SetActive(true);
            else
                bodyParts2.SetActive(true);
        }
    }

    /// <summary>
    /// Receives the button of the item to choose
    /// </summary>
    public void ChooseItem(ItemButton button){

        OnCoinsChange?.Invoke(this, coinsNum);

        int itemPrice = button.item.price;

        // Enough money to buy it or is already purchased
        if (coinsNum >= itemPrice || button.item.isPurchased){
            // Not purchased yet
            if (!button.item.isPurchased)
                // Reduces coins number
                coinsNum -= itemPrice;

            // Sends it to the player customization
            player.UpdateBodyPart(button.item);
            player.UpdateCoins(coinsNum);
        }  
        else
            Debug.Log("Not enough coins");
    }

    /// <summary>
    /// Change between bodyparts pages
    /// </summary>
    /// <param name="nextPageNum"></param>
    public void ChangeBodyParts(int nextPageNum){
        lastPage = nextPageNum;

        if (nextPageNum == 1){
            bodyParts1.SetActive(true);
            bodyParts2.SetActive(false);
        }else{
            bodyParts1.SetActive(false);
            bodyParts2.SetActive(true);
        }
    }

    /// <summary>
    /// Increases the given coins
    /// </summary>
    void AddCoins(object sender, int coinsToAdd)
    {
        coinsNum += coinsToAdd;

        coinsNumText.text = coinsNum.ToString();

        player.UpdateCoins(coinsNum);

        OnCoinsChange?.Invoke(this, coinsNum);
    }

    public void Reset(){
        PlayerPrefs.DeleteAll();
        player.Load();
    }
}
