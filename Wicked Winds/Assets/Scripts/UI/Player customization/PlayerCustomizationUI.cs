using UnityEngine;
using TMPro;

public class PlayerCustomizationUI : MonoBehaviour
{
    public CustomizableCharacter playerCustomizable; //Player.Instance.customizable

    public TextMeshProUGUI coinsNumText;
    public int coinsNum;
    public GameObject bodyParts, adPanel, buyCoinsPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        coinsNumText.text = coinsNum.ToString();
    }

    // Receives the button of the item to choose
    public void ChooseItem(ItemButton button){
        // Sends it to the player customization
        playerCustomizable.UpdateBodyPart(button.item);
    }

    public void WatchAd(){
        coinsNum++;

        bodyParts.SetActive(false);
        buyCoinsPanel.SetActive(false);

        adPanel.SetActive(true);
    }

    public void BuyCoins(){
        coinsNum++;
        
        bodyParts.SetActive(false);
        adPanel.SetActive(false);

        buyCoinsPanel.SetActive(true);
    }

    public void ClosePanel (GameObject panel){
        panel.SetActive(false);
        bodyParts.SetActive(true);
    }
}
