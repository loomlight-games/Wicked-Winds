using TMPro;
using UnityEngine;

public class PotionButtonsTutorial : MonoBehaviour
{
    private TextMeshProUGUI textoDinamico, tituloDinamico; // Referencias a los componentes de texto din�mico
    private GameObject textContainer, textTitle, textDescription, textSelect, barraDinamica; // Contenedor del texto y barra din�mica
    [SerializeField] private GameObject PotionsPage; // Asignado desde el Inspector

    // Barras asignadas desde el Inspector
    [SerializeField] private GameObject FlyHighBar;
    [SerializeField] private GameObject HighSpeedBar;

    private void Awake()
    {
        // Verificar si PotionsPage est� asignado
        if (PotionsPage == null)
        {
            Debug.LogError("PotionsPage no est� asignado en el Inspector.");
            return;
        }

        // Buscar el contenedor dentro de PotionsPage
        Transform contenedorTransform = PotionsPage.transform.Find("Contenedor");
        if (contenedorTransform == null)
        {
            Debug.LogError("No se encontr� el objeto 'Contenedor' dentro de 'PotionsPage'.");
            return;
        }

        textContainer = contenedorTransform.gameObject;
        textTitle = contenedorTransform.Find("TituloPociones").gameObject;
        textDescription = contenedorTransform.Find("Descripcion").gameObject;
        textSelect = contenedorTransform.Find("SelectText").gameObject;

        // Buscar los componentes Text dentro del contenedor
        textoDinamico = textDescription.GetComponent<TextMeshProUGUI>();
        tituloDinamico = textTitle.GetComponent<TextMeshProUGUI>();

        if (textoDinamico == null)
        {
            Debug.LogError("No se encontr� un componente Text dentro del contenedor 'Descripcion'.");
        }

        if (tituloDinamico == null)
        {
            Debug.LogError("No se encontr� un componente Text dentro del contenedor 'TituloPociones'.");
        }

        // Ocultar la descripci�n y las barras al principio
        textDescription.SetActive(false);
        if (FlyHighBar != null) FlyHighBar.SetActive(false);
        if (HighSpeedBar != null) HighSpeedBar.SetActive(false);
    }

    public void MostrarTexto(string botonPresionado)
    {
        if (textoDinamico == null || tituloDinamico == null || textContainer == null)
        {
            Debug.LogError("El texto din�mico, el t�tulo din�mico o el contenedor de texto no est�n configurados.");
            return;
        }

        // Reiniciar barra din�mica previa si existe
        if (barraDinamica != null)
        {
            barraDinamica.SetActive(false); // Ocultar barra anterior
            barraDinamica = null; // Limpiar referencia
        }

        // Cambiar el texto seg�n el bot�n presionado y asignar barra din�mica
        switch (botonPresionado)
        {
            case "FlyHigh":
                tituloDinamico.text = "Fly High Potion";
                textoDinamico.text = "This potion allows you to fly until your stamina runs out.";
                barraDinamica = FlyHighBar; // Asignar FlyHighBar
                break;
            case "Speed":
                tituloDinamico.text = "Speed Potion";
                textoDinamico.text = "This potion allows you to sprint until your stamina runs out.";
                barraDinamica = HighSpeedBar; // Asignar HighSpeedBar
                break;
            case "Birds":
                tituloDinamico.text = "Birds Potion";
                textoDinamico.text = "This potion clears the sky of pesky birds, allowing you to fly freely without obstacles for 20 seconds.";
                break;
            case "Teleport":
                tituloDinamico.text = "Teleport Potion";
                textoDinamico.text = "This potion teleports you close to your mission objective, bringing you one step closer to success.";
                break;
            case "Fog":
                tituloDinamico.text = "Fog Potion";
                textoDinamico.text = "Sometimes, the weather works against you, and the fog hides the arrow above your head. This potion frees you from its effects, revealing the arrow guiding you to your objective for 20 seconds.";
                break;
            default:
                tituloDinamico.text = "Unknown Potion";
                textoDinamico.text = "Invalid option.";
                break;
        }

        // Activar la barra din�mica si est� asignada
        if (barraDinamica != null)
        {
            barraDinamica.SetActive(true);
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

        // Ocultar barra din�mica si est� activa
        if (barraDinamica != null)
        {
            barraDinamica.SetActive(false);
        }
    }
}
