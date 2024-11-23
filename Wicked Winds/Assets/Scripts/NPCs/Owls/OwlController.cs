using UnityEngine;

public class OwlController : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public float defaultMoveSpeed = 5f; // Velocidad base del b�ho
    public float detectionRadius = 10f; // Radio de detecci�n del jugador
    public float flightHeight = 10f; // Altura a la que vuela
    public float followDistance = 3f; // Distancia detr�s del jugador al seguirlo

    // L�mites del mapa
    private Vector3 mapMinBounds = new Vector3(-175, 0, -175);
    private Vector3 mapMaxBounds = new Vector3(175, 30, 175);

    private bool isEscaping = false;
    private bool isFollowingPlayer = false;
    private Vector3 targetPosition;

    void Start()
    {
        SetRandomTarget(); // Define una posici�n aleatoria inicial
    }

    void Update()
    {
        if (isFollowingPlayer)
        {
            // Ajusta la velocidad del b�ho a la del jugador
            float playerSpeed = player.GetComponent<PlayerController>()?.moveSpeed ?? defaultMoveSpeed;
            float followSpeed = playerSpeed; // Misma velocidad que el jugador

            // Posici�n objetivo: detr�s del jugador en el plano XZ
            Vector3 followTarget = player.position - player.forward * followDistance;
            followTarget.y = flightHeight; // Mant�n la altura fija

            // Mover al b�ho hacia la posici�n objetivo
            transform.position = Vector3.MoveTowards(transform.position, followTarget, followSpeed * Time.deltaTime);
        }
        else if (isEscaping)
        {
            // Ajusta la velocidad del b�ho a un poco menos que la del jugador
            float playerSpeed = player.GetComponent<PlayerController>()?.moveSpeed ?? defaultMoveSpeed;
            float escapeSpeed = playerSpeed * 0.8f; // 80% de la velocidad del jugador

            // Huye del jugador
            Vector3 escapeDirection = (transform.position - player.position).normalized;
            escapeDirection.y = 0; // No cambia en Y
            transform.position += escapeDirection * escapeSpeed * Time.deltaTime;

            // Restringe al b�ho dentro de los l�mites del mapa
            ClampToMapBounds();
        }
        else
        {
            // Vuelo aleatorio mientras no interact�a con el jugador
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, defaultMoveSpeed * Time.deltaTime);

            // Si llega al destino, selecciona un nuevo objetivo
            if (Vector3.Distance(transform.position, targetPosition) < 1f)
            {
                SetRandomTarget();
            }

            // Detecta al jugador y activa el modo de escape
            if (Vector3.Distance(transform.position, player.position) < detectionRadius)
            {
                isEscaping = true;
            }

            // Restringe al b�ho dentro de los l�mites del mapa
            ClampToMapBounds();
        }
    }

    // Define una nueva posici�n aleatoria dentro de los l�mites del mapa
    void SetRandomTarget()
    {
        float randomX = Random.Range(mapMinBounds.x, mapMaxBounds.x);
        float randomZ = Random.Range(mapMinBounds.z, mapMaxBounds.z);

        targetPosition = new Vector3(randomX, flightHeight, randomZ); // Mant�n la altura fija
    }

    // Restringe la posici�n del b�ho dentro de los l�mites del mapa
    void ClampToMapBounds()
    {
        Vector3 clampedPosition = transform.position;

        clampedPosition.x = Mathf.Clamp(clampedPosition.x, mapMinBounds.x, mapMaxBounds.x);
        clampedPosition.y = flightHeight; // Fija la altura
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, mapMinBounds.z, mapMaxBounds.z);

        transform.position = clampedPosition;
    }

    // Inicia el seguimiento del jugador despu�s de ser atrapado
    public void StartFollowingPlayer()
    {
        isEscaping = false;
        isFollowingPlayer = true;
    }
}
