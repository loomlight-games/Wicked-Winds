using UnityEngine;
using System.Collections.Generic;
public class BirdController : MonoBehaviour
{
    public Transform flockCenter; // Centro de la bandada
    public float flightRadius = 5f; // Radio de vuelo alrededor del centro
    public float moveSpeed = 2f; // Velocidad de movimiento
    public float heightOffset = 25f; // Altura fija de vuelo
    public float randomTargetInterval = 2f; // Tiempo entre objetivos aleatorios

    private Vector3 targetPosition; // Posición aleatoria dentro del radio de vuelo
    private float timer = 0f; // Temporizador para cambiar el objetivo

    void Start()
    {
        SetRandomTarget(); // Definir el primer objetivo
    }

    void Update()
    {
        // Movimiento hacia el objetivo
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Cambiar objetivo si alcanza el actual
        if (Vector3.Distance(transform.position, targetPosition) < 0.5f || timer >= randomTargetInterval)
        {
            SetRandomTarget();
            timer = 0f;
        }

        timer += Time.deltaTime;
    }

    // Define una nueva posición objetivo aleatoria dentro del radio de vuelo
    private void SetRandomTarget()
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-flightRadius, flightRadius),
            Random.Range(-5f, 10f), // Pequeñas variaciones de altura
            Random.Range(-flightRadius, flightRadius)
        );

        targetPosition = flockCenter.position + randomOffset;
        targetPosition.y = heightOffset; // Mantener la altura fija
    }
}
