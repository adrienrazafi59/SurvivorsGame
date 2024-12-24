using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
   [System.Serializable]
   public class EnemyStats
{
        public string enemyName;
        public int health;
        public int damage;
        public float speed;
        public int rewardPoints;
   }

   public List<EnemyStats> enemyStatsList = new List<EnemyStats>()
   {
        new EnemyStats() { enemyName = "Enemy_BigBoss_Artillery", health = 1000, damage = 80, speed = 2f, rewardPoints = 500 },
        new EnemyStats() { enemyName = "Enemy_Boss_Laser", health = 900, damage = 70, speed = 3f, rewardPoints = 450 },
        new EnemyStats() { enemyName = "Enemy_MiniBoss_Laser", health = 600, damage = 50, speed = 4f, rewardPoints = 300 },
        new EnemyStats() { enemyName = "Enemy_MiniBoss_TankLaser", health = 650, damage = 60, speed = 3.5f, rewardPoints = 320 },
        new EnemyStats() { enemyName = "Enemy_SmallSpider", health = 200, damage = 10, speed = 8f, rewardPoints = 50 },
        new EnemyStats() { enemyName = "Enemy_SmallTank", health = 300, damage = 15, speed = 6f, rewardPoints = 75 },
        new EnemyStats() { enemyName = "Enemy_TankArtillery", health = 400, damage = 25, speed = 5f, rewardPoints = 100 },
        new EnemyStats() { enemyName = "Enemy_TankGunBattery", health = 450, damage = 30, speed = 4f, rewardPoints = 120 },
        new EnemyStats() { enemyName = "Enemy_TankLaser", health = 500, damage = 40, speed = 4.5f, rewardPoints = 150 },
   };

   private Dictionary<string, EnemyStats> enemyStatsDictionary;

   void Awake()
   {

        enemyStatsDictionary = new Dictionary<string, EnemyStats>();
        foreach (var stats in enemyStatsList)
        {
            enemyStatsDictionary.Add(stats.enemyName, stats);
        }
   }

   public EnemyStats GetEnemyStats(string enemyName)
   {
        if (enemyStatsDictionary.TryGetValue(enemyName, out EnemyStats stats))
        {
            return stats;
        }
        else
        {
            Debug.LogError($"Stats introuvables pour l'ennemi : {enemyName}");
            return null;
        }
   }

   public void InitializeEnemy(GameObject enemyObject, string enemyName)
   {
        var stats = GetEnemyStats(enemyName);

        if (stats != null)
        {
            EnemyController enemyController = enemyObject.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.Initialize(stats.health, stats.damage, stats.speed);
            }
            else
            {
                Debug.LogError($"EnemyController manquant sur {enemyObject.name}");
            }
        }
   }
}
