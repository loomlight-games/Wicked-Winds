using System.Collections;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{

    public TextMeshProUGUI text = null; // Para mostrar el di�logo
    public TextMeshProUGUI npcName = null;   // Para mostrar el nombre del NPC
    public string[] lines;        // Las l�neas del di�logo
    public float textSpeed;       // Velocidad del texto

    private int lineIndex;

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

        if (PlayerManager.Instance.nextLineKey)
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

    // M�todo para iniciar el di�logo y mostrar el nombre del NPC
    

    public void StartDialogue(NPC npc, string mensajito)
    {
        // Limpiar cualquier texto previo
        text.text = string.Empty;
        npcName.text = string.Empty;

        
        Debug.Log("Iniciando diálogo...");

        // Asignar el nombre del NPC
        npcName.text = npc.npcname;
        Debug.Log($"Nombre del NPC asignado: {npc.npcname}");

        // Dividir el mensaje en líneas
        lines = mensajito.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        Debug.Log($"Número de líneas en el mensaje: {lines.Length}");

        if (lines.Length == 0)
        {
            Debug.LogWarning("¡No hay mensaje! Se requiere un mensaje.");
            return;
        }

        // Activar todos los hijos del objeto
        ActivateAllChildren();
        lineIndex = 0;
        StartCoroutine(TypeLine());
        Debug.Log("Se han activado todos los hijos del objeto.");

      

        // Si se usa la animación de escritura, descomentar la siguiente línea:
        // 
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

    // M�todo para ajustar el cuadro de texto
    void AdjustTextBox()
    {
        text.ForceMeshUpdate(); // Fuerza una actualizaci�n del texto
        text.GetComponent<RectTransform>().sizeDelta = new Vector2(text.preferredWidth, text.preferredHeight); // Ajusta el tama�o del cuadro de texto
    }
}
