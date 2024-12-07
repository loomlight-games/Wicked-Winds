using UnityEngine.AI;
using UnityEngine;

public class CatController : AStateController
{
    public Transform ownerPosition;
    public LayerMask groundLayer, buildingLayer;
    public float playerApproachSpeed = 1.5f;
    public float fleeDistance = 10f;
    public NPC owner;
    private NavMeshAgent agent;
    public Vector3 previousPlayerPosition;

    // STATES
    public IdleState idleState;
    public MovingState randomMoveState;
    public ClimbingState climbingState;
    public FleeingState fleeingState;
    public FollowingPlayerState followingPlayerState;
    public FollowingOwnerState followingOwnerState;

    public string currentStateName;

    public Animator animator;

    public override void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ownerPosition = owner.transform;

        // Inicializar estados
        idleState = new IdleState(this, agent);
        randomMoveState = new MovingState(this, agent);
        climbingState = new ClimbingState(this, agent);
        followingPlayerState = new FollowingPlayerState(this, agent, ownerPosition);
        followingOwnerState = new FollowingOwnerState(this, agent, PlayerManager.Instance.transform, ownerPosition, owner);

        // Estado inicial
        currentState = followingOwnerState;
        currentState.Enter();
        UpdateCurrentStateName();
    }

    public override void UpdateFrame()
    {
        UpdateCurrentStateName(); // Actualiza el nombre del estado en cada frame
    }

    public void ChangeState(AState newState)
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
            currentStateName = currentState.GetType().Name;
        }
    }

    /// <summary>
    /// Interacts if this cat is a target
    /// </summary>
    public void Interact()
    {
        // Player has mission assigned
        if (PlayerManager.Instance.hasActiveMission &&
            PlayerManager.Instance.currentTargets.Contains(gameObject))
        {
            ChangeState(followingPlayerState);

            PlayerManager.Instance.RemoveTarget(gameObject);

            // Next target is cat owner
            PlayerManager.Instance.AddTarget(owner.gameObject);
        }
    }
}
