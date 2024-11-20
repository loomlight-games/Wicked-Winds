using UnityEngine;
using UnityEngine.AI;

public class FollowingPlayerState : ICatState
{
    private CatController catController;
    private NavMeshAgent agent;
    private Transform player;
    private Transform owner;
    private float ownerReturnDistance = 2f; // Distancia m�nima para que el gato regrese al due�o

    public FollowingPlayerState(CatController catController, NavMeshAgent agent, Transform player, Transform owner)
    {
        this.catController = catController;
        this.agent = agent;
        this.player = player;
        this.owner = owner;
    }

    public void Enter()
    {
        // Al entrar en el estado, el gato sigue al jugador
        agent.SetDestination(player.position);
        UpdateFeedbackText("SIGUIENDO AL JUGADOR");
    }

    public void Update()
    {
        // Calcular la distancia al due�o
        float distanceToOwner = Vector3.Distance(catController.transform.position, owner.position);

        if (distanceToOwner < ownerReturnDistance)
        {
            // Si el gato est� lo suficientemente cerca del due�o, cambia de estado
            UpdateFeedbackText("�El gato ha vuelto con su due�o!");
            catController.ChangeState(catController.followingOwnerState);
            return;
        }

        // Continuar siguiendo al jugador
        agent.SetDestination(player.position);
    }

    public void Exit()
    {
        // Mensaje opcional al salir del estado (puedes a�adir l�gica aqu� si es necesario)
        UpdateFeedbackText("");
    }

    // M�todo para actualizar el texto de feedback de forma segura
    private void UpdateFeedbackText(string text)
    {
        if (GameManager.Instance != null && GameManager.Instance.playState != null && GameManager.Instance.playState.feedBackText != null)
        {
            GameManager.Instance.playState.feedBackText.text = text;
        }
    }
}
