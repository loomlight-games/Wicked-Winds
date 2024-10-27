using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuthentificationPanel : Panel
{
    [SerializeField] private TMP_InputField usernameInput = null;
    [SerializeField] private TMP_InputField passwordInput = null;
    [SerializeField] private Button signinButton = null;
    [SerializeField] private Button signupButton = null;
    [SerializeField] Button anonymousButton = null, leaderboardButton;

    public override void Initialize()
    {
        if ( IsInitialized)
        {
            return;
        }
        signinButton.onClick.AddListener(SignIn);
        signupButton.onClick.AddListener(SignUp);
        anonymousButton.onClick.AddListener(AnonymousSignIn);
        leaderboardButton.onClick.AddListener(Viewtable);
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

    private void SignIn()
    {
        string user = usernameInput.text.Trim();
        string pass = passwordInput.text.Trim();
        if (string.IsNullOrEmpty(user) == false && string.IsNullOrEmpty(pass) == false)
        {
            GameManager.Instance.SignInWithUsernameAndPasswordAsync(user, pass);
        }
    }

    private void SignUp()
    {
        string user = usernameInput.text.Trim();
        string pass = passwordInput.text.Trim();
        if (string.IsNullOrEmpty(user) == false && string.IsNullOrEmpty(pass) == false)
        {
            if (IsPasswordValid(pass))
            {
                GameManager.Instance.SignUpWithUsernameAndPasswordAsync(user, pass);
            }
            else
            {
                ErrorPanel panel = (ErrorPanel)PanelManager.GetSingleton("error");
                panel.Open(ErrorPanel.Action.None, "Password does not match requirements. Insert at least 1 uppercase, 1 lowercase, 1 digit and 1 symbol. With minimum 8 and a maximum of 30 characters.", "OK");
            }
        }
    }


    private bool IsPasswordValid(string password)
    {
        if (password.Length < 8 || password.Length > 30)
        {
            return false;
        }

        bool hasUppercase = false;
        bool hasLowercase = false;
        bool hasDigit = false;
        bool hasSymbol = false;

        foreach (char c in password)
        {
            if (char.IsUpper(c))
            {
                hasUppercase = true;
            }
            else if (char.IsLower(c))
            {
                hasLowercase = true;
            }
            else if (char.IsDigit(c))
            {
                hasDigit = true;
            }
            else if (!char.IsLetterOrDigit(c))
            {
                hasSymbol = true;
            }
        }
        return hasUppercase && hasLowercase && hasDigit && hasSymbol;
    }

    void Viewtable(){
        Close();
        PanelManager.Open("Leaderboard");
    }
}

