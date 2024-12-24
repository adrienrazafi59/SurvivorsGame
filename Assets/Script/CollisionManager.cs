using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public int playerLayer = 8;
    public int enemyLayer = 9;

    void Start()
    {
        // Ignore les collisions entre le joueur et les ennemis
        Physics.IgnoreLayerCollision(playerLayer, enemyLayer, true);

        // Permet les collisions entre les ennemis eux-mêmes
        Physics.IgnoreLayerCollision(enemyLayer, enemyLayer, false);
    }
}