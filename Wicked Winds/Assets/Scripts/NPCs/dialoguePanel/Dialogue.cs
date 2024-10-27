using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewBehaviourScript : MonoBehaviour
{
    public TextMeshProUGUI text = null; // Para mostrar el diálogo
    public TextMeshProUGUI npcName = null;   // Para mostrar el nombre del NPC
    public string[] lines;        // Las líneas del diálogo
    public float textSpeed;       // Velocidad del texto

    private int index;

    // Start is called before the first frame update
    void Start()
    {
      
    }
    // Update is called once per frame
    void Update()
    {
        // Asegúrate de que lines tenga contenido antes de verificar el índice
        if (lines.Length == 0)
        {
            return; // Sale del método si lines está vacío
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (index <= lines.Length) // Verifica que el índice esté dentro del rango
            {
                if (text.text == lines[index])
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    text.text = lines[index];
                }
                AdjustTextBox(); // Llama a la función para ajustar el cuadro de texto
            }
            else
            {
                Debug.LogWarning("Índice fuera de los límites del arreglo 'lines'.");
            }
        }
    }

    // Método para iniciar el diálogo y mostrar el nombre del NPC
    public void StartDialogue(NPC npc)
    {
        text.text = string.Empty;
        npcName.text = string.Empty;

        index = 0; // Reinicia el índice aquí
        npcName.text = npc.npcname; // Muestra el nombre del NPC

        // Divide el mensaje del NPC en líneas y las almacena en el arreglo lines
        lines = npc.message.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        if (lines.Length == 0)
        {
            //Debug.LogWarning("El arreglo 'lines' está vacío. No hay diálogos para mostrar.");
            return; // Sale del método si no hay líneas
        }

        ActivateAllChildren(); // Activa todos los hijos del objeto padre
        StartCoroutine(TypeLine());
    }

    // Nuevo método para iniciar el diálogo sin el nombre del NPC
    public void StartDialogue(string message)
    {
        
        index = 0; // Reinicia el índice

        lines = message.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries); // Divide el mensaje en líneas

        if (lines.Length == 0)
        {
            Debug.LogWarning("El arreglo 'lines' está vacío. No hay diálogos para mostrar.");
            return; // Sale del método si no hay líneas
        }

        ActivateAllChildren(); // Activa todos los hijos del objeto padre
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            text.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        AdjustTextBox(); // Ajusta el cuadro de texto después de escribir la línea
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            text.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            DeactivateAllChildren();
        }
    }

    // Método para activar todos los hijos del objeto padre
    void ActivateAllChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true); // Activa cada hijo
        }
    }

    
    void DeactivateAllChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false); 
        }
    }

    // Método para ajustar el cuadro de texto
    void AdjustTextBox()
    {
        text.ForceMeshUpdate(); // Fuerza una actualización del texto
        text.GetComponent<RectTransform>().sizeDelta = new Vector2(text.preferredWidth, text.preferredHeight); // Ajusta el tamaño del cuadro de texto
    }
}
