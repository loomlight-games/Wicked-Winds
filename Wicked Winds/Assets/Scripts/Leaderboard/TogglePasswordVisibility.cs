using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TogglePasswordTMP : MonoBehaviour
{
    [SerializeField] private TMP_InputField passwordInputField; // Campo de contrase�a
    [SerializeField] private Button toggleButton;              // Bot�n para alternar
    [SerializeField] private Sprite showIcon;                  // �cono para mostrar contrase�a (opcional)
    [SerializeField] private Sprite hideIcon;                  // �cono para ocultar contrase�a (opcional)

    private bool isPasswordVisible = false;

    private void Start()
    {
        // Aseg�rate de asignar el evento al bot�n
        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(TogglePassword);
        }
    }

    private void TogglePassword()
    {
        isPasswordVisible = !isPasswordVisible;

        if (isPasswordVisible)
        {
            // Mostrar contrase�a
            passwordInputField.contentType = TMP_InputField.ContentType.Standard;
            if (toggleButton.image != null && showIcon != null)
                toggleButton.image.sprite = hideIcon;
        }
        else
        {
            // Ocultar contrase�a
            passwordInputField.contentType = TMP_InputField.ContentType.Password;
            if (toggleButton.image != null && hideIcon != null)
                toggleButton.image.sprite = showIcon;
        }

        // Forzar actualizaci�n para aplicar los cambios
        passwordInputField.ForceLabelUpdate();
    }
}
