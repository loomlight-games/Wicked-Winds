using UnityEngine.AI;
using UnityEngine;

public class CatController : AAnimationController
{
    public string currentStateName; // For debugging
    public float stopDistance = 2f,
        followSpeedFactor = 0.9f,
        distanceToPlayer,
        distanceToOwner,
        minIdleTime = 5f,
        maxIdleTime = 10f;
    public NpcController owner;
    public LayerMask buildingLayer;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public RaycastHit hit;

    #region STATES
    public IdleState idleState;
    public MovingState randomMoveState;
    public ClimbingState climbingState;
    public FollowingPlayerState followingPlayerState;
    public FollowingOwnerState followingOwnerState;
    #endregion

    #region ANIMATIONS
    readonly int IdleAnimation = Animator.StringToHash("Idle"),
                MovingAnimation = Animator.StringToHash("Moving");
    #endregion

    public override void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        idleState = new IdleState(this);
        randomMoveState = new MovingState(this);
        climbingState = new ClimbingState(this);
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
            SoundManager.PlaySound(SoundType.Cat);

            SwitchState(followingPlayerState);

            PlayerManager.Instance.RemoveTarget(gameObject);

            // Next target is cat owner
            PlayerManager.Instance.AddTarget(owner.gameObject);
        }
    }

    public override void CheckAnimation()
    {
        // Is still
        if (GetState() == idleState)
            ChangeAnimationTo(IdleAnimation);
        else
            ChangeAnimationTo(MovingAnimation);
    }
}
