using UnityEngine;
using UnityEngine.AI;

public class FollowingOwnerState : ICatState
{
    private CatController catController;
    private NavMeshAgent agent;
    private Transform player;
    private Transform ownerPosition;
    private NPC ownerNpc; // Aqu? almacenamos la referencia al NPC due?o para verificar su misi?n.

    public FollowingOwnerState(CatController catController, NavMeshAgent agent, Transform player, Transform owner, NPC ownerNpc)
    {
        this.catController = catController;
        this.agent = agent;
        this.player = player;
        this.ownerPosition = owner;
        this.ownerNpc = ownerNpc;  // Guardamos la referencia del NPC due?o
    }
    public void Enter()
    {
        if (ownerNpc.missionType != "CatMission")
        {
            agent.SetDestination(ownerPosition.position);
        }
        else
        {
            catController.ChangeState(catController.randomMoveState);
        }
    }


    public void Update()
    {
        // Verificamos si el due?o tiene una misi?n de tipo CatMission
        if (ownerNpc.missionType == "CatMission")
        {
            // Si la misi?n de tipo CatMission est? activa, huir del due?o
            StartCatRun();
            catController.ChangeState(catController.randomMoveState);
        }
        else
        {
            // Si no hay misi?n activa o la misi?n ha terminado, seguimos al due?o
            agent.SetDestination(ownerPosition.position);
        }
    }

    public void Exit() { }

    // M?todo para hacer que el gato huya
    private void StartCatRun()
    {
        if (ownerPosition != null)
        {
            // Huir del due?o, movi?ndonos en la direcci?n opuesta
            Vector3 fleeDirection = (ownerPosition.position - catController.transform.position).normalized; // Direcci?n opuesta al due?o
            Vector3 fleePosition = catController.transform.position + fleeDirection * 10f; // 10f es la distancia de huida

            agent.SetDestination(fleePosition);
        }
    }
}
