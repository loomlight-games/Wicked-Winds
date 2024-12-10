using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomNPCMovement
{
    readonly NpcController controller;
    private float lastCheckTime;

    public RandomNPCMovement(NpcController controller)
    {
        this.controller = controller;
    }

    public void Update()
    {
        if (controller.agent.remainingDistance <= controller.agent.stoppingDistance) // Done with path
        {
            MoveToRandomPoint();
        }

        // Periodically check if this NPC is stuck
        if (Time.time - lastCheckTime > controller.checkInterval)
        {
            lastCheckTime = Time.time;
            HandleStuckNPC();
        }
    }

    void MoveToRandomPoint()
    {
        Vector3 point;
        if (RandomPoint(controller.transform.position, controller.range, out point))
        {
            Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); // Visualize the point
            controller.agent.SetDestination(point);
        }
    }

    void HandleStuckNPC()
    {
        Collider[] nearbyNPCs = Physics.OverlapSphere(controller.transform.position, controller.stuckDistance, controller.npcLayer);
        foreach (Collider npc in nearbyNPCs)
        {
            if (npc.gameObject != controller.gameObject) // Avoid detecting itself
            {
                Debug.Log($"{controller.gameObject.name} is stuck with {npc.gameObject.name}. Moving...");
                MoveToRandomPoint();
                break;
            }
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range; // Random point in a sphere
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the stuck detection radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controller.transform.position, controller.stuckDistance);
    }
}
