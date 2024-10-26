using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthentificationPanel : Panel
{
    [SerializeField] Button anonymousButton = null;

    public override void Initialize()
    {
        if ( IsInitialized)
        {
            return;
        }
        anonymousButton.onClick.AddListener(AnonymousSignIn);
        base.Initialize();
    }
    public override void Open()
    {

        base.Open();
    }
    private void AnonymousSignIn()
    {
        GameManager.Instance.SignInAnonymouslyAsync();

    }
}

