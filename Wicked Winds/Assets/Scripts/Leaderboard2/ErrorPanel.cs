using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

public class ErrorPanel : Panel
{
    [SerializeField] private TextMeshProUGUI errorText = null;
    [SerializeField] private TextMeshProUGUI buttonText = null;
    [SerializeField] private Button actionButton = null;

    /// <summary>
    ///  when the player clicks on the action button it does a different action depending of the error
    /// </summary>
    public enum Action
    {
        None = 0, StartService = 1, SignIn = 2, OpenAuthMenu = 3
    }

    private Action action = Action.None;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        actionButton.onClick.AddListener(ButtonAction);
        base.Initialize();
    }

    public override void Open()
    {  
        action = Action.None;
        base.Open();
    }


    public void Open(Action action, string error, string button)
    {
        Open();
        this.action = action;
        if (string.IsNullOrEmpty(error) == false)
        {
            errorText.text = error;
        }
        if (string.IsNullOrEmpty(button) == false)
        {
            buttonText.text = button;
        }
    }

    private void ButtonAction()
    {
        Close();
        //Debug.Log("action none");
        //AuthenticationService.Instance.SignOut();
        switch (action)
        {
            case Action.StartService:
                GameManager.Instance.StartClientService();
                break;
            case Action.SignIn:
                GameManager.Instance.SignInAnonymouslyAsync();
                break;
            case Action.OpenAuthMenu:
                PanelManager.CloseAll();
                PanelManager.Open("auth");
                break;
        }
    }
}
