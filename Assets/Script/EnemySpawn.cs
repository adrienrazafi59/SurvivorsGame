using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;

    public float spawnRadius = 10f;
    public int spawnLimit = 100;

    private int spawnedEnemies = 0;

    void Start()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy prefab is not assigned!");
            return;
        }

        if (player == null)
        {
            Debug.LogError("Player reference is not assigned!");
            return;
        }
    }

    void Update()
    {
        if (spawnedEnemies < spawnLimit)
        {
            Spawn();
        }
    }

    void Spawn()
    {
        if (enemyPrefab == null || player == null || spawnedEnemies >= spawnLimit)
        {
            return;
        }

        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection.y = 0;
        Vector3 spawnPosition = player.position + randomDirection;

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        spawnedEnemies++;

        Enemy enemyScript = enemy.AddComponent<Enemy>();  // Adding Enemy script dynamically

        // Abonnement à l'événement OnDeath du script Enemy pour réduire le nombre d'ennemis
        //enemyScript.OnDeath += OnEnemyDeath;
    }

    void OnEnemyDeath()
    {
        spawnedEnemies--;
        Debug.Log("-1 Enemy");
    }

    public class Enemy : MonoBehaviour
    {
        public delegate void DeathAction();
        public event DeathAction OnDeath;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Die();
                Debug.Log("Die");
            }
        }

        void Die()
        {
            OnDeath?.Invoke();
            Destroy(gameObject);
            Debug.Log("Die");
        }
    }
}
