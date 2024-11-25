using UnityEngine;

/// <summary>
/// Handles cat animations
/// </summary>
public class CatAnimator : MonoBehaviour
{
    CatController catController;

    readonly int Idle = Animator.StringToHash("Idle"),
        Moving = Animator.StringToHash("Moving");

    int currentAnimation;

    void Start(){
        catController = GetComponent<CatController>();
    }

    // Update is called once per frame
    public void Update()
    {
        CheckAnimation();
    }

    /// <summary>
    /// Checks animation conditions
    /// </summary>
    void CheckAnimation(){
        // Is still
        if (catController.currentStateName == "IdleState")
            ChangeAnimation(Idle);
        else 
            ChangeAnimation(Moving);
    }

    /// <summary>
    /// Transitions to given animation 
    /// </summary>
    void ChangeAnimation(int newAnimation, float duration = 0.2f){
        // Not same as current
        if (currentAnimation != newAnimation){
            currentAnimation = newAnimation;
            // Interpolate transition to new animation
            catController.animator.CrossFade(newAnimation, duration);
        }
    }
}
