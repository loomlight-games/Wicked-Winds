using UnityEngine;
using UnityEngine.AI;

public class FollowingPlayerState : AState
{
    readonly CatController catController;

    public FollowingPlayerState(CatController catController)
    {
        this.catController = catController;
    }

    public override void Update()
    {
        // Distance to player is greater than minimum
        if (catController.distanceToPlayer > catController.stopDistance)
        {
            catController.agent.isStopped = false; // Moves
            catController.agent.SetDestination(PlayerManager.Instance.transform.position);
        }
        else // Distance has reached minimum
            catController.agent.isStopped = true; // Stops

        // Distance to owner is smaller than minimum
        if (catController.distanceToOwner < catController.stopDistance)
        {
            catController.SwitchState(catController.followingOwnerState);
        }
    }
}
