using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyFollow : MonoBehaviour
{
    private Transform player;
    private Rigidbody rb;
    private float speed;

    public float minSpeed = 2f;
    public float maxSpeed = 4f;
    public float stopDistance = 1.5f;
    public float slowDownDistance = 3f;
    public float rotationSpeed = 5f;

    void Start()
    {
        // Find player
        player = GameObject.Find("Player")?.transform;

        if (player == null)
        {
            Debug.LogError("Aucun objet nommé 'Player' trouvé dans la scène. Assurez-vous que le joueur est bien nommé 'Player'.");
            enabled = false;
            return;
        }

        // Initialize Rigidbody
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent unwanted physics rotations

        // Randomize speed
        speed = Random.Range(minSpeed, maxSpeed);
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > stopDistance)
        {
            // Calculate direction
            Vector3 direction = (player.position - transform.position).normalized;

            // Slow down if within slowDownDistance
            float currentSpeed = speed;
            if (distanceToPlayer < slowDownDistance)
            {
                currentSpeed *= (distanceToPlayer - stopDistance) / (slowDownDistance - stopDistance);
            }

            // Move enemy
            Vector3 movePosition = transform.position + direction * currentSpeed * Time.fixedDeltaTime;
            rb.MovePosition(movePosition);

            // Smoothly rotate towards player
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}