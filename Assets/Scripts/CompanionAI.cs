using UnityEngine;

public class CompanionAI : MonoBehaviour
{
    public Transform player;
    public float followDistance = 3f;
    public float followSpeed = 8f;

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > followDistance)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                followSpeed * Time.deltaTime
            );
        }
    }
}
