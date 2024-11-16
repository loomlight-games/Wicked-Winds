using UnityEngine;
using UnityEngine.AI;

public class FollowingOwnerState : ICatState
{
    private CatController catController;
    private NavMeshAgent agent;
    private Transform player;
    private Transform ownerPosition;
    private NPC ownerNpc; // Aquí almacenamos la referencia al NPC dueño para verificar su misión.

    public FollowingOwnerState(CatController catController, NavMeshAgent agent, Transform player, Transform owner, NPC ownerNpc)
    {
        this.catController = catController;
        this.agent = agent;
        this.player = player;
        this.ownerPosition = owner;
        this.ownerNpc = ownerNpc;  // Guardamos la referencia del NPC dueño
    }

    public void Enter()
    {
        // Verificamos si el dueño tiene una misión de tipo CatMission. Si no la tiene, seguimos al dueño.
        if (ownerNpc.missionType != "CatMission")
        {
            agent.SetDestination(ownerPosition.position);
        }
        else
        {
            // Si tiene una misión de tipo CatMission, el gato huye
            catController.ChangeState(catController.randomMoveState);
        }
    }

    public void Update()
    {
        // Verificamos si el dueño tiene una misión de tipo CatMission
        if (ownerNpc.missionType == "CatMission")
        {
            // Si la misión de tipo CatMission está activa, huir del dueño
            StartCatRun();
            catController.ChangeState(catController.randomMoveState);
        }
        else
        {
            // Si no hay misión activa o la misión ha terminado, seguimos al dueño
            agent.SetDestination(ownerPosition.position);
        }
    }

    public void Exit() { }

    // Método para hacer que el gato huya
    private void StartCatRun()
    {
        if (ownerPosition != null)
        {
            // Huir del dueño, moviéndonos en la dirección opuesta
            Vector3 fleeDirection = (ownerPosition.position - catController.transform.position).normalized; // Dirección opuesta al dueño
            Vector3 fleePosition = catController.transform.position + fleeDirection * 10f; // 10f es la distancia de huida

            agent.SetDestination(fleePosition);
        }
    }
}
