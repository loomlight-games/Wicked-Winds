using UnityEngine;
using UnityEngine.AI;

public class FollowingPlayerState : ICatState
{
    private CatController catController;
    private NavMeshAgent agent;
    private Transform player;
    private Transform owner;
    private float ownerReturnDistance = 2f; // Distancia mínima para que el gato regrese al dueño

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
        // Calcular la distancia al dueño
        float distanceToOwner = Vector3.Distance(catController.transform.position, owner.position);

        if (distanceToOwner < ownerReturnDistance)
        {
            // Si el gato está lo suficientemente cerca del dueño, cambia de estado
            UpdateFeedbackText("¡El gato ha vuelto con su dueño!");
            catController.ChangeState(catController.followingOwnerState);
            return;
        }

        // Continuar siguiendo al jugador
        agent.SetDestination(player.position);
    }

    public void Exit()
    {
        // Mensaje opcional al salir del estado (puedes añadir lógica aquí si es necesario)
        UpdateFeedbackText("");
    }

    // Método para actualizar el texto de feedback de forma segura
    private void UpdateFeedbackText(string text)
    {
        if (GameManager.Instance != null && GameManager.Instance.playState != null && GameManager.Instance.playState.feedBackText != null)
        {
            GameManager.Instance.playState.feedBackText.text = text;
        }
    }
}
