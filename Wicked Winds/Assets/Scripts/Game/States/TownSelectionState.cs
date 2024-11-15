using UnityEngine;
using UnityEngine.UI;

public class TownSelectionState : AState
{
    GameObject townSelectionMenu,
            townImage, 
            townImage1, 
            townImage2;

    Button playButton;

    public override void Enter()
    {
        GameManager.Instance.TownSelected += ShowTownImage;

        GameObject UI = GameObject.Find("UI");
        townSelectionMenu = UI.transform.Find("Town selection").gameObject;
        townSelectionMenu.SetActive(true);
        GameObject townInfo = townSelectionMenu.transform.Find("Town info").gameObject;


        playButton = townInfo.transform.Find("Play").GetComponent<Button>();
        townImage = townInfo.transform.Find("Town image").gameObject;
        townImage1 = townInfo.transform.Find("Town image (1)").gameObject;
        townImage2 = townInfo.transform.Find("Town image (2)").gameObject;
    }

    private void ShowTownImage(object sender, string town)
    {
        switch (town){
            case "Summer":
                GameManager.Instance.town = TownGenerator.Town.Summer;
                townImage.SetActive(true);
                townImage1.SetActive(false);
                townImage2.SetActive(false);
                playButton.interactable = true;
                break;
            case "Autumn":
                GameManager.Instance.town = TownGenerator.Town.Autumn;
                townImage.SetActive(false);
                townImage1.SetActive(true);
                townImage2.SetActive(false);
                playButton.interactable = true;
                break;
            case "Winter":
                GameManager.Instance.town = TownGenerator.Town.Winter;
                townImage.SetActive(false);
                townImage1.SetActive(false);
                townImage2.SetActive(true);
                playButton.interactable = true;
                break;
            default:
                break;
        }
    }

    public override void Exit()
    {
        townSelectionMenu.SetActive(false);
    }
}
