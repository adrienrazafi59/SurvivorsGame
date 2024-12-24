using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private int health;
    private int damage;
    private float speed;

    public void Initialize(int health, int damage, float speed)
    {
        this.health = health;
        this.damage = damage;
        this.speed = speed;

        Debug.Log($"{gameObject.name} initialized with {health} HP, {damage} damage, and {speed} speed.");
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}