using UnityEngine;

public class InteractableCat : MonoBehaviour
{
    public CatController cat; // Referencia al NPC con el que se interactua
    public NPC owner;

    private void Start()
    {
        cat = GetComponent<CatController>();

        owner = cat.owner;
    }

    // Metodo para manejar la interaccion con el NPC
    public void InteractCat()
    {
        // Has mission assigned
        if (PlayerManager.Instance.hasActiveMission &&
            PlayerManager.Instance.currentTargets.Contains(gameObject))
        {
            cat.ChangeState(cat.followingPlayerState);
            cat.InteractWithCat();
            GameManager.Instance.playState.feedBackText.text = "Bring the cat to its owner\n";
        }
    }
}
