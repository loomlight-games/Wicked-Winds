using UnityEngine;

/// <summary>
/// Defines a common class for all animation controlles.
/// Handles animation transitions.
/// </summary>
[RequireComponent(typeof(Animator))]
public abstract class AAnimationController : AStateController
{
    protected Animator animator;
    protected int currentAnimation;

    // Specific animations must be defined in derived classes

    public override void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void Update()
    {
        currentState?.Update();

        UpdateFrame();

        CheckAnimation();
    }

    /// <summary>
    /// Checks animation conditions.
    /// </summary>
    public abstract void CheckAnimation();

    /// <summary>
    /// Crossfade to new animation.
    /// </summary>
    public virtual void ChangeAnimationTo(int newAnimation, float duration = 0.2f)
    {
        // Not same as current
        if (currentAnimation != newAnimation)
        {
            currentAnimation = newAnimation;
            // Interpolate transition to new animation
            animator.CrossFade(newAnimation, duration);
        }
    }
}
