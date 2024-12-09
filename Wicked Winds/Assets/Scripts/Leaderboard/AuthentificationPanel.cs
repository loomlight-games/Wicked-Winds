using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuthentificationPanel : Panel
{
    [SerializeField] private TMP_InputField usernameInput = null;
    [SerializeField] private TMP_InputField passwordInput = null;

    [SerializeField] private Button signinButton = null;
    [SerializeField] private Button signupButton = null;
    //[SerializeField] Button anonymousButton = null, leaderboardButton;

    // Colores para el placeholder
    private Color errorPlaceholderColor = Color.red;
    private Color defaultPlaceholderColor = Color.gray;
    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        signinButton.onClick.AddListener(SignIn);
        signupButton.onClick.AddListener(SignUp);
        //anonymousButton.onClick.AddListener(AnonymousSignIn);
        //leaderboardButton.onClick.AddListener(Viewtable);
        
        // Restaurar el color cuando el usuario empieza a escribir
        usernameInput.onValueChanged.AddListener((text) => ResetPlaceholderColor(usernameInput));
        passwordInput.onValueChanged.AddListener((text) => ResetPlaceholderColor(passwordInput));


        base.Initialize();
    }
    public override void Open()
    {

        base.Open();
        usernameInput.ActivateInputField();
        passwordInput.DeactivateInputField();

    }
    private void AnonymousSignIn()
    {
        GameManager.Instance.SignInAnonymouslyAsync();

    }

    private void SignIn()
    {
        string user = usernameInput.text.Trim();
        string pass = passwordInput.text.Trim();

        bool isValid = true;
        if (string.IsNullOrEmpty(user))
        {
            SetPlaceholderErrorColor(usernameInput);
            isValid = false;
        }
        if (string.IsNullOrEmpty(pass))
        {
            SetPlaceholderErrorColor(passwordInput);
            isValid = false;
        }

        if (isValid)
        {
            GameManager.Instance.SignInWithUsernameAndPasswordAsync(user, pass);
            PlayerPrefs.SetString(GameManager.Instance.PLAYER_USERNAME_FILE, user);
        }
        /*
        if (string.IsNullOrEmpty(user) == false && string.IsNullOrEmpty(pass) == false)
        {
            GameManager.Instance.SignInWithUsernameAndPasswordAsync(user, pass);
        }^*/
    }
    private void SignUp()
    {
        string user = usernameInput.text.Trim();
        string pass = passwordInput.text.Trim();

        // Validar que ambos campos estén llenos
        bool isValid = true;
        if (string.IsNullOrEmpty(user))
        {
            SetPlaceholderErrorColor(usernameInput);
            isValid = false;
        }
        if (string.IsNullOrEmpty(pass))
        {
            SetPlaceholderErrorColor(passwordInput);
            isValid = false;
        }

        if (isValid)
        {
            if (IsPasswordValid(pass))
            {
                GameManager.Instance.SignUpWithUsernameAndPasswordAsync(user, pass);
                PlayerPrefs.SetString(GameManager.Instance.PLAYER_USERNAME_FILE, user);
            }
            else
            {
                ErrorPanel panel = (ErrorPanel)PanelManager.GetSingleton("error");
                panel.Open(ErrorPanel.Action.None, "Password does not match requirements. Insert at least 1 uppercase, 1 lowercase, 1 digit and 1 symbol. With minimum 8 and a maximum of 30 characters.", "OK");
            }
        }
    }
    public void PassingUsername(string username)
    {
        username = usernameInput.text.Trim();
    }
    /*
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
    }*/


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


    // UI EFFECTS
    // Método para cambiar el color del borde a rojo en caso de error
    private void SetPlaceholderErrorColor(TMP_InputField inputField)
    {
        if (inputField.placeholder is TextMeshProUGUI placeholderText)
        {
            placeholderText.color = errorPlaceholderColor;
        }
    }

    // Método para restaurar el color predeterminado cuando el usuario empieza a escribir
    private void ResetPlaceholderColor(TMP_InputField inputField)
    {
        if (inputField.placeholder is TextMeshProUGUI placeholderText)
        {
            placeholderText.color = defaultPlaceholderColor;
        }
    }
}

