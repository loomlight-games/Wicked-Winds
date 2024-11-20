using UnityEngine;

[RequireComponent(typeof(CatController))]
public class InteractableCat : MonoBehaviour
{
    public CatController cat; // Referencia al NPC con el que se interactua
    public NPC owner;
    public Dialogue dialoguePanel; // Referencia al script del bocadillo de texto

    private void Start()
    {

        cat = GetComponent<CatController>();

        owner = cat.owner;
    }

    // Metodo para manejar la interaccion con el NPC
    public void Interact()
    {

        // Has mission assigned
        if (PlayerManager.Instance.hasActiveMission)
        {
            if (PlayerManager.Instance.currentTargets.Contains(gameObject))
            {


            }


        }
    }
}
