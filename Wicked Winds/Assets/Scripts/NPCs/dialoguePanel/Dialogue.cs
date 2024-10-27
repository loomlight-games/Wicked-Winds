using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewBehaviourScript : MonoBehaviour
{
    public TextMeshProUGUI text = null; // Para mostrar el di�logo
    public TextMeshProUGUI npcName = null;   // Para mostrar el nombre del NPC
    public string[] lines;        // Las l�neas del di�logo
    public float textSpeed;       // Velocidad del texto

    private int index;

    // Start is called before the first frame update
    void Start()
    {
      
    }
    // Update is called once per frame
    void Update()
    {
        // Aseg�rate de que lines tenga contenido antes de verificar el �ndice
        if (lines.Length == 0)
        {
            return; // Sale del m�todo si lines est� vac�o
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (index <= lines.Length) // Verifica que el �ndice est� dentro del rango
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
                AdjustTextBox(); // Llama a la funci�n para ajustar el cuadro de texto
            }
            else
            {
                Debug.LogWarning("�ndice fuera de los l�mites del arreglo 'lines'.");
            }
        }
    }

    // M�todo para iniciar el di�logo y mostrar el nombre del NPC
    public void StartDialogue(NPC npc)
    {
        text.text = string.Empty;
        npcName.text = string.Empty;

        index = 0; // Reinicia el �ndice aqu�
        npcName.text = npc.npcname; // Muestra el nombre del NPC

        // Divide el mensaje del NPC en l�neas y las almacena en el arreglo lines
        lines = npc.message.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        if (lines.Length == 0)
        {
            //Debug.LogWarning("El arreglo 'lines' est� vac�o. No hay di�logos para mostrar.");
            return; // Sale del m�todo si no hay l�neas
        }

        ActivateAllChildren(); // Activa todos los hijos del objeto padre
        StartCoroutine(TypeLine());
    }

    // Nuevo m�todo para iniciar el di�logo sin el nombre del NPC
    public void StartDialogue(string message)
    {
        
        index = 0; // Reinicia el �ndice

        lines = message.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries); // Divide el mensaje en l�neas

        if (lines.Length == 0)
        {
            Debug.LogWarning("El arreglo 'lines' est� vac�o. No hay di�logos para mostrar.");
            return; // Sale del m�todo si no hay l�neas
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
        AdjustTextBox(); // Ajusta el cuadro de texto despu�s de escribir la l�nea
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

    // M�todo para activar todos los hijos del objeto padre
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

    // M�todo para ajustar el cuadro de texto
    void AdjustTextBox()
    {
        text.ForceMeshUpdate(); // Fuerza una actualizaci�n del texto
        text.GetComponent<RectTransform>().sizeDelta = new Vector2(text.preferredWidth, text.preferredHeight); // Ajusta el tama�o del cuadro de texto
    }
}
