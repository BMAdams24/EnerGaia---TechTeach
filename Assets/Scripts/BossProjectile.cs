using NUnit.Framework.Interfaces;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float speed = 6f;
    public int damage = 1;

    private Vector2 moveDirection;

    public void Initialize(Transform player)
    {
        moveDirection = (player.position - transform.position).normalized;

        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg; //Rotates the projectile to face the direction
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Update()
    {
        transform.position += (Vector3)(moveDirection * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth player = collision.GetComponent<PlayerHealth>();

        if (player != null ) //If the projectile hits the player, take damage and destroy the object
        {
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
