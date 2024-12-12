using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionButtonsTutorial : MonoBehaviour
{
    private TextMeshProUGUI textoDinamico; // Referencia al componente Text
    private GameObject textContainer; // Contenedor del texto para mostrar/ocultar

    private void Awake()
    {
        // Buscar el objeto 'Contenedor' en el mismo nivel que 'LevelPages'
        Transform contenedorTransform = transform.parent.Find("Contenedor");
        if (contenedorTransform == null)
        {
            Debug.LogError("No se encontró el objeto 'Contenedor'. Asegúrate de que está al mismo nivel que 'LevelPages' en la jerarquía.");
            return;
        }

        textContainer = contenedorTransform.gameObject;

        // Buscar el componente Text dentro del contenedor
        textoDinamico = textContainer.GetComponentInChildren<TextMeshProUGUI>();
        if (textoDinamico == null)
        {
            Debug.LogError("No se encontró un componente Text dentro del contenedor 'Contenedor'.");
        }

        // Ocultar el contenedor inicialmente
        textContainer.SetActive(false);
    }

    public void MostrarTexto(string botonPresionado)
    {
        if (textoDinamico == null || textContainer == null)
        {
            Debug.LogError("El texto dinámico o el contenedor de texto no están configurados.");
            return;
        }

        // Cambiar el texto según el botón presionado
        switch (botonPresionado)
        {
            case "FlyHigh":
                textoDinamico.text = "Has seleccionado volar alto.";
                break;
            case "Speed":
                textoDinamico.text = "Has seleccionado aumentar la velocidad.";
                break;
            case "Birds":
                textoDinamico.text = "Has seleccionado invocar a los pájaros.";
                break;
            case "Teleport":
                textoDinamico.text = "Has seleccionado teletransportarte.";
                break;
            case "Fog":
                textoDinamico.text = "Has seleccionado crear niebla.";
                break;
            default:
                textoDinamico.text = "Opción no válida.";
                break;
        }

        // Activar el contenedor para mostrar el texto
        textContainer.SetActive(true);
    }

    public void OcultarTexto()
    {
        // Ocultar el contenedor cuando no se necesite mostrar el texto
        if (textContainer != null)
        {
            textContainer.SetActive(false);
        }
    }
}
