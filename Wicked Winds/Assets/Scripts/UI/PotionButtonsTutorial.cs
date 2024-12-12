using TMPro;
using UnityEngine;

public class PotionButtonsTutorial : MonoBehaviour
{
    private TextMeshProUGUI textoDinamico; // Referencia al componente Text
    private GameObject textContainer, textTitle, textDescription, textSelect; // Contenedor del texto para mostrar/ocultar
    [SerializeField] private GameObject PotionsPage; // Asignado desde el Inspector

    private void Awake()
    {
        // Comprobamos si PotionsPage est� asignado desde el Inspector
        if (PotionsPage == null)
        {
            Debug.LogError("PotionsPage no est� asignado en el Inspector.");
            return;
        }

        // Buscar el contenedor dentro de PotionsPage
        Transform contenedorTransform = PotionsPage.transform.Find("Contenedor");
        if (contenedorTransform == null)
        {
            Debug.LogError("No se encontr� el objeto 'Contenedor' dentro de 'PotionsPage'. Aseg�rate de que existe en la jerarqu�a.");
            return;
        }

        textContainer = contenedorTransform.gameObject;
        textTitle = contenedorTransform.Find("TituloPociones").gameObject;
        textDescription = contenedorTransform.Find("Descripcion").gameObject;
        textSelect = contenedorTransform.Find("SelectText").gameObject;

        // Buscar el componente Text dentro del contenedor de Descripci�n
        textoDinamico = textDescription.GetComponent<TextMeshProUGUI>();
        if (textoDinamico == null)
        {
            Debug.LogError("No se encontr� un componente Text dentro del contenedor 'Descripcion'.");
        }

        // Ocultar la descripci�n al principio
        textDescription.SetActive(false);
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
                textoDinamico.text = "This potion allows you to fly until your stamina runs out.";
                break;
            case "Speed":
                textoDinamico.text = "This potion allows you to sprint until your stamina runs out. To use it press 'shift'.";
                break;
            case "Birds":
                textoDinamico.text = "This potion clears the sky of pesky birds, allowing you to fly freely without obstacles.";
                break;
            case "Teleport":
                textoDinamico.text = "This potion teleports you close to your mission objective, bringing you one step closer to success.";
                break;
            case "Fog":
                textoDinamico.text = "Sometimes, the weather works against you, and the fog hides the arrow above your head. This potion frees you from its effects, revealing the arrow guiding you to your objective.";
                break;
            default:
                textoDinamico.text = "Opci�n no v�lida.";
                break;
        }

        // Activar el contenedor de texto de descripci�n para mostrarlo
        textDescription.SetActive(true);
    }

    public void OcultarTexto()
    {
        // Ocultar el contenedor cuando no se necesite mostrar el texto
        if (textDescription != null)
        {
            textDescription.SetActive(false);
        }
    }
}
