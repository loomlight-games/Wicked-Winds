using System.Collections;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI text = null; // Para mostrar el diálogo
    public TextMeshProUGUI npcName = null;   // Para mostrar el nombre del NPC
    public string[] lines;        // Las líneas del diálogo
    public float textSpeed;       // Velocidad del texto

    private int lineIndex;
    private bool isTyping = false; // Indica si se está escribiendo texto

    private SoundManager soundManager;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    void Update()
    {
        if (lines.Length == 0)
        {
            return; // Sale si lines está vacío
        }

        if (PlayerManager.Instance.nextLineKey && !isTyping) // Solo avanza si no se está escribiendo
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
    }

    // Método para iniciar el diálogo y mostrar el nombre del NPC
    public void StartDialogue(NPC npc, string mensajito, int tipo) //0= humano, 1= gato
    {
        soundManager.PlaySoundEffect(0);
        // Activar todos los hijos del objeto
        ActivateAllChildren();
        lineIndex = 0;
        // Limpiar cualquier texto previo

        text.text = string.Empty;
        npcName.text = string.Empty;

        Debug.Log("Iniciando diálogo...");
        if(tipo == 0)
        {// Asignar el nombre del NPC
            npcName.text = npc.npcname;
            Debug.Log($"Nombre del NPC asignado: {npc.npcname}");
        }

        if (tipo == 1)
        {
            npcName.text = npc.npcname + "'s cat";
        }
      

        // Dividir el mensaje en líneas
        lines = mensajito.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        Debug.Log($"Número de líneas en el mensaje: {lines.Length}");

        if (lines.Length == 0)
        {
            Debug.LogWarning("¡No hay mensaje! Se requiere un mensaje.");
            return;
        }

        StartCoroutine(TypeLine());
        Debug.Log("Se han activado todos los hijos del objeto.");
    }

    IEnumerator TypeLine()
    {
        isTyping = true; // Inicia la escritura
        foreach (char c in lines[lineIndex].ToCharArray())
        {
            text.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        isTyping = false; // Termina de escribir
    }

    void NextLine()
    {
        if (lineIndex < lines.Length - 1)
        {
            lineIndex++;
            text.text = string.Empty; // Limpia el texto actual
            StartCoroutine(TypeLine()); // Escribe la siguiente línea
        }
        else
        {
            DeactivateAllChildren(); // Termina el diálogo
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
            child.gameObject.SetActive(false); // Desactiva todos los hijos
        }
    }

    // Método para ajustar el cuadro de texto
    void AdjustTextBox()
    {
        text.ForceMeshUpdate(); // Fuerza una actualización del texto
        text.GetComponent<RectTransform>().sizeDelta = new Vector2(text.preferredWidth, text.preferredHeight); // Ajusta el tamaño del cuadro de texto
    }
}
