using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TownSelectionState : AState
{
    GameObject townSelectionMenu, 
            advice, 
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
        advice = townInfo.transform.Find("Advice").gameObject;
        townImage = townInfo.transform.Find("Town image").gameObject;
        townImage1 = townInfo.transform.Find("Town image (1)").gameObject;
        townImage2 = townInfo.transform.Find("Town image (2)").gameObject;
    }

    private void ShowTownImage(object sender, string town)
    {
        //advice.SetActive(false);
        townImage.SetActive(false);
        townImage1.SetActive(false);
        townImage2.SetActive(false);

        switch (town){
            case "Summer":
                GameManager.Instance.town = TownGenerator.Town.Summer;
                townImage.SetActive(true);
                playButton.interactable = true;
                break;
            case "Autumn":
                GameManager.Instance.town = TownGenerator.Town.Autumn;
                townImage1.SetActive(true);
                playButton.interactable = true;
                break;
            case "Winter":
                GameManager.Instance.town = TownGenerator.Town.Winter;
                townImage2.SetActive(true);
                playButton.interactable = true;
                break;
            default:
                //advice.SetActive(true);
                //playButton.interactable = false;
                break;
        }
    }

    public override void Exit()
    {
        townSelectionMenu.SetActive(false);
    }
}
