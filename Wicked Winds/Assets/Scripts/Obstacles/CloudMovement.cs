using System.Collections;
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    // Rango de movimiento en 3D
    [SerializeField] private Vector3 movementAreaMin = new Vector3(-175f, 40f, -175f); // Coordenadas mínimas del área
    [SerializeField] private Vector3 movementAreaMax = new Vector3(175f, 40f, 175f);   // Coordenadas máximas del área
    // Velocidad de la nube
    [SerializeField] private float speed = 2f;

    // Tiempo entre cambios de dirección
    [SerializeField] private float directionChangeInterval = 3f;

    private Vector3 targetPosition;

    private void Start()
    {
        // Generar una posición inicial aleatoria
        GenerateNewTargetPosition();
        StartCoroutine(ChangeDirectionRoutine());
    }

    private void Update()
    {
        // Mover la nube hacia la posición objetivo
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Si llega al objetivo, genera una nueva posición
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            GenerateNewTargetPosition();
        }
    }

    private void GenerateNewTargetPosition()
    {
        // Genera una nueva posición aleatoria dentro del área
        float randomX = Random.Range(movementAreaMin.x, movementAreaMax.x);
        float randomY = Random.Range(movementAreaMin.y, movementAreaMax.y);
        targetPosition = new Vector3(randomX, randomY, transform.position.z);
        PlayerManager.Instance.cloudTransform = transform;
    }

    private IEnumerator ChangeDirectionRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(directionChangeInterval);
            GenerateNewTargetPosition();
        }
    }
}
