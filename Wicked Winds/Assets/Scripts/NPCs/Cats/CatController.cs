using UnityEngine;
using UnityEngine.AI;

public class CatController : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public Transform ownerPosition; // Referencia al due�o del gato
    public LayerMask groundLayer, buildingLayer; // Capas de interacci�n
    public float detectionRadius = 10f;
    public float playerSpeedThreshold = 2f; // Velocidad para decidir si el jugador es lento o r�pido
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

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
        player = playerGameObject.transform;

        // Inicializar estados
        idleState = new IdleState(this, agent);
        randomMoveState = new MovingState(this, agent);
        climbingState = new ClimbingState(this, agent);
        fleeingState = new FleeingState(this, agent, player);
        followingPlayerState = new FollowingPlayerState(this, agent, player, ownerPosition);
        followingOwnerState = new FollowingOwnerState(this, agent, player,  ownerPosition, owner);

        // Estado inicial
        currentState = idleState;
        currentState.Enter();
    }

    void Update()
    {
        currentState.Update();
    }

    public void ChangeState(ICatState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
