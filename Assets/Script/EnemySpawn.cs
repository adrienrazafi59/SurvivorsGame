using UnityEngine;
using System.Collections.Generic;

public class EnemySpawn : MonoBehaviour
{
    public Transform player; // Référence au joueur
    public float spawnRadius = 10f; // Rayon de spawn autour du joueur
    public int spawnLimit = 50; // Limite totale d'ennemis

    private int spawnedEnemies = 0; // Nombre actuel d'ennemis spawnés
    private bool bossSpawned = false; // Indicateur pour le boss

    private List<GameObject> enemyPrefabs = new List<GameObject>(); // Liste des ennemis normaux
    private List<GameObject> bossPrefabs = new List<GameObject>(); // Liste des boss

    void Start()
    {
        // Charger tous les prefabs d'ennemis depuis le dossier Enemy
        enemyPrefabs.AddRange(Resources.LoadAll<GameObject>("Enemies/Enemy"));

        // Charger tous les prefabs de boss depuis le dossier Boss
        bossPrefabs.AddRange(Resources.LoadAll<GameObject>("Enemies/Boss"));

        if (enemyPrefabs.Count == 0 || bossPrefabs.Count == 0)
        {
            Debug.LogError("No enemy or boss prefabs found in the specified directories!");
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
        // Si le boss n'a pas encore spawn, spawn le boss
        if (!bossSpawned)
        {
            SpawnBoss();
            bossSpawned = true;
            return;
        }

        // Sinon, spawn un ennemi normal
        SpawnEnemy();
    }

    void SpawnBoss()
    {
        // Sélectionner un boss aléatoire dans la liste des boss
        GameObject bossPrefab = bossPrefabs[Random.Range(0, bossPrefabs.Count)];
        SpawnAtRandomPosition(bossPrefab);
        Debug.Log($"Boss spawned: {bossPrefab.name}");
    }

    void SpawnEnemy()
    {
        // Sélectionner un ennemi aléatoire dans la liste des ennemis
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        SpawnAtRandomPosition(enemyPrefab);
        Debug.Log($"Enemy spawned: {enemyPrefab.name}");
    }

    void SpawnAtRandomPosition(GameObject prefab)
    {
        // Calculer une position aléatoire autour du joueur
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection.y = 0; // Garder la position sur un plan horizontal
        Vector3 spawnPosition = player.position + randomDirection;

        // Instancier l'ennemi
        Instantiate(prefab, spawnPosition, Quaternion.identity);
        spawnedEnemies++;
    }
}
