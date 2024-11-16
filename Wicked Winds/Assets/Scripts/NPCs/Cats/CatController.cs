using UnityEngine;
using UnityEngine.AI;

public class CatController : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public Transform ownerPosition; // Referencia al dueño del gato
    public LayerMask groundLayer, buildingLayer; // Capas de interacción
    public float detectionRadius = 10f;
    public float playerSpeedThreshold = 2f; // Velocidad para decidir si el jugador es lento o rápido
    public NPC owner;
    private NavMeshAgent agent;
    private ICatState currentState;

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
        GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
        player = playerGameObject.transform;
        ownerPosition = owner.transform; // Asignar el Transform del NPC dueño al catController

        // Inicializar estados
        idleState = new IdleState(this, agent);
        randomMoveState = new MovingState(this, agent);
        climbingState = new ClimbingState(this, agent);
        fleeingState = new FleeingState(this, agent, player);
        followingPlayerState = new FollowingPlayerState(this, agent, player, ownerPosition);
        followingOwnerState = new FollowingOwnerState(this, agent, player, ownerPosition, owner);

        // Estado inicial
        currentState = idleState;
        currentState.Enter();
        UpdateCurrentStateName();
    }

    void Update()
    {
        currentState.Update();
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
}
