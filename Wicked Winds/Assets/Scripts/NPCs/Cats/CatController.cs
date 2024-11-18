using UnityEngine.AI;
using UnityEngine;

public class CatController : MonoBehaviour
{
    public GameObject playerGameObject; // Referencia al jugador
    public Transform ownerPosition; // Referencia al dueño del gato
    public LayerMask groundLayer, buildingLayer; // Capas de interacción
    public float playerApproachSpeed = 1.5f; // Velocidad para diferenciar entre acercamiento rápido o lento
    public float fleeDistance = 10f;
    public NPC owner;
    private NavMeshAgent agent;
    private ICatState currentState;
    public Vector3 previousPlayerPosition;
    public Transform player;

    // Estados del gato
    public IdleState idleState;
    public MovingState randomMoveState;
    public ClimbingState climbingState;
    public FleeingState fleeingState;
    public FollowingPlayerState followingPlayerState;
    public FollowingOwnerState followingOwnerState;

    // Variable pública para mostrar el estado actual en el Inspector
    public string currentStateName;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = playerGameObject.transform;
        previousPlayerPosition = player.position;
        ownerPosition = owner.transform;

        // Inicializar estados
        idleState = new IdleState(this, agent);
        randomMoveState = new MovingState(this, agent);
        climbingState = new ClimbingState(this, agent);
        fleeingState = new FleeingState(this, agent, player);
        followingPlayerState = new FollowingPlayerState(this, agent, player, ownerPosition);
        followingOwnerState = new FollowingOwnerState(this, agent, player, ownerPosition, owner);

        // Estado inicial
        currentState = followingOwnerState;
        currentState.Enter();
        UpdateCurrentStateName();
    }

    void Update()
    {
        currentState?.Update();
        UpdateCurrentStateName(); // Actualiza el nombre del estado en cada frame
       
    }

    public void ChangeState(ICatState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
        UpdateCurrentStateName(); // Actualiza el nombre del estado cuando cambia
    }

    // Método para actualizar el nombre del estado actual
    void UpdateCurrentStateName()
    {
        if (currentState != null)
        {
            currentStateName = currentState.GetType().Name; // Guarda el nombre del tipo de estado
        }
    }

 

    // Nuevo método para interactuar con el gato
    public void InteractWithCat()
    {
        // Llamar al estado de seguir al jugador o hacer que el gato interactúe con el jugador
        PlayerManager.Instance.RemoveTarget(gameObject);
        // Aniade el NPC como nuevo objetivo en `currentTargets`
        PlayerManager.Instance.AddTarget(owner.gameObject);
        ChangeState(followingPlayerState);
      
    }
}
