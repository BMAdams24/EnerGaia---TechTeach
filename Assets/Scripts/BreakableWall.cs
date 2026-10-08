using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    public float requiredForce = 50f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if (player != null && player.peBeforeDash > requiredForce)
        {
            Destroy(gameObject);
        }
    }

}
