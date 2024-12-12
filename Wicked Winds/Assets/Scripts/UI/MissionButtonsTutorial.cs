using TMPro;
using UnityEngine;

public class MissionButtonsTutorial : MonoBehaviour
{
    private TextMeshProUGUI textoDinamico; // Referencia al componente Text
    private GameObject textContainer, textTitle, textDescription, textSelect; // Contenedor del texto para mostrar/ocultar
    [SerializeField] private GameObject MissionsPage; // Asignado desde el Inspector

    private void Awake()
    {
        // Comprobamos si MissionsPage est� asignado desde el Inspector
        if (MissionsPage == null)
        {
            Debug.LogError("MissionsPage no est� asignado en el Inspector.");
            return;
        }

        // Buscar el contenedor dentro de MissionsPage
        Transform contenedorTransform = MissionsPage.transform.Find("Contenedor");
        if (contenedorTransform == null)
        {
            Debug.LogError("No se encontr� el objeto 'Contenedor' dentro de 'MissionsPage'. Aseg�rate de que existe en la jerarqu�a.");
            return;
        }

        textContainer = contenedorTransform.gameObject;
        textTitle = contenedorTransform.Find("TituloMisiones").gameObject;
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
            case "Letter":
                textoDinamico.text = "This mission requires you to deliver a letter to a specific NPC.";
                break;
            case "Recipe":
                textoDinamico.text = "This mission involves finding all the ingredients of a rare recipe hidden in secret locations.";
                break;
            case "Owl":
                textoDinamico.text = "This mission tasks you with rescuing a lost owl from the forest.";
                break;
            case "Cat":
                textoDinamico.text = "This mission requires you to find a missing cat in the nearby village.";
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
