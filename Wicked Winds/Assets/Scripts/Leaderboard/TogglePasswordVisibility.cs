using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TogglePasswordTMP : MonoBehaviour
{
    [SerializeField] private TMP_InputField passwordInputField; // Campo de contraseña
    [SerializeField] private Button toggleButton;              // Botón para alternar
    [SerializeField] private Sprite showIcon;                  // Ícono para mostrar contraseña (opcional)
    [SerializeField] private Sprite hideIcon;                  // Ícono para ocultar contraseña (opcional)

    private bool isPasswordVisible = false;

    private void Start()
    {
        // Asegúrate de asignar el evento al botón
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
            // Mostrar contraseña
            passwordInputField.contentType = TMP_InputField.ContentType.Standard;
            if (toggleButton.image != null && showIcon != null)
                toggleButton.image.sprite = hideIcon;
        }
        else
        {
            // Ocultar contraseña
            passwordInputField.contentType = TMP_InputField.ContentType.Password;
            if (toggleButton.image != null && hideIcon != null)
                toggleButton.image.sprite = showIcon;
        }

        // Forzar actualización para aplicar los cambios
        passwordInputField.ForceLabelUpdate();
    }
}
