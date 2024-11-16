using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{

    public TextMeshProUGUI text = null; // Para mostrar el dialogo
    public TextMeshProUGUI npcName = null;   // Para mostrar el nombre del NPC
    public string[] lines;        // Las lineas del dialogo
    public float textSpeed;       // Velocidad del texto

    private int lineIndex;

    // Update is called once per frame
    void Update()
    {
        if (lines.Length == 0)
        {
            return; // Sale del metodo si lines esta vacio
        }

        if (PlayerManager.Instance.nextLineKey)
        {
            if (lineIndex <= lines.Length) // Verifica que el indice esta dentro del rango
            {
                if (text.text == lines[lineIndex])
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    text.text = lines[lineIndex];
                }
            }
            else
            {
                Debug.LogWarning("Indice fuera de los limites del arreglo 'lines'.");
            }
        }
    }

    // M�todo para iniciar el dialogo y mostrar el nombre del NPC
    public void StartDialogue(NPC npc)
    {
        text.text = string.Empty;
        npcName.text = string.Empty;

        lineIndex = 0; // Reinicia el indice aqui
        npcName.text = npc.npcname; // Muestra el nombre del NPC

        // Divide el mensaje del NPC en lineas y las almacena en el arreglo lines
        lines = npc.message.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        if (lines.Length == 0)
        {
            return; // Sale del metodo si no hay lineas
        }

        ActivateAllChildren(); // Activa todos los hijos del objeto padre
        NextLine();
        StartCoroutine(TypeLine());
    }

    // Nuevo m�todo para iniciar el dialogo sin el nombre del NPC
    public void StartDialogue(string message)
    {
        text.text = string.Empty;
        npcName.text = string.Empty;
        lineIndex = 0;

        lines = message.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries); // Divide el mensaje en l�neas

        if (lines.Length == 0)
        {
            Debug.LogWarning("El arreglo 'lines' esta vacio. No hay dialogos para mostrar.");
            return; // Sale del metodo si no hay lineas
        }

        ActivateAllChildren(); // Activa todos los hijos del objeto padre
        NextLine();
        StartCoroutine(TypeLine());
    }

    public void StartDialogue(NPC npc, string message)
    {
        text.text = string.Empty;
        npcName.text = string.Empty;

        lineIndex = 0;
        npcName.text = npc.npcname;

        lines = message.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries); // Divide el mensaje en lineas

        if (lines.Length == 0)
        {
            return;
        }

        ActivateAllChildren(); // Activa todos los hijos del objeto padre
        NextLine();
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[lineIndex].ToCharArray())
        {
            text.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (lineIndex < lines.Length - 1)
        {
            text.text = string.Empty;
            text.text = lines[lineIndex];
            lineIndex++;
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
}
