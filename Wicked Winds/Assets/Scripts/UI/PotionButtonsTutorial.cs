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
            Debug.LogError("No se encontr� el objeto 'Contenedor'. Aseg�rate de que est� al mismo nivel que 'LevelPages' en la jerarqu�a.");
            return;
        }

        textContainer = contenedorTransform.gameObject;

        // Buscar el componente Text dentro del contenedor
        textoDinamico = textContainer.GetComponentInChildren<TextMeshProUGUI>();
        if (textoDinamico == null)
        {
            Debug.LogError("No se encontr� un componente Text dentro del contenedor 'Contenedor'.");
        }

        // Ocultar el contenedor inicialmente
        textContainer.SetActive(false);
    }

    public void MostrarTexto(string botonPresionado)
    {
        if (textoDinamico == null || textContainer == null)
        {
            Debug.LogError("El texto din�mico o el contenedor de texto no est�n configurados.");
            return;
        }

        // Cambiar el texto seg�n el bot�n presionado
        switch (botonPresionado)
        {
            case "FlyHigh":
                textoDinamico.text = "Has seleccionado volar alto.";
                break;
            case "Speed":
                textoDinamico.text = "Has seleccionado aumentar la velocidad.";
                break;
            case "Birds":
                textoDinamico.text = "Has seleccionado invocar a los p�jaros.";
                break;
            case "Teleport":
                textoDinamico.text = "Has seleccionado teletransportarte.";
                break;
            case "Fog":
                textoDinamico.text = "Has seleccionado crear niebla.";
                break;
            default:
                textoDinamico.text = "Opci�n no v�lida.";
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
