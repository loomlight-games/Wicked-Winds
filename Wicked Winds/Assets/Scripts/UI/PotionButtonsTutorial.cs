using TMPro;
using UnityEngine;

public class PotionButtonsTutorial : MonoBehaviour
{
    private TextMeshProUGUI textoDinamico, tituloDinamico; // Referencias a los componentes de texto dinámico
    private GameObject textContainer, textTitle, textDescription, textSelect, barraDinamica; // Contenedor del texto y barra dinámica
    [SerializeField] private GameObject PotionsPage; // Asignado desde el Inspector

    // Barras asignadas desde el Inspector
    [SerializeField] private GameObject FlyHighBar;
    [SerializeField] private GameObject HighSpeedBar;

    private void Awake()
    {
        // Verificar si PotionsPage está asignado
        if (PotionsPage == null)
        {
            Debug.LogError("PotionsPage no está asignado en el Inspector.");
            return;
        }

        // Buscar el contenedor dentro de PotionsPage
        Transform contenedorTransform = PotionsPage.transform.Find("Contenedor");
        if (contenedorTransform == null)
        {
            Debug.LogError("No se encontró el objeto 'Contenedor' dentro de 'PotionsPage'.");
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
            Debug.LogError("No se encontró un componente Text dentro del contenedor 'Descripcion'.");
        }

        if (tituloDinamico == null)
        {
            Debug.LogError("No se encontró un componente Text dentro del contenedor 'TituloPociones'.");
        }

        // Ocultar la descripción y las barras al principio
        textDescription.SetActive(false);
        if (FlyHighBar != null) FlyHighBar.SetActive(false);
        if (HighSpeedBar != null) HighSpeedBar.SetActive(false);
    }

    public void MostrarTexto(string botonPresionado)
    {
        if (textoDinamico == null || tituloDinamico == null || textContainer == null)
        {
            Debug.LogError("El texto dinámico, el título dinámico o el contenedor de texto no están configurados.");
            return;
        }

        // Reiniciar barra dinámica previa si existe
        if (barraDinamica != null)
        {
            barraDinamica.SetActive(false); // Ocultar barra anterior
            barraDinamica = null; // Limpiar referencia
        }

        // Cambiar el texto según el botón presionado y asignar barra dinámica
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

        // Activar la barra dinámica si está asignada
        if (barraDinamica != null)
        {
            barraDinamica.SetActive(true);
        }

        // Activar el contenedor de texto de descripción para mostrarlo
        textDescription.SetActive(true);
    }

    public void OcultarTexto()
    {
        // Ocultar el contenedor cuando no se necesite mostrar el texto
        if (textDescription != null)
        {
            textDescription.SetActive(false);
        }

        // Ocultar barra dinámica si está activa
        if (barraDinamica != null)
        {
            barraDinamica.SetActive(false);
        }
    }
}
