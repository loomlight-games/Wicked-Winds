using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimator : MonoBehaviour
{
    NPC npc;

    readonly int Idle = Animator.StringToHash("Idle"),
        Moving = Animator.StringToHash("Moving");

    int currentAnimation;

    void Start(){
        npc = GetComponent<NPC>();
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
        if (npc.hasMission)
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
            npc.animator.CrossFade(newAnimation, duration);
        }
    }
}
