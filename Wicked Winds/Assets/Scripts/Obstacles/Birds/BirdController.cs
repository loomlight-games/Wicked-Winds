using UnityEngine;
using System.Collections.Generic;

public class BirdController : MonoBehaviour
{
    public Transform flockCenter; // Centro de la bandada
    public float flightRadius = 15f; // Radio de vuelo alrededor del centro
    public float moveSpeed = 2f; // Velocidad de movimiento
    public float minHeightOffset = 12f; // Altura m�nima de vuelo
    public float maxHeightOffset = 30f; // Altura m�xima de vuelo
    public float randomTargetInterval = 2f; // Tiempo entre objetivos aleatorios

    private Vector3 targetPosition; // Posici�n aleatoria dentro del radio de vuelo
    private float timer = 0f; // Temporizador para cambiar el objetivo

    void Start()
    {
        SetRandomTarget(); // Definir el primer objetivo
    }

    void Update()
    {
        // Movimiento hacia el objetivo
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Calcular direcci�n y ajustar rotaci�n para mirar hacia la posici�n objetivo
        Vector3 direction = targetPosition - transform.position;

        // Evitar rotaci�n si el vector direcci�n es muy peque�o
        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Aumentar velocidad de rotaci�n
            float rotationSpeed = 5f; // Controla la rapidez del giro
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        // Cambiar objetivo si alcanza el actual
        if (Vector3.Distance(transform.position, targetPosition) < 0.5f || timer >= randomTargetInterval)
        {
            SetRandomTarget();
            timer = 0f;
        }

        timer += Time.deltaTime;
    }

    // Define una nueva posici�n objetivo aleatoria dentro del radio de vuelo
    private void SetRandomTarget()
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-flightRadius, flightRadius),
            Random.Range(-5f, 10f), // Peque�as variaciones de altura
            Random.Range(-flightRadius, flightRadius)
        );

        targetPosition = flockCenter.position + randomOffset;

        // Asignar una altura aleatoria entre minHeightOffset y maxHeightOffset
        targetPosition.y = Random.Range(minHeightOffset, maxHeightOffset);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Cambia de direcci�n o realiza otra acci�n
            SetRandomTarget();
            Debug.Log("Bird collided with the player and changed direction!");
        }
    }
}
