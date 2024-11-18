using UnityEngine;
using UnityEngine.AI;

public class FollowingPlayerState : ICatState
{
    private CatController catController;
    private NavMeshAgent agent;
    private Transform player;
    private Transform owner;

    public FollowingPlayerState(CatController catController, NavMeshAgent agent, Transform player, Transform owner)
    {
        this.catController = catController;
        this.agent = agent;
        this.player = player;
        this.owner = owner;
    }

    public void Enter()
    {
        agent.SetDestination(player.position);
    }

    public void Update()
    {
        if (Vector3.Distance(catController.transform.position, owner.position) < 2f)
        {
            Debug.Log("¡El gato ha vuelto con su dueño!");
            catController.ChangeState(catController.followingOwnerState);
        }

        agent.SetDestination(player.position);
    }


    public void Exit() { }
}
