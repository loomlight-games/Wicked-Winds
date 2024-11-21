using UnityEngine;
using UnityEngine.AI;

public class FollowingPlayerState : ICatState
{
    private CatController catController;
    private NavMeshAgent agent;
    private Transform player;
    private Transform owner;

    // Distancia mínima para que el gato se detenga
    public float minFollowDistance = 2f;

    // Factor para hacer que el gato sea un poco más lento (ajusta según sea necesario)
    public float followSpeedFactor = 0.9f;

    public FollowingPlayerState(CatController catController, NavMeshAgent agent, Transform owner)
    {
        this.catController = catController;
        this.agent = agent;
        this.owner = owner;

        // Buscar el jugador en la escena por su etiqueta
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("No se encontró ningún objeto con la etiqueta 'Player'. Asegúrate de que el jugador tenga esa etiqueta.");
        }
    }

    public void Enter()
    {
        if (player != null)
        {
            // Ajustar la velocidad del gato para que sea igual o un poco más lenta que la del jugador
            NavMeshAgent playerAgent = player.GetComponent<NavMeshAgent>();
            if (playerAgent != null)
            {
                agent.speed = playerAgent.speed * followSpeedFactor;
            }
            agent.SetDestination(player.position);
        }
    }

    public void Update()
    {
        if (player != null)
        {
            // Calcular la distancia entre el gato y el jugador
            float distanceToPlayer = Vector3.Distance(catController.transform.position, player.position);

            // Si el gato está a una distancia mayor que la mínima, sigue al jugador
            if (distanceToPlayer > minFollowDistance)
            {
                agent.SetDestination(player.position);
            }
            else
            {
                // Si el gato está demasiado cerca del jugador, se detiene
                agent.ResetPath(); // Detiene el movimiento del NavMeshAgent
            }

            // Condición para cambiar de estado cuando el gato está cerca de su dueño
            if (Vector3.Distance(catController.transform.position, owner.position) < 2f)
            {
                Debug.Log("¡El gato ha vuelto con su dueño!");
                catController.ChangeState(catController.followingOwnerState);
            }
        }
    }

    public void Exit() { }
}
