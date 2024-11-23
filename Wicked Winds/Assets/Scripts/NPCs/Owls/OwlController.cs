using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class OwlController : MonoBehaviour
{
    public Transform player; // Reference to the player
    public NPC owner; // Reference to the NPC owner of the owl
    public float moveSpeed = 5f; // Movement speed of the owl
    public float detectionRadius = 10f; // Radius for detecting the player
    public float flightHeight = 10f; // Height at which the owl flies
    public float flightRange = 5f; // Random flight range while the owl is flying

    // Map boundaries (defined here)
    private Vector3 mapMinBounds = new Vector3(-175, 0, -175); // Minimum coordinates of the map
    private Vector3 mapMaxBounds = new Vector3(175, 10, 175);  // Maximum coordinates of the map

    private bool isEscaping = false;
    private bool isFollowingPlayer = false; // State for following the player
    private Vector3 targetPosition; // Target position for random flight

    void Start()
    {
        SetRandomTarget(); // Set an initial random position for flight
    }

    void Update()
    {
        if (isFollowingPlayer)
        {
            // Follow the player in the air
            Vector3 followDirection = (player.position - transform.position).normalized;
            followDirection.y = Mathf.Clamp(player.position.y + flightHeight, mapMinBounds.y, mapMaxBounds.y); // Smoothly adjust the height
            transform.position += followDirection * moveSpeed * Time.deltaTime;
        }
        else if (isEscaping)
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

    // Starts following the player after being caught
    public void StartFollowingPlayer()
    {
        isEscaping = false;
        isFollowingPlayer = true;
        // Call the follow player state or have the owl interact with the player
        PlayerManager.Instance.RemoveTarget(gameObject);
        // Add the NPC as a new target in `currentTargets`
        PlayerManager.Instance.AddTarget(owner.gameObject);
    }
}
