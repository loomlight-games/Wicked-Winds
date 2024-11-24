using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class OwlController : MonoBehaviour
{
    public Transform player; // Reference to the player
    public NPC owner; // Reference to the NPC owner of the owl
    public float moveSpeed = 2f; // Movement speed of the owl (This will be overwritten by PlayerManager)
    public float detectionRadius = 10f; // Radius for detecting the player
    public float flightHeight = 15f; // Height at which the owl flies
    public float flightRange = 5f; // Random flight range while the owl is flying
    public float followDistance = 5f; // Distance at which the owl will follow the player
    public MissionIcon missionIcon; // Referencia al icono de mision del NPC

    // Map boundaries (defined here)
    private Vector3 mapMinBounds = new Vector3(-80, 20, -80); // Minimum coordinates of the map
    private Vector3 mapMaxBounds = new Vector3(80, 30, 80);  // Maximum coordinates of the map

    private bool isEscaping = false;
    private Vector3 targetPosition; // Target position for random flight

    void Start()
    {
        this.missionIcon = owner.missionIcon;

        SetRandomTarget(); // Set an initial random position for flight
    }

    void Update()
    {
        
        if (PlayerManager.Instance.activeMission != null &&
            this.missionIcon != null &&
            this.missionIcon.missionID == PlayerManager.Instance.activeMission.missionID)
        {
            if (!PlayerManager.Instance.currentTargets.Contains(gameObject))
            {
                PlayerManager.Instance.AddTarget(gameObject);
            }
        }
        

        if (isEscaping)
        {
            // Escape from the player in a straight line
            Vector3 escapeDirection = (transform.position - player.position).normalized;
            escapeDirection.y += flightHeight; // Raise the owl while escaping
            transform.position += escapeDirection * moveSpeed * Time.deltaTime;

            // Restrict the owl within the map boundaries
            ClampToMapBounds();
        }
        else
        {
            // Random flight while not interacting with the player
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // If the owl reaches the destination, choose a new target
            if (Vector3.Distance(transform.position, targetPosition) < 1f)
            {
                SetRandomTarget();
            }

            // Detect the player and trigger escape mode
            if (Vector3.Distance(transform.position, player.position) < detectionRadius)
            {
                isEscaping = true;
            }

            // Restrict the owl within the map boundaries
            ClampToMapBounds();
        }
    }

    // Defines a new random position within a range for the owl to fly
    void SetRandomTarget()
    {
        float randomX = Random.Range(mapMinBounds.x, mapMaxBounds.x);
        float randomY = Random.Range(mapMinBounds.y, mapMaxBounds.y);
        float randomZ = Random.Range(mapMinBounds.z, mapMaxBounds.z);

        targetPosition = new Vector3(randomX, randomY, randomZ);
    }

    // Restricts the owl's position within the map boundaries
    void ClampToMapBounds()
    {
        Vector3 clampedPosition = transform.position;

        clampedPosition.x = Mathf.Clamp(clampedPosition.x, mapMinBounds.x, mapMaxBounds.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, mapMinBounds.y, mapMaxBounds.y);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, mapMinBounds.z, mapMaxBounds.z);

        transform.position = clampedPosition;
    }

   

    // Nuevo m?todo para interactuar con el gato
    public void InteractWithOwl()
    {
        // Llamar al estado de seguir al jugador o hacer que el gato interact?e con el jugador
        PlayerManager.Instance.RemoveTarget(gameObject);
        // Aniade el NPC como nuevo objetivo en `currentTargets`
        PlayerManager.Instance.AddTarget(owner.gameObject);
        desactivarOwlUI.Instance.activateOwlUI = true;


    }
}
