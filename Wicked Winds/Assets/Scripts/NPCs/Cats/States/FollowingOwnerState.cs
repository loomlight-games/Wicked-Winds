using UnityEngine;
using UnityEngine.AI;

public class FollowingOwnerState : AState
{
    readonly CatController catController;

    public FollowingOwnerState(CatController catController)
    {
        this.catController = catController;
    }

    public override void Enter()
    {
        NavMeshAgent ownerAgent = catController.owner.transform.GetComponent<NavMeshAgent>();

        // Maintain speed relative to owner's
        if (ownerAgent != null)
            catController.agent.speed = ownerAgent.speed * catController.followSpeedFactor;
    }

    public override void Update()
    {
        // Owner has the cat mission type
        if (catController.owner.missionType == "CatMission")
            // Cat moves randomly
            catController.SwitchState(catController.randomMoveState);
        else
        {
            // Distance to owner is greater than minimum
            if (catController.distanceToOwner > catController.stopDistance)
            {
                catController.agent.isStopped = false; // Moves
                catController.agent.SetDestination(catController.owner.gameObject.transform.position);
            }
            else // Distance has reached minimum
                catController.agent.isStopped = true; // Stops
        }
    }
}
