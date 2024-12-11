using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    private Transform player;
    private float speed;
    public float minSpeed = 2f; 
    public float maxSpeed = 4f; 
    public float stopDistance = 1.5f; 

    void Start()
    {
        player = GameObject.Find("Player").transform;

        if (player == null)
        {
            Debug.LogError("Aucun objet nommé 'Player' trouvé dans la scène. Assurez-vous que le joueur est bien nommé 'Player'.");
            return;
        }
        speed = Random.Range(minSpeed, maxSpeed);
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > stopDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        }
    }
}