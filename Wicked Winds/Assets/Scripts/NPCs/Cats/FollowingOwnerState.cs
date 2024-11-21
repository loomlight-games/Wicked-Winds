using UnityEngine;
using UnityEngine.AI;

public class FollowingOwnerState : ICatState
{
    private CatController catController;
    private NavMeshAgent agent;
    private Transform player;
    private Transform ownerPosition;
    private NPC ownerNpc; // Aquí almacenamos la referencia al NPC dueño para verificar su misión.

    // Distancia mínima para que el gato se detenga de su dueño
    public float minFollowDistance = 2f;

    // Factor para hacer que el gato sea un poco más lento (ajusta según sea necesario)
    public float followSpeedFactor = 0.9f;

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
        if (ownerNpc.missionType != "CatMission")
        {
            // Ajustar la velocidad del gato para que sea igual o un poco más lenta que la del dueño
            NavMeshAgent ownerAgent = ownerPosition.GetComponent<NavMeshAgent>();
            if (ownerAgent != null)
            {
                agent.speed = ownerAgent.speed * followSpeedFactor;
            }
            agent.SetDestination(ownerPosition.position);
        }
        else
        {
            Debug.Log("Dueño tiene misión de tipo CatMission. Cambiando a RandomMoveState.");
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
            float distanceToOwner = Vector3.Distance(catController.transform.position, ownerPosition.position);

            // Si el gato está más lejos que la distancia mínima, sigue al dueño
            if (distanceToOwner > minFollowDistance)
            {
                agent.SetDestination(ownerPosition.position);
            }
            else
            {
                // Si está lo suficientemente cerca, el gato se detiene
                agent.ResetPath();
            }
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
