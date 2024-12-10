using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomNPCMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public float range; // Radius of sphere
    public Transform centrePoint; // Centre of the area the agent wants to move around in
    public float stuckDistance = 1.0f; // Minimum distance to consider NPCs stuck
    public float checkInterval = 0.5f; // How often to check for stuck NPCs
    public LayerMask npcLayer; // Layer to identify other NPCs

    private float lastCheckTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // `avoidancePriority` is assumed to be set externally
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance;
    }

    void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance) // Done with path
        {
            MoveToRandomPoint();
        }

        // Periodically check if this NPC is stuck
        if (Time.time - lastCheckTime > checkInterval)
        {
            lastCheckTime = Time.time;
            HandleStuckNPC();
        }
    }

    void MoveToRandomPoint()
    {
        Vector3 point;
        if (RandomPoint(centrePoint.position, range, out point))
        {
            Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); // Visualize the point
            agent.SetDestination(point);
        }
    }

    void HandleStuckNPC()
    {
        Collider[] nearbyNPCs = Physics.OverlapSphere(transform.position, stuckDistance, npcLayer);
        foreach (Collider npc in nearbyNPCs)
        {
            if (npc.gameObject != gameObject) // Avoid detecting itself
            {
                Debug.Log($"{gameObject.name} is stuck with {npc.gameObject.name}. Moving...");
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
        Gizmos.DrawWireSphere(transform.position, stuckDistance);
    }
}
