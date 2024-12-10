using UnityEngine.AI;
using UnityEngine;

public class CatController : AStateController
{
    public Animator animator;
    public LayerMask buildingLayer;
    public NPC owner;
    public string currentStateName;
    public float minFollowDistance = 2f, // Min distance for the cat to stop
    followSpeedFactor = 0.9f,
    distanceToPlayer,
    distanceToOwner,
    minIdleTime = 5f,
    maxIdleTime = 10f;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public RaycastHit hit;

    // STATES
    public IdleState idleState;
    public MovingState randomMoveState;
    public FollowingPlayerState followingPlayerState;
    public FollowingOwnerState followingOwnerState;

    public override void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        idleState = new IdleState(this);
        randomMoveState = new MovingState(this);
        followingPlayerState = new FollowingPlayerState(this);
        followingOwnerState = new FollowingOwnerState(this);

        SetState(followingOwnerState);
    }

    public override void UpdateFrame()
    {
        // Calculate distances
        distanceToPlayer = Vector3.Distance(transform.position, PlayerManager.Instance.transform.position);
        distanceToOwner = Vector3.Distance(transform.position, owner.transform.position);

        currentStateName = currentState.ToString();
    }

    /// <summary>
    /// Interacts if this cat is the target
    /// </summary>
    public void Interact()
    {
        // Player has mission assigned
        if (PlayerManager.Instance.hasActiveMission &&
            PlayerManager.Instance.currentTargets.Contains(gameObject))
        {
            SwitchState(followingPlayerState);

            PlayerManager.Instance.RemoveTarget(gameObject);

            // Next target is cat owner
            PlayerManager.Instance.AddTarget(owner.gameObject);
        }
    }
}
