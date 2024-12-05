using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class MultiAgentNavMeshGenerator : MonoBehaviour
{
    public NavMeshSurface humanoidNavMeshSurface;
    public NavMeshSurface catNavMeshSurface;

    public void GenerateNavMesh()
    {
        humanoidNavMeshSurface.BuildNavMesh(); // Genera NavMesh para humanoides
        catNavMeshSurface.BuildNavMesh();      // Genera NavMesh para gatos
    }
}