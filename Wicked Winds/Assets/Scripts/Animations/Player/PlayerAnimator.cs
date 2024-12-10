using UnityEngine;

/// <summary>
/// Handles player animations
/// </summary>
public class PlayerAnimator
{
    readonly int Idle = Animator.StringToHash("Idle"),
        Moving = Animator.StringToHash("Moving"),
        MovingFast = Animator.StringToHash("MovingFast");

    int currentAnimation;

    // Update is called once per frame
    public void Update()
    {
        CheckAnimation();
    }

    /// <summary>
    /// Checks animation conditions
    /// </summary>
    void CheckAnimation(){
        // Is moving
        if(PlayerManager.Instance.movement2D.sqrMagnitude != 0){
            // Moving fast
            if (PlayerManager.Instance.runKey || PlayerManager.Instance.runJoystick)
                ChangeAnimation(MovingFast);
            // Not moving fast
            else
                ChangeAnimation(Moving);
        } // Is still
        else
            ChangeAnimation(Idle);
    }

    /// <summary>
    /// Transitions to given animation 
    /// </summary>
    void ChangeAnimation(int newAnimation, float duration = 0.2f){
        // Not same as current
        if (currentAnimation != newAnimation){
            currentAnimation = newAnimation;
            // Interpolate transition to new animation
            PlayerManager.Instance.animator.CrossFade(newAnimation, duration);
        }
    }
}
