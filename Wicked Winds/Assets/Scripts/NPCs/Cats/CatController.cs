using UnityEngine.AI;
using UnityEngine;

public class CatController : MonoBehaviour
{
    public GameObject playerGameObject; // Referencia al jugador
    public Transform ownerPosition; // Referencia al due?o del gato
    public LayerMask groundLayer, buildingLayer; // Capas de interacci?n
    public float playerApproachSpeed = 1.5f; // Velocidad para diferenciar entre acercamiento r?pido o lento
    public float fleeDistance = 10f;
    public NPC owner;
    private NavMeshAgent agent;
    private ICatState currentState;
    public Vector3 previousPlayerPosition;
    public Transform player;
    public CatController cat;

    // Estados del gato
    public IdleState idleState;
    public MovingState randomMoveState;
    public ClimbingState climbingState;
    public FleeingState fleeingState;
    public FollowingPlayerState followingPlayerState;
    public FollowingOwnerState followingOwnerState;

    // Variable p?blica para mostrar el estado actual en el Inspector
    public string currentStateName;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = playerGameObject.transform;
        previousPlayerPosition = player.position;
        ownerPosition = owner.transform;

        // Inicializar estados
        idleState = new IdleState(cat, agent);
        randomMoveState = new MovingState(cat, agent);
        climbingState = new ClimbingState(cat, agent);
        fleeingState = new FleeingState(cat, agent, player);
        followingPlayerState = new FollowingPlayerState(cat, agent, player, ownerPosition);
        followingOwnerState = new FollowingOwnerState(cat, agent, player, ownerPosition, owner);

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

    // M?todo para actualizar el nombre del estado actual
    void UpdateCurrentStateName()
    {
        if (currentState != null)
        {
            currentStateName = currentState.GetType().Name; // Guarda el nombre del tipo de estado
        }
    }



    // Nuevo m?todo para interactuar con el gato
    public void InteractWithCat(CatController cat)
    {
        // Llamar al estado de seguir al jugador o hacer que el gato interact?e con el jugador
        PlayerManager.Instance.RemoveTarget(gameObject);
        // Aniade el NPC como nuevo objetivo en `currentTargets`
        PlayerManager.Instance.AddTarget(owner.gameObject);
        ChangeState(followingPlayerState);

    }
}
