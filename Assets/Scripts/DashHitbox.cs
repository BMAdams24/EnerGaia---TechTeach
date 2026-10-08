using UnityEngine;

public class DashHitbox : MonoBehaviour
{
    private float damage;

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BossController boss = collision.GetComponent<BossController>();

        if (boss != null)
        {
            boss.TakeDamage(damage);
        }
    }
}